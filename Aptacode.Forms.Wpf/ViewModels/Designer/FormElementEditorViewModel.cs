﻿using System;
using System.Linq;
using Aptacode.CSharp.Common.Utilities.Extensions;
using Aptacode.CSharp.Common.Utilities.Mvvm;
using Aptacode.Forms.Shared.Builders;
using Aptacode.Forms.Shared.Builders.Elements.Composite;
using Aptacode.Forms.Shared.Builders.Elements.Controls;
using Aptacode.Forms.Shared.Builders.Elements.Controls.Fields;
using Aptacode.Forms.Shared.Enums;
using Aptacode.Forms.Shared.EventListeners.Events;
using Aptacode.Forms.Shared.Interfaces;
using Aptacode.Forms.Shared.Interfaces.Composite;
using Aptacode.Forms.Shared.Interfaces.Controls;
using Aptacode.Forms.Shared.Models.Elements.Controls;
using Aptacode.Forms.Shared.Models.Elements.Controls.Fields;
using Aptacode.Forms.Shared.ViewModels;
using Aptacode.Forms.Shared.ViewModels.Elements.Composite;
using Aptacode.Forms.Shared.ViewModels.Elements.Controls;
using Aptacode.Forms.Wpf.Views.Designer;

namespace Aptacode.Forms.Wpf.ViewModels.Designer
{
    public class FormElementEditorViewModel : BindableBase
    {
        private readonly SelectElementViewModel _elementHorizontalAlignment;
        private readonly SelectElementViewModel _elementLabelPosition;
        private readonly TextElementViewModel _elementLabelTextBox;
        private readonly TextElementViewModel _elementNameTextBox;
        private readonly SelectElementViewModel _elementTypeComboBox;
        private readonly SelectElementViewModel _elementVerticalAlignment;

        public FormElementEditorViewModel()
        {
            _elementNameTextBox =
                new TextElementViewModel(
                    new TextElementBuilder().SetName("elementNameTextBox")
                        .SetLabel(ElementLabel.Left("Control Name")).Build());

            _elementLabelTextBox =
                new TextElementViewModel(
                    new TextElementBuilder().SetName("elementLabelTextBox")
                        .SetLabel(ElementLabel.Left("Label Text")).Build());

            _elementTypeComboBox =
                new SelectElementViewModel(
                    new SelectElementBuilder().SetName("elementTypeComboBox")
                        .SetLabel(ElementLabel.Left("Control Type")).AddValues("Button", "Html", "CheckBox", "ComboBox",
                            "TextBox", "Columns", "Rows", "Group").Build());

            _elementLabelPosition =
                new SelectElementViewModel(
                    new SelectElementBuilder().SetName("elementLabelPosition")
                        .SetLabel(ElementLabel.Left("Label Position"))
                        .AddValues("Hidden", "Above", "Below", "Left", "Right").SetDefaultValue("Hidden").Build());

            _elementVerticalAlignment =
                new SelectElementViewModel(
                    new SelectElementBuilder().SetName("elementVerticalAlignment")
                        .SetLabel(ElementLabel.Left("Vertical Alignment"))
                        .AddValues("Center", "Top", "Bottom", "Stretch")
                        .SetDefaultValue("Stretch").Build());
            _elementHorizontalAlignment = new SelectElementViewModel(
                new SelectElementBuilder().SetName("elementHorizontalAlignment")
                    .SetLabel(ElementLabel.Left("Horizontal Alignment")).AddValues("Center", "Left", "Right", "Stretch")
                    .SetDefaultValue("Stretch").Build());

            _elementTypeComboBox.OnFormEvent += ElementTypeComboBoxOnOnFormEvent;
            _elementNameTextBox.OnFormEvent += ElementNameTextBoxOnOnFormEvent;
            _elementLabelTextBox.OnFormEvent += ElementLabelTextBoxOnOnFormEvent;
            _elementLabelPosition.OnFormEvent += ElementLabelPosition_OnFormEvent;
            _elementVerticalAlignment.OnFormEvent += ElementVerticalAlignment_OnFormEvent;
            _elementHorizontalAlignment.OnFormEvent += ElementHorizontalAlignment_OnFormEvent;
        }

        private void ElementHorizontalAlignment_OnFormEvent(object sender, FormElementEvent e)
        {
            if (!(e is SelectElementChangedEvent eventArgs))
            {
                return;
            }

            if (!Enum.TryParse(eventArgs.NewValue, out HorizontalAlignment alignment))
            {
                alignment = HorizontalAlignment.Center;
            }

            FormElement.HorizontalAlignment = alignment;
        }

        private void ElementVerticalAlignment_OnFormEvent(object sender, FormElementEvent e)
        {
            if (!(e is SelectElementChangedEvent eventArgs))
            {
                return;
            }

            if (!Enum.TryParse(eventArgs.NewValue, out VerticalAlignment alignment))
            {
                alignment = VerticalAlignment.Center;
            }

            FormElement.VerticalAlignment = alignment;
        }

        private void ElementLabelPosition_OnFormEvent(object sender, FormElementEvent e)
        {
            if (!(e is SelectElementChangedEvent eventArgs))
            {
                return;
            }

            if (!Enum.TryParse(eventArgs.NewValue, out LabelPosition position))
            {
                position = LabelPosition.Hidden;
            }

            FormElement.Label.Position = position;
        }

        private void ElementLabelTextBoxOnOnFormEvent(object sender, FormElementEvent e)
        {
            if (e is TextElementChangedEvent eventArgs)
            {
                FormElement.Label.Text = eventArgs.NewValue;
            }
        }

        private void ElementNameTextBoxOnOnFormEvent(object sender, FormElementEvent e)
        {
            if (e is TextElementChangedEvent eventArgs)
            {
                FormElement.Name = eventArgs.NewValue;
            }
        }

        private void ElementTypeComboBoxOnOnFormEvent(object sender, FormElementEvent e)
        {
            var oldElement = FormElement;

            if (!(e is SelectElementChangedEvent eventArgs))
            {
                return;
            }

            if (ElementTypeToFriendlyName(FormElement?.Model.GetType()) == eventArgs.NewValue)
            {
                return;
            }

            IControlElementViewModel newElementViewModel = null;

            switch (eventArgs.NewValue)
            {
                case "Button":
                    newElementViewModel =
                        new ButtonElementViewModel(new ButtonElementBuilder().FromTemplate(oldElement.Model)
                            .Build());
                    break;
                case "Html":
                    newElementViewModel =
                        new HtmlElementViewModel(new HtmlElementBuilder().FromTemplate(oldElement.Model)
                            .Build());
                    break;
                case "CheckBox":
                    newElementViewModel =
                        new CheckElementViewModel(new CheckElementBuilder().FromTemplate(oldElement.Model)
                            .Build());
                    break;
                case "TextBox":
                    newElementViewModel =
                        new TextElementViewModel(new TextElementBuilder().FromTemplate(oldElement.Model)
                            .Build());
                    break;
                case "ComboBox":
                    newElementViewModel =
                        new SelectElementViewModel(new SelectElementBuilder().FromTemplate(oldElement.Model)
                            .Build());
                    break;
            }

            var elementIndex = ParentElement.Children.IndexOf(oldElement);

            ParentElement.Children.RemoveAt(elementIndex);
            ParentElement.Children.Insert(elementIndex, newElementViewModel);
            FormElement = newElementViewModel;
        }

        #region Methods

        private string ElementTypeToFriendlyName(Type elementType)
        {
            return elementType?.Name switch
            {
                nameof(ButtonElement) => "Button",
                nameof(HtmlElement) => "Html",
                nameof(CheckElement) => "CheckBox",
                nameof(TextElement) => "TextBox",
                nameof(SelectElement) => "ComboBox",
                _ => string.Empty
            };
        }

        private void AddButtonConfigurationRows(ICompositeElementViewModel group, ButtonElementViewModel button)
        {
            var textField =
                new TextElementViewModel(
                    new TextElementBuilder()
                        .SetName("buttonContentTextBox")
                        .SetLabel(ElementLabel.Left("Content"))
                        .SetDefaultValue(button.Content).Build());

            textField.OnFormEvent += (s, e) =>
            {
                if (e is TextElementChangedEvent eventArgs)
                {
                    button.Content = eventArgs.NewValue;
                }
            };

            group.Children.Add(textField);
        }

        private void AddHtmlConfigurationRows(ICompositeElementViewModel group, HtmlElementViewModel element)
        {
            var buttonField =
                new ButtonElementViewModel(
                    new ButtonElementBuilder()
                        .SetName("editHtml")
                        .SetLabel(ElementLabel.Left("Edit"))
                        .SetContent("Edit").Build());

            buttonField.OnFormEvent += (s, e) =>
            {
                if (e is ButtonElementClickedEvent eventArgs)
                {
                    var htmlEditor = new HtmlEditorDialog(element.Content);
                    htmlEditor.ShowDialog();

                    element.Content = htmlEditor.ViewModel.Content;
                    Refresh();
                }
            };

            group.Children.Add(buttonField);
        }

        private void AddTextConfigurationRows(ICompositeElementViewModel group, TextElementViewModel element)
        {
            var textField =
                new TextElementViewModel(
                    new TextElementBuilder()
                        .SetName("htmlContent")
                        .SetLabel(ElementLabel.Left("Content"))
                        .SetDefaultValue(element.DefaultContent).Build());

            textField.OnFormEvent += (s, e) =>
            {
                if (e is TextElementChangedEvent eventArgs)
                {
                    element.DefaultContent = eventArgs.NewValue;
                    element.Content = eventArgs.NewValue;
                }
            };

            group.Children.Add(textField);
        }

        private void AddComboBoxConfigurationRows(ICompositeElementViewModel group, ISelectElementViewModel element)
        {
            var optionsField =
                new TextElementViewModel(
                    new TextElementBuilder()
                        .SetName("options")
                        .SetLabel(ElementLabel.Left("Options"))
                        .SetDefaultValue(string.Join(",", element.Items)).Build());

            var defaultSelectedText =
                new TextElementViewModel(
                    new TextElementBuilder()
                        .SetName("defaultText")
                        .SetLabel(ElementLabel.Left("Default Text"))
                        .SetDefaultValue(element.DefaultSelectedItem).Build());

            var defaultSelectedItem =
                new SelectElementViewModel(
                    new SelectElementBuilder()
                        .SetName("defaultItem")
                        .SetLabel(ElementLabel.Left("Default Item"))
                        .SetDefaultValue(element.DefaultSelectedItem)
                        .AddValues(element.Items).Build());

            optionsField.OnFormEvent += (s, e) =>
            {
                if (e is TextElementChangedEvent eventArgs)
                {
                    element.Items.Clear();
                    element.Items.AddRange(eventArgs.NewValue.Split(','));

                    defaultSelectedItem.Items.Clear();
                    defaultSelectedItem.Items.AddRange(eventArgs.NewValue.Split(',').ToList());
                    if (!defaultSelectedItem.Items.Contains(element.DefaultSelectedItem))
                    {
                        defaultSelectedItem.DefaultSelectedItem = string.Empty;
                        element.DefaultSelectedItem = string.Empty;
                    }
                }
            };

            defaultSelectedItem.OnFormEvent += (s, e) =>
            {
                if (e is SelectElementChangedEvent eventArgs && element.SelectedItem != eventArgs.NewValue)
                {
                    element.DefaultSelectedItem = eventArgs.NewValue;
                    element.SelectedItem = eventArgs.NewValue;
                    defaultSelectedText.Content = eventArgs.NewValue;
                }
            };

            defaultSelectedText.OnFormEvent += (s, e) =>
            {
                if (e is TextElementChangedEvent eventArgs && element.SelectedItem != eventArgs.NewValue)
                {
                    element.DefaultSelectedItem = eventArgs.NewValue;
                    element.SelectedItem = string.Empty;
                    defaultSelectedItem.SelectedItem = string.Empty;
                    defaultSelectedItem.DefaultSelectedItem = eventArgs.NewValue;
                }
            };

            group.Children.Add(optionsField);
            group.Children.Add(defaultSelectedItem);
            group.Children.Add(defaultSelectedText);
        }

        private void AddCheckBoxConfigurationRows(ICompositeElementViewModel group, ICheckElementViewModel element)
        {
            var textField =
                new TextElementViewModel(
                    new TextElementBuilder()
                        .SetName("contentTextField")
                        .SetLabel(ElementLabel.Left("Default Content"))
                        .SetDefaultValue(element.Content).Build());

            var checkedField =
                new CheckElementViewModel(
                    new CheckElementBuilder()
                        .SetName("checkbox")
                        .SetLabel(ElementLabel.Left("Default Value"))
                        .SetDefaultValue(element.DefaultIsChecked)
                        .SetContent(element.Content).Build());

            textField.OnFormEvent += (s, e) =>
            {
                if (e is TextElementChangedEvent eventArgs)
                {
                    element.Content = eventArgs.NewValue;
                }
            };

            checkedField.OnFormEvent += (s, e) =>
            {
                if (e is CheckElementChangedEvent eventArgs)
                {
                    element.DefaultIsChecked = eventArgs.NewValue;
                    element.IsChecked = eventArgs.NewValue;
                }
            };

            group.Children.Add(textField);
            group.Children.Add(checkedField);
        }

        private void AddElementConfigurationRows(ICompositeElementViewModel group, IFormElementViewModel formElement)
        {
            switch (formElement)
            {
                case ButtonElementViewModel buttonElement:
                    AddButtonConfigurationRows(group, buttonElement);
                    break;
                case HtmlElementViewModel htmlElement:
                    AddHtmlConfigurationRows(group, htmlElement);
                    break;
                case CheckElementViewModel checkBox:
                    AddCheckBoxConfigurationRows(group, checkBox);
                    break;
                case TextElementViewModel textField:
                    AddTextConfigurationRows(group, textField);
                    break;
                case SelectElementViewModel comboBox:
                    AddComboBoxConfigurationRows(group, comboBox);
                    break;
            }
        }

        public void Clear()
        {
            ElementEditorFormViewModel = null;
        }

        public void Refresh()
        {
            if (FormElement == null)
            {
                ParentElement = null;
                return;
            }

            ParentElement = FormViewModel.Elements.OfType<ICompositeElementViewModel>()
                .FirstOrDefault(e => e.Children.Contains(FormElement));

            var group = new LinearLayoutElementViewModel(new LinearLayoutBuilder().Build());
            group.Children.Add(_elementTypeComboBox);
            group.Children.Add(_elementNameTextBox);
            group.Children.Add(_elementLabelTextBox);
            group.Children.Add(_elementLabelPosition);
            group.Children.Add(_elementVerticalAlignment);
            group.Children.Add(_elementHorizontalAlignment);

            _elementNameTextBox.Content = FormElement.Name;
            _elementLabelTextBox.Content = FormElement.Label.Text;
            _elementLabelPosition.SelectedItem = FormElement.Label.Position.ToString();
            _elementVerticalAlignment.SelectedItem = FormElement.VerticalAlignment.ToString();
            _elementHorizontalAlignment.SelectedItem = FormElement.HorizontalAlignment.ToString();

            _elementTypeComboBox.SelectedItem = ElementTypeToFriendlyName(FormElement?.Model?.GetType());

            AddElementConfigurationRows(group, FormElement);

            var form = new FormBuilder()
                .SetName("Element Editor Form")
                .SetTitle("Element Editor Form")
                .SetRoot(group.Model).Build();

            ElementEditorFormViewModel = new FormViewModel(form) {RootElement = group};
        }

        #endregion

        #region Events

        public EventHandler<IFormElementViewModel> OnSelected { get; set; }
        public EventHandler<IFormElementViewModel> OnRemoved { get; set; }
        public EventHandler<IFormElementViewModel> OnCreated { get; set; }

        #endregion

        #region Properties

        private ICompositeElementViewModel _parentElement;

        public ICompositeElementViewModel ParentElement
        {
            get => _parentElement;
            set => SetProperty(ref _parentElement, value);
        }

        private IControlElementViewModel _formElement;

        public IControlElementViewModel FormElement
        {
            get => _formElement;
            set
            {
                SetProperty(ref _formElement, value);
                Refresh();
            }
        }

        private FormViewModel _elementEditorFormViewModel;

        public FormViewModel ElementEditorFormViewModel
        {
            get => _elementEditorFormViewModel;
            set => SetProperty(ref _elementEditorFormViewModel, value);
        }

        private FormViewModel _formViewModel;

        public FormViewModel FormViewModel
        {
            get => _formViewModel;
            set => SetProperty(ref _formViewModel, value);
        }

        #endregion
    }
}
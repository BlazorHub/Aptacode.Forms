﻿using Aptacode.Forms.Shared.Interfaces;
using Aptacode.Forms.Shared.Interfaces.Composite;
using Aptacode.Forms.Shared.Interfaces.Controls;
using Aptacode.Forms.Shared.ViewModels;
using BindableBase = Aptacode.CSharp.Common.Utilities.Mvvm.BindableBase;

namespace Aptacode.Forms.Wpf.ViewModels.Designer
{
    public class FormDesignerViewModel : BindableBase
    {
        public FormDesignerViewModel()
        {
            ElementBrowserViewModel = new FormElementBrowserViewModel();
            FormElementEditorViewModel = new FormElementEditorViewModel();
            CompositeElementEditorViewModel = new CompositeElementEditorViewModel();

            ElementBrowserViewModel.OnElementSelected += OnEditElement;
        }

        private void OnEditElement(object sender, IFormElementViewModel e)
        {
            switch (e) {
                case IControlElementViewModel controlElementViewModel:
                    FormElementEditorViewModel.FormElement = controlElementViewModel;
                    ElementEditorViewModel = FormElementEditorViewModel;
                    break;
                case ICompositeElementViewModel compositeElementViewModel:
                    CompositeElementEditorViewModel.SelectedElement = compositeElementViewModel;
                    ElementEditorViewModel = CompositeElementEditorViewModel;
                    break;
            }
        }

        #region Properties

        private FormViewModel _formViewModel;

        public FormViewModel FormViewModel
        {
            get => _formViewModel;
            set
            {
                SetProperty(ref _formViewModel, value);

                ElementBrowserViewModel.FormViewModel = FormViewModel;
                FormElementEditorViewModel.FormViewModel = FormViewModel;
                CompositeElementEditorViewModel.FormViewModel = FormViewModel;
                EventListenerEditorViewModel = new EventListenerEditorViewModel
                {
                    FormViewModel = FormViewModel
                };
            }
        }

        private FormElementBrowserViewModel _elementBrowserViewModel;

        public FormElementBrowserViewModel ElementBrowserViewModel
        {
            get => _elementBrowserViewModel;
            set => SetProperty(ref _elementBrowserViewModel, value);
        }

        private FormElementEditorViewModel _formElementEditorViewModel;

        public FormElementEditorViewModel FormElementEditorViewModel
        {
            get => _formElementEditorViewModel;
            set => SetProperty(ref _formElementEditorViewModel, value);
        }

        private CompositeElementEditorViewModel _compositeElementEditorViewModel;

        public CompositeElementEditorViewModel CompositeElementEditorViewModel
        {
            get => _compositeElementEditorViewModel;
            set => SetProperty(ref _compositeElementEditorViewModel, value);
        }

        private BindableBase _elementEditorViewModel;

        public BindableBase ElementEditorViewModel
        {
            get => _elementEditorViewModel;
            set => SetProperty(ref _elementEditorViewModel, value);
        }

        private EventListenerEditorViewModel _eventListenerEditorViewModel;

        public EventListenerEditorViewModel EventListenerEditorViewModel
        {
            get => _eventListenerEditorViewModel;
            set => SetProperty(ref _eventListenerEditorViewModel, value);
        }

        #endregion
    }
}
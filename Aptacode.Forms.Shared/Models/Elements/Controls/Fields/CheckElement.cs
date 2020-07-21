﻿using System.Collections.Generic;
using System.Linq;
using Aptacode.Forms.Shared.ValidationRules;
using Aptacode.Forms.Shared.ViewModels.Elements.Interfaces;

namespace Aptacode.Forms.Shared.Models.Elements.Controls.Fields
{
    public class CheckElement : FieldElement
    {
        public CheckElement(string name, ElementLabel label, string content, bool defaultIsChecked,
            IEnumerable<ValidationRule<ICheckElementViewModel>> rules) : base(name, label)
        {
            Content = content;
            DefaultIsChecked = defaultIsChecked;
            Rules = rules ?? Rules;
        }

        public CheckElement(string name, ElementLabel label, string content, bool defaultIsChecked,
            params ValidationRule<ICheckElementViewModel>[] rules) : this(name, label, content, defaultIsChecked,
            rules?.ToList()) { }

        #region Properties

        public bool DefaultIsChecked { get; set; }
        public string Content { get; set; }

        public IEnumerable<ValidationRule<ICheckElementViewModel>> Rules { get; set; } =
            new List<ValidationRule<ICheckElementViewModel>>();

        #endregion
    }
}
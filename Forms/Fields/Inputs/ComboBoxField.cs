﻿using System.Collections.Generic;
using System.Linq;
using Aptacode.Forms.Fields.ValidationRules;

namespace Aptacode.Forms.Fields.Inputs
{

    public class ComboBoxBaseField : BaseFieldInput
    {
        private readonly IEnumerable<ValidationRule<ComboBoxBaseField>> _rules;
        public ComboBoxBaseField()
        {

        }

        public ComboBoxBaseField(IEnumerable<ValidationRule<ComboBoxBaseField>> rules) : base(nameof(ComboBoxBaseField), rules)
        {

        }

        public override bool IsValid() => ValidationRules.All(r => r is ValidationRule<ComboBoxBaseField> rule && rule.Passed(this));
        public override IEnumerable<string> GetValidationMessages() => _rules.Select(rule => rule.GetMessage(this));

    }
}
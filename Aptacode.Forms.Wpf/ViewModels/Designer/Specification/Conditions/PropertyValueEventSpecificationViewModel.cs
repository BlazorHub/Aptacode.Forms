﻿using System.Linq;
using Aptacode.CSharp.Common.Patterns.Specification;
using Aptacode.Forms.Shared.EventListeners.Events;
using Aptacode.Forms.Shared.EventListeners.Specifications.EventSpecifications;

namespace Aptacode.Forms.Wpf.ViewModels.Designer.Specification.Conditions {
    public class PropertyValueEventSpecificationViewModel : SpecificationViewModel<FormElementEvent>
    {
        public override Specification<FormElementEvent> BuildSpecification()
        {
            var parameters = Parameters?.Split(',');
            var parameter1 = parameters?.ElementAt(0);
            var parameter2 = parameters?.ElementAt(1);
            return new PropertyValueEventSpecification(parameter1, parameter2);
        } 
        public PropertyValueEventSpecificationViewModel() : base(nameof(PropertyValueEventSpecification)) { }
        public override void LoadParameters(Specification<FormElementEvent> specification)
        {
            if (specification is PropertyValueEventSpecification spec)
            {
                Parameters = $"{spec?.PropertyName},{spec?.PropertyValue}";
            }
        }
    }
}
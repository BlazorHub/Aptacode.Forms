﻿using System;
using System.Linq.Expressions;
using Aptacode.CSharp.Common.Patterns.Specification;

namespace Aptacode.Forms.Shared.EventListeners.Specifications
{
    public class PropertyValueSpecification<T> : Specification<T>
    {
        internal PropertyValueSpecification() { }

        public PropertyValueSpecification(string propertyName, object propertyValue)
        {
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }

        public string PropertyName { get; set; }
        public object PropertyValue { get; set; }

        public override Expression<Func<T, bool>> ToExpression() => input =>
            ValuesMatch(PropertyValue, GetValue(input, PropertyName));

        protected static object GetValue(object target, string propertyName) =>
            target?.GetType().GetProperty(propertyName)?.GetValue(target);

        protected static bool ValuesMatch(object left, object right)
        {
            var result = false;
            if (left == right)
            {
                result = true;
            }
            else
            {
                result = string.Equals(left.ToString(), right.ToString(), StringComparison.OrdinalIgnoreCase);
            }

            return result;
        }
    }
}
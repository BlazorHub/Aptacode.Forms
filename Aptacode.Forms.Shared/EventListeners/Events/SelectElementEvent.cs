﻿using System;

namespace Aptacode.Forms.Shared.EventListeners.Events
{
    public abstract class SelectElementEvent : FormFieldEvent
    {
        protected SelectElementEvent(string eventType, DateTime time, string elementName) : base(eventType, time, elementName) { }
    }

    public class SelectElementChangedEvent : SelectElementEvent
    {
        public SelectElementChangedEvent(DateTime time, string elementName, string newValue) : base(nameof(SelectElementChangedEvent), time, elementName)
        {
            NewValue = newValue;
        }

        public string NewValue { get; set; }

        public override string ToString() => $"ComboBox Selection Changed: {NewValue}";
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace EifelMono.QuickButDirty.Binding
{
    public class BindingProperty
    {
        public string PropertyName { get; set; }
        public IBindingClass ParentBindingObject { get; set; } = null;
        public void OnPropertyChanged(string propertyName = null) =>
            ParentBindingObject?.OnPropertyChanged(propertyName ?? PropertyName);
        public void RefreshAll() => OnPropertyChanged(string.Empty);
    }
    public class BindingProperty<T> : BindingProperty where T : IComparable
    {
        public BindingProperty([CallerMemberName]string propertyName = "")
        {
            PropertyName = propertyName;
        }
        protected T _Value = default(T);
        public T LastValue = default(T);
        protected bool First = true;
        public T Value
        {
            get => _Value; set
            {
                try
                {
                    if (value.CompareTo(_Value) != 0 || First)
                    {
                        First = false;
                        LastValue = _Value;
                        _Value = value;
                        OnPropertyChanged();
                        OnChanged?.Invoke(LastValue, value);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    _Value = value;
                    OnPropertyChanged();
                }
            }
        }

        public delegate void OnChangedAction(T oldValue, T newValue);
        public OnChangedAction OnChanged { get; set; }

        public BindingProperty<T> SetOnChanged(OnChangedAction onChanged)
        {
            OnChanged = onChanged;
            return this;
        }
        public BindingProperty<T> SetValue(T setValue)
        {
            Value = setValue;
            return this;
        }

        public BindingProperty<T> Default(T defaultValue= default(T)) => SetValue(defaultValue);
    }
}
 
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;

namespace EifelMono.QuickButDirty.Binding {
    public class BindingCollection<T> : ObservableCollection<T> {
	public BindingCollection<T> AddRange(IEnumerable<T> items)
	{
	    foreach (var item in items)
		this.Add(item);
	    return this;
	}

	public void RefreshAll() => OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(string.Empty));
    }
}

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace System.Maui
{
	public class SpanCollection : ObservableCollection<Span>
	{
		protected override void InsertItem(int index, Span item) => base.InsertItem(index, item ?? throw new ArgumentNullException(nameof(item)));
		protected override void SetItem(int index, Span item) => base.SetItem(index, item ?? throw new ArgumentNullException(nameof(item)));

		protected override void ClearItems()
		{
			var removed = new List<Span>(this);
			base.ClearItems();
			base.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removed));
		}
	}
}
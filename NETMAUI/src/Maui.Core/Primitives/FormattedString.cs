using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace System.Maui
{
	public class FormattedString
	{
		readonly SpanCollection _spans = new SpanCollection();

		internal event NotifyCollectionChangedEventHandler SpansCollectionChanged;

		public FormattedString() => _spans.CollectionChanged += OnCollectionChanged;

		public IList<Span> Spans => _spans;

		public static explicit operator string(FormattedString formatted) => formatted.ToString();

		public static implicit operator FormattedString(string text) => new FormattedString { Spans = { new Span { Text = text } } };

		public override string ToString() => string.Concat(Spans.Select(span => span.Text));

		void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			SpansCollectionChanged?.Invoke(sender, e);
		}
	}
}

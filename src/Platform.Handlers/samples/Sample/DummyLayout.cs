using System;
using System.Collections;
using System.Collections.Generic;
using System.Maui;

namespace Xamarin.Forms
{
	public class DummyLayout : View, System.Maui.ILayout, IEnumerable<IView>
	{
		public IList<IView> Children { get; } = new List<IView>();

		public void Add(IView view)
		{
			if (view == null)
				return;

			Children.Add(view);
		}

		public IEnumerator<IView> GetEnumerator() => Children.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => Children.GetEnumerator();
	}
}
using System.Collections;
using System.Collections.Generic;

namespace System.Maui.Controls
{
	public class Layout : View, IEnumerable<IView>
	{
		readonly protected List<IView> Views = new List<IView>();
		public void Add(IView view)
		{
			if (view == null)
				return;

			Views.Add(view);
		}

		public IEnumerator<IView> GetEnumerator() => Views.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => Views.GetEnumerator();
	}
}
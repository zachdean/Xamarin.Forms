using System.Collections;
using System.Collections.Generic;

namespace Xamarin.Forms
{
	public class SwipeItems : IList<SwipeItem>
	{
		readonly List<SwipeItem> _internal;

		public SwipeItems()
		{
			_internal = new List<SwipeItem>();
		}

		public SwipeMode Mode { get; set; }

		public SwipeItem this[int index] { get => _internal[index]; set => _internal[index] = value; }

		public int Count => _internal.Count;

		public bool IsReadOnly => false;

		public void Add(SwipeItem item)
		{
			_internal.Add(item);
		}

		public void Clear()
		{
			_internal.Clear();
		}

		public bool Contains(SwipeItem item)
		{
			return _internal.Contains(item);
		}

		public void CopyTo(SwipeItem[] array, int arrayIndex)
		{
			_internal.CopyTo(array, arrayIndex);
		}

		public IEnumerator<SwipeItem> GetEnumerator()
		{
			return _internal.GetEnumerator();
		}

		public int IndexOf(SwipeItem item)
		{
			return _internal.IndexOf(item);
		}

		public void Insert(int index, SwipeItem item)
		{
			_internal.Insert(index, item);
		}

		public bool Remove(SwipeItem item)
		{
			return _internal.Remove(item);
		}

		public void RemoveAt(int index)
		{
			_internal.RemoveAt(index);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _internal.GetEnumerator();
		}
	}
}
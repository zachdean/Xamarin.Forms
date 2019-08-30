using System;

namespace Xamarin.Forms
{
	public class SwipeItem : ContextItem
	{
		private TapGestureRecognizer _tapGestureRecognizer;

		public event EventHandler<SwipeItemInvokedEventArgs> Invoked;

		public SwipeItem()
		{
			_tapGestureRecognizer = new TapGestureRecognizer();
			_tapGestureRecognizer.Tapped += OnSwipeItemTapped;
			GestureRecognizers.Add(_tapGestureRecognizer);
		}

		public new void Dispose()
		{
			GestureRecognizers.Remove(_tapGestureRecognizer);
			_tapGestureRecognizer = null;
		}

		private void OnSwipeItemTapped(object sender, EventArgs e)
		{
			Invoked?.Invoke(this, new SwipeItemInvokedEventArgs(this));
		}
	}

	public class SwipeItemInvokedEventArgs : EventArgs
	{
		public SwipeItemInvokedEventArgs(SwipeItem swipeItem)
		{
			SwipeItem = swipeItem;
		}

		public SwipeItem SwipeItem { get; set; }
	}
}
using System;
using System.ComponentModel;

namespace Xamarin.Forms
{
	public class SwipeItem : ContextItem
	{
		public static readonly BindableProperty BackgroundColorProperty = BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(SwipeItem), Color.Default);

		public Color BackgroundColor
		{
			get { return (Color)GetValue(BackgroundColorProperty); }
			set { SetValue(BackgroundColorProperty, value); }
		}

		public event EventHandler<SwipeItemInvokedEventArgs> Invoked;

		[EditorBrowsable(EditorBrowsableState.Never)]
		public void OnInvoked() => Invoked?.Invoke(this, new SwipeItemInvokedEventArgs(this));
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
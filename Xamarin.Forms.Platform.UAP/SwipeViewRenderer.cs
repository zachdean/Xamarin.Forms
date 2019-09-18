using System.ComponentModel;
using System.Collections.Generic;
using Microsoft.UI.Xaml.Controls;
using WSwipeBehaviorOnInvoked = Microsoft.UI.Xaml.Controls.SwipeBehaviorOnInvoked;
using WSwipeItems = Microsoft.UI.Xaml.Controls.SwipeItems;
using WSwipeItem = Microsoft.UI.Xaml.Controls.SwipeItem;
using WSwipeMode = Microsoft.UI.Xaml.Controls.SwipeMode;
using System.Linq;

namespace Xamarin.Forms.Platform.UWP
{
	public class SwipeViewRenderer : ViewRenderer<SwipeView, SwipeControl>
	{
		private bool _isDisposed;
		private Dictionary<WSwipeItem, SwipeItem> _leftItems;
		private Dictionary<WSwipeItem, SwipeItem> _rightItems;
		private Dictionary<WSwipeItem, SwipeItem> _topItems;
		private Dictionary<WSwipeItem, SwipeItem> _bottomItems;

		protected override void OnElementChanged(ElementChangedEventArgs<SwipeView> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement != null)
			{
				if (Control == null)
				{
					SetNativeControl(new SwipeControl());
				}

				UpdateSwipeItems();
				UpdateBackgroundColor();
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs changedProperty)
		{
			base.OnElementPropertyChanged(sender, changedProperty);

			if (changedProperty.IsOneOf(SwipeView.LeftItemsProperty, SwipeView.RightItemsProperty, SwipeView.TopItemsProperty, SwipeView.BottomItemsProperty))
				UpdateSwipeItems();
			else if (changedProperty.PropertyName == VisualElement.BackgroundColorProperty.PropertyName)
				UpdateBackgroundColor();
		}

		protected override void Dispose(bool disposing)
		{
			if (_isDisposed)
			{
				return;
			}

			if (disposing)
			{
				if (Element.LeftItems != null)
					Element.LeftItems.PropertyChanged -= OnSwipeItemsPropertyChanged;

				if (Element.RightItems != null)
					Element.RightItems.PropertyChanged -= OnSwipeItemsPropertyChanged;

				if (Element.TopItems != null)
					Element.TopItems.PropertyChanged -= OnSwipeItemsPropertyChanged;

				if (Element.BottomItems != null)
					Element.BottomItems.PropertyChanged -= OnSwipeItemsPropertyChanged;

				if (_leftItems != null)
					DisposeSwipeItems(_leftItems);

				if (_rightItems != null)
					DisposeSwipeItems(_rightItems);

				if (_topItems != null)
					DisposeSwipeItems(_topItems);

				if (_bottomItems != null)
					DisposeSwipeItems(_bottomItems);
			}

			_isDisposed = true;

			base.Dispose(disposing);
		}

		protected override void UpdateBackgroundColor()
		{
			Color backgroundColor = Element.BackgroundColor;

			if (Control != null)
			{
				Control.Background = backgroundColor.IsDefault ? null : backgroundColor.ToBrush();
			}

			base.UpdateBackgroundColor();
		}

		void OnSwipeItemsPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			var formsSwipeItems = sender as SwipeItems;

			if (e.PropertyName == SwipeItems.ModeProperty.PropertyName)
				UpdateSwipeMode(formsSwipeItems);
		}

		void OnSwipeItemPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			var formsSwipeItem = sender as SwipeItem;

			if (e.IsOneOf(
				ContextItem.TextColorProperty, 
				ContextItem.TextColorProperty, 
				ContextItem.IconProperty, 
				ContextItem.CommandProperty, 
				ContextItem.CommandParameterProperty,
				VisualElement.BackgroundColorProperty))
				UpdateSwipeItem(formsSwipeItem);
		}

		void UpdateSwipeItems()
		{
			if (Element.LeftItems != null)
				Control.LeftItems = CreateSwipeItems(SwipeDirection.Left);

			if (Element.RightItems != null)
				Control.RightItems = CreateSwipeItems(SwipeDirection.Right);

			if (Element.TopItems != null)
				Control.TopItems = CreateSwipeItems(SwipeDirection.Up);

			if (Element.BottomItems != null)
				Control.BottomItems = CreateSwipeItems(SwipeDirection.Down);
		}

		void UpdateSwipeMode(SwipeItems swipeItems)
		{
			var windowsSwipeItems = GetWindowsSwipeItems(swipeItems);

			if (windowsSwipeItems != null)
				windowsSwipeItems.Mode = GetSwipeMode(swipeItems.Mode);
		}

		void UpdateSwipeItem(SwipeItem formsSwipeItem)
		{
			if (formsSwipeItem == null)
				return;

			var windowsSwipeItem = GetWindowsSwipeItem(formsSwipeItem);

			if (windowsSwipeItem != null)
			{
				windowsSwipeItem.Text = formsSwipeItem.Text;
				windowsSwipeItem.IconSource = formsSwipeItem.Icon.ToWindowsIconSource();
				windowsSwipeItem.Background = formsSwipeItem.BackgroundColor.ToBrush();
				windowsSwipeItem.Foreground = formsSwipeItem.TextColor.ToBrush();
				windowsSwipeItem.Command = formsSwipeItem.Command;
				windowsSwipeItem.CommandParameter = formsSwipeItem.CommandParameter;
			}
		}

		void DisposeSwipeItems(Dictionary<WSwipeItem, SwipeItem> list)
		{
			if (list != null)
			{
				foreach (var item in list)
				{
					if (item.Key != null)
						item.Key.Invoked -= OnSwipeItemInvoked;
					if(item.Value != null)
						item.Value.PropertyChanged -= OnSwipeItemPropertyChanged;
				}

				list.Clear();
				list = null;
			}
		}

		WSwipeItems CreateSwipeItems(SwipeDirection swipeDirection)
		{
			var swipeItems = new WSwipeItems();

			SwipeItems items = null;

			switch (swipeDirection)
			{
				case SwipeDirection.Left:
					DisposeSwipeItems(_leftItems);
					items = Element.LeftItems;
					_leftItems = new Dictionary<WSwipeItem, SwipeItem>();
					break;
				case SwipeDirection.Right:
					DisposeSwipeItems(_rightItems);
					items = Element.RightItems;
					_rightItems = new Dictionary<WSwipeItem, SwipeItem>();
					break;
				case SwipeDirection.Up:
					DisposeSwipeItems(_topItems);
					items = Element.TopItems;
					_topItems = new Dictionary<WSwipeItem, SwipeItem>();
					break;
				case SwipeDirection.Down:
					DisposeSwipeItems(_bottomItems);
					items = Element.BottomItems;
					_bottomItems = new Dictionary<WSwipeItem, SwipeItem>();
					break;
			}

			items.PropertyChanged += OnSwipeItemsPropertyChanged;
			swipeItems.Mode = GetSwipeMode(items.Mode);

			foreach (var formsSwipeItem in items)
			{
				var windowsSwipeItem = new WSwipeItem
				{
					Background = formsSwipeItem.BackgroundColor.IsDefault ? null : formsSwipeItem.BackgroundColor.ToBrush(),
					Foreground = formsSwipeItem.TextColor.IsDefault ? null : formsSwipeItem.TextColor.ToBrush(),
					IconSource = formsSwipeItem.Icon.ToWindowsIconSource(),
					Text = formsSwipeItem.Text,
					Command = formsSwipeItem.Command,
					CommandParameter = formsSwipeItem.CommandParameter,
					BehaviorOnInvoked = GetSwipeBehaviorOnInvoked(items.SwipeBehaviorOnInvoked)
				};

				formsSwipeItem.PropertyChanged += OnSwipeItemPropertyChanged;
				windowsSwipeItem.Invoked += OnSwipeItemInvoked;

				swipeItems.Add(windowsSwipeItem);

				FillSwipeItemsCache(swipeDirection, windowsSwipeItem, formsSwipeItem);
			}

			return swipeItems;
		}

		void FillSwipeItemsCache(SwipeDirection swipeDirection, WSwipeItem windowsSwipeItem, SwipeItem formsSwipeItem)
		{
			switch (swipeDirection)
			{
				case SwipeDirection.Left:
					_leftItems.Add(windowsSwipeItem, formsSwipeItem);
					break;
				case SwipeDirection.Right:
					_rightItems.Add(windowsSwipeItem, formsSwipeItem);
					break;
				case SwipeDirection.Up:
					_topItems.Add(windowsSwipeItem, formsSwipeItem);
					break;
				case SwipeDirection.Down:
					_bottomItems.Add(windowsSwipeItem, formsSwipeItem);
					break;
			}
		}

		void OnSwipeItemInvoked(WSwipeItem sender, Microsoft.UI.Xaml.Controls.SwipeItemInvokedEventArgs args)
		{
			var windowsSwipeItem = sender;
			var formsSwipeItem = GetFormsSwipeItem(windowsSwipeItem);
			formsSwipeItem?.OnInvoked();
		}

		WSwipeItems GetWindowsSwipeItems(SwipeItems swipeItems)
		{
			if (swipeItems == Element.LeftItems)
				return Control.LeftItems;

			if (swipeItems == Element.RightItems)
				return Control.RightItems;

			if (swipeItems == Element.TopItems)
				return Control.TopItems;

			if (swipeItems == Element.BottomItems)
				return Control.BottomItems;

			return null;
		}

		WSwipeItem GetWindowsSwipeItem(SwipeItem swipeItem)
		{
			if (_leftItems != null)
				return _leftItems.FirstOrDefault(x => x.Value.Equals(swipeItem)).Key;

			if (_rightItems != null)
				return _rightItems.FirstOrDefault(x => x.Value.Equals(swipeItem)).Key;

			if (_topItems != null)
				return _topItems.FirstOrDefault(x => x.Value.Equals(swipeItem)).Key;

			if (_bottomItems != null)
				return _bottomItems.FirstOrDefault(x => x.Value.Equals(swipeItem)).Key;

			return null;
		}

		SwipeItem GetFormsSwipeItem(WSwipeItem swipeItem)
		{
			if (_leftItems != null)
			{
				_leftItems.TryGetValue(swipeItem, out SwipeItem formsSwipeItem);

				if (formsSwipeItem != null)
					return formsSwipeItem;
			}

			if (_rightItems != null)
			{
				_rightItems.TryGetValue(swipeItem, out SwipeItem formsSwipeItem);

				if (formsSwipeItem != null)
					return formsSwipeItem;
			}

			if (_topItems != null)
			{
				_topItems.TryGetValue(swipeItem, out SwipeItem formsSwipeItem);

				if (formsSwipeItem != null)
					return formsSwipeItem;
			}

			if (_bottomItems != null)
			{
				_bottomItems.TryGetValue(swipeItem, out SwipeItem formsSwipeItem);

				if (formsSwipeItem != null)
					return formsSwipeItem;
			}

			return null;
		}

		WSwipeMode GetSwipeMode(SwipeMode swipeMode)
		{
			switch (swipeMode)
			{
				case SwipeMode.Execute:
					return WSwipeMode.Execute;
				case SwipeMode.Reveal:
					return WSwipeMode.Reveal;
			}

			return WSwipeMode.Reveal;
		}

		WSwipeBehaviorOnInvoked GetSwipeBehaviorOnInvoked(SwipeBehaviorOnInvoked swipeBehaviorOnInvoked)
		{
			switch (swipeBehaviorOnInvoked)
			{
				case SwipeBehaviorOnInvoked.Auto:
					return WSwipeBehaviorOnInvoked.Auto;
				case SwipeBehaviorOnInvoked.Close:
					return WSwipeBehaviorOnInvoked.Close;
				case SwipeBehaviorOnInvoked.RemainOpen:
					return WSwipeBehaviorOnInvoked.RemainOpen;
			}

			return WSwipeBehaviorOnInvoked.Auto;
		}
	}
}
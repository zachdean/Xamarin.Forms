using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform;

namespace Xamarin.Forms
{
	[ContentProperty("Content")]
	[RenderWith(typeof(_SwipeViewRenderer))]
	public class SwipeView : ContentView, IDisposable
	{
		private const double SwipeItemWidth = 80;

		bool _isTouchDown;
		Point _initialPoint;
		SwipeDirection _swipeDirection;
		double _swipeOffset;
		Grid _content;
		View _view;
		readonly IList<Element> _gridChild;
		readonly ReadOnlyCollection<Element> _logicalChildren;

		public SwipeView()
		{
			IsClippedToBounds = true;

			_content = new Grid();
			_view = new View();

			_gridChild = new List<Element>();

			CompressedLayout.SetIsHeadless(_content, true);

			_gridChild.Add(_content);
			_logicalChildren = new ReadOnlyCollection<Element>(_gridChild);

			Content = _content;
		}
  
		public static readonly BindableProperty LeftItemsProperty = BindableProperty.Create(nameof(LeftItems), typeof(SwipeItems), typeof(SwipeView), null, BindingMode.TwoWay, null);
		public static readonly BindableProperty RightItemsProperty = BindableProperty.Create(nameof(RightItems), typeof(SwipeItems), typeof(SwipeView), null, BindingMode.TwoWay, null);
		public static readonly BindableProperty TopItemsProperty = BindableProperty.Create(nameof(TopItems), typeof(SwipeItems), typeof(SwipeView), null, BindingMode.TwoWay, null);
		public static readonly BindableProperty BottomItemsProperty = BindableProperty.Create(nameof(BottomItems), typeof(SwipeItems), typeof(SwipeView), null, BindingMode.TwoWay, null);
		public static readonly BindableProperty SwipeThresholdProperty = BindableProperty.Create(nameof(SwipeThreshold), typeof(double), typeof(SwipeView), 250.0d, BindingMode.TwoWay);

		public SwipeItems LeftItems
		{
			get { return (SwipeItems)GetValue(LeftItemsProperty); }
			set { SetValue(LeftItemsProperty, value); }
		}

		public SwipeItems RightItems
		{
			get { return (SwipeItems)GetValue(RightItemsProperty); }
			set { SetValue(RightItemsProperty, value); }
		}

		public SwipeItems TopItems
		{
			get { return (SwipeItems)GetValue(TopItemsProperty); }
			set { SetValue(TopItemsProperty, value); }
		}

		public SwipeItems BottomItems
		{
			get { return (SwipeItems)GetValue(BottomItemsProperty); }
			set { SetValue(BottomItemsProperty, value); }
		}

		public double SwipeThreshold
		{
			get { return (double)GetValue(SwipeThresholdProperty); }
			set { SetValue(SwipeThresholdProperty, value); }
		}

		internal bool IsSwiping { get; private set; }

		public event EventHandler<SwipeStartedEventArgs> SwipeStarted;
		public event EventHandler<SwipeEndedEventArgs> SwipeEnded;

		internal override ReadOnlyCollection<Element> LogicalChildrenInternal => _logicalChildren;

		protected override bool ShouldInvalidateOnChildAdded(View child)
		{
			return false;
		}

		protected override bool ShouldInvalidateOnChildRemoved(View child)
		{
			return false;
		}

		protected override void OnParentSet()
		{
			base.OnParentSet();
			InternalChildren.CollectionChanged += ItemsCollectionChanged;	
  		}
  
		protected override void OnChildAdded(Element child)
		{
			if (child is View view)
			{
				if (view != _content)
				{
					if(view is SwipeViewLayout)
						_content.Children.Add(view);
					else
					{
						_view = view;
						_content.Children.Add(_view);
					}
				}
			}
		}

		protected override void OnChildRemoved(Element child)
		{
			if (child is View view)
				_content.Children.Remove(view);
		}

		protected override void OnSizeAllocated(double width, double height)
		{
			if (_view != null)
			{
				if (_view.HeightRequest > 0 && _view.WidthRequest > 0)
				{
					HeightRequest = _view.HeightRequest;
					WidthRequest = _view.WidthRequest;
				}
				else
				{
					_view.HeightRequest = height;
					_view.WidthRequest = width;
				}
			}

			base.OnSizeAllocated(width, height);
		}

		public void Dispose()
		{
			if (InternalChildren != null)
				InternalChildren.CollectionChanged += ItemsCollectionChanged;

			_content = null;
			_view = null;
		}
  
		[Preserve(Conditional = true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool HandleTouchInteractions(GestureStatus status, Point point)
		{
			switch (status)
			{
				case GestureStatus.Started:
					return !ProcessTouchDown(point);
				case GestureStatus.Running:
					return !ProcessTouchMove(point);
				case GestureStatus.Completed:
					return !ProcessTouchUp();
			}

			_isTouchDown = false;

			return true;
		}

		bool ProcessTouchDown(Point point)
		{
			if (IsSwiping || _isTouchDown || _view == null)
				return false;

			bool touchContent = TouchInsideContent(_view.X + _view.TranslationX, _view.Y + _view.TranslationY, _view.Width, _view.Height, point.X, point.Y);

			if (touchContent)
				ResetSwipe(_swipeDirection);
			else
				ProcessTouchSwipeItems(point);

			_initialPoint = point;
			_isTouchDown = true;

			return true;
		}

		bool ProcessTouchMove(Point point)
		{
			if (!IsSwiping)
			{
				_swipeDirection = GetSwipeDirection(_initialPoint, point);
				RaiseSwipeStarted();
				IsSwiping = true;
			}

			if (!ValidateSwipeDirection(_swipeDirection))
				return false;

			_swipeOffset = GetSwipeOffset(_initialPoint, point, _swipeDirection);
			InitializeSwipe(_swipeDirection);

			if (Math.Abs(_swipeOffset) > double.Epsilon)
				Swipe(_swipeDirection, _swipeOffset);
			else
				ResetSwipe(_swipeDirection);

			return true;
		}

		bool ProcessTouchUp()
		{
			_isTouchDown = false;

			if (!IsSwiping)
				return false;

			IsSwiping = false;

			RaiseSwipeEnded();

			if (!ValidateSwipeDirection(_swipeDirection))
				return false;

			ValidateSwipeThreshold(_swipeDirection);

			return false;
		}

		void ProcessTouchSwipeItems(Point point)
		{
			switch (_swipeDirection)
			{
				case SwipeDirection.Down:
					ProcessTouchSwipeItems(_swipeDirection, TopItems, point);
					break;
				case SwipeDirection.Left:
					ProcessTouchSwipeItems(_swipeDirection, RightItems, point);
					break;
				case SwipeDirection.Right:
					ProcessTouchSwipeItems(_swipeDirection, LeftItems, point);
					break;
				case SwipeDirection.Up:
					ProcessTouchSwipeItems(_swipeDirection, BottomItems, point);
					break;
			}
		}

		void ItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.NewItems != null)
				foreach (Element el in e.NewItems)
					OnChildAdded(el);

			if (e.OldItems != null)
				foreach (Element el in e.OldItems)
					OnChildRemoved(el);
		}

		void ProcessTouchSwipeItems(SwipeDirection swipeDirection, SwipeItems swipeItems, Point point)
		{
			if (swipeItems == null)
				return;

			foreach (var swipeItem in swipeItems)
			{
				var swipeItemX = swipeItem.X;
				var swipeItemY = swipeItem.Y;

				if (swipeDirection == SwipeDirection.Left)
				{
					double totalSwipeItemsWidth = swipeItems.Sum(s => s.Width);
					swipeItemX += totalSwipeItemsWidth;
				}

				if (TouchInsideContent(swipeItemX, swipeItemY, swipeItem.Width, swipeItem.Height, point.X, point.Y))
				{
					ICommand cmd = swipeItem.Command;
					object parameter = swipeItem.CommandParameter;

					if (cmd != null && cmd.CanExecute(parameter))
						cmd.Execute(parameter);

					swipeItem.OnInvoked();

					if (swipeItems.SwipeBehaviorOnInvoked != SwipeBehaviorOnInvoked.RemainOpen)
						ResetSwipe(_swipeDirection);

					break;
				}
			}
		}

		bool TouchInsideContent(double x1, double y1, double x2, double y2, double x, double y)
		{
			if (x > x1 && x < (x1 + x2) && y > y1 && y < (y1 + y2))
				return true;

			return false;
		}

		SwipeDirection GetSwipeDirection(Point initialPoint, Point endPoint)
		{
			var angle = GetAngleFromPoints(initialPoint.X, initialPoint.Y, endPoint.X, endPoint.Y);
			return GetSwipeDirectionFromAngle(angle);
		}

		double GetSwipeOffset(Point initialPoint, Point endPoint, SwipeDirection swipeDirection)
		{
			double swipeOffset = 0;

			switch (swipeDirection)
			{
				case SwipeDirection.Left:
				case SwipeDirection.Right:
					swipeOffset = endPoint.X - initialPoint.X;
					break;
				case SwipeDirection.Up:
				case SwipeDirection.Down:
					swipeOffset = endPoint.Y - initialPoint.Y;
					break;
			}

			return swipeOffset;
		}

		double GetSwipeThreshold(SwipeDirection swipeDirection)
		{
			double swipeThreshold = 0;

			switch (swipeDirection)
			{
				case SwipeDirection.Left:
					swipeThreshold = GetSwipeThreshold(swipeDirection, RightItems);
					break;
				case SwipeDirection.Right:
					swipeThreshold = GetSwipeThreshold(swipeDirection, LeftItems);
					break;
				case SwipeDirection.Up:
					swipeThreshold = GetSwipeThreshold(swipeDirection, BottomItems);
					break;
				case SwipeDirection.Down:
					swipeThreshold = GetSwipeThreshold(swipeDirection, TopItems);
					break;
			}

			return swipeThreshold;
		}

		double GetSwipeThreshold(SwipeDirection swipeDirection, SwipeItems swipeItems)
		{
			double swipeThreshold = 0;

			if (swipeItems == null)
				return 0;

			bool isHorizontal = swipeDirection == SwipeDirection.Left || swipeDirection == SwipeDirection.Right;

			if (swipeItems.Mode == SwipeMode.Reveal)
			{
				if (isHorizontal)
				{
					foreach (var swipeItem in swipeItems)
						swipeThreshold += swipeItem.WidthRequest;
				}
				else
					swipeThreshold = (SwipeThreshold > _view.HeightRequest) ? _view.HeightRequest : SwipeThreshold;
			}
			else
			{
				if (isHorizontal)
					swipeThreshold = SwipeThreshold;
				else
					swipeThreshold = (SwipeThreshold > _view.HeightRequest) ? _view.HeightRequest : SwipeThreshold;
			}

			return swipeThreshold;
		}

		bool ValidateSwipeDirection(SwipeDirection swipeDirection)
		{
			switch (swipeDirection)
			{
				case SwipeDirection.Left:
					return RightItems != null;
				case SwipeDirection.Right:
					return LeftItems != null;
				case SwipeDirection.Up:
					return BottomItems != null;
				case SwipeDirection.Down:
					return TopItems != null;
			}

			return false;
		}

		double ValidateSwipeOffset(double offset)
		{
			var swipeThreshold = GetSwipeThreshold(_swipeDirection);

			switch (_swipeDirection)
			{
				case SwipeDirection.Left:
					if (offset > 0)
						offset = 0;

					if (Math.Abs(offset) > swipeThreshold)
						return -swipeThreshold;
					break;
				case SwipeDirection.Right:
					if (offset < 0)
						offset = 0;

					if (Math.Abs(offset) > swipeThreshold)
						return swipeThreshold;
					break;
				case SwipeDirection.Up:
					if (offset > 0)
						offset = 0;

					if (Math.Abs(offset) > swipeThreshold)
						return -swipeThreshold;
					break;
				case SwipeDirection.Down:
					if (offset < 0)
						offset = 0;

					if (Math.Abs(offset) > swipeThreshold)
						return swipeThreshold;
					break;
			}

			return offset;
		}

		double GetAngleFromPoints(double x1, double y1, double x2, double y2)
		{
			double rad = Math.Atan2(y1 - y2, x2 - x1) + Math.PI;
			return (rad * 180 / Math.PI + 180) % 360;
		}

		SwipeDirection GetSwipeDirectionFromAngle(double angle)
		{
			if (IsAngleInRange(angle, 45, 135))
				return SwipeDirection.Up;

			if (IsAngleInRange(angle, 0, 45) || IsAngleInRange(angle, 315, 360))
				return SwipeDirection.Right;

			if (IsAngleInRange(angle, 225, 315))
				return SwipeDirection.Down;

			return SwipeDirection.Left;
		}

		bool IsAngleInRange(double angle, float init, float end)
		{
			return (angle >= init) && (angle < end);
		}

		void InitializeSwipe(SwipeDirection swipeDirection)
		{
			switch (swipeDirection)
			{
				case SwipeDirection.Left:
					InitializeSwipeItems(SwipeDirection.Right);
					break;
				case SwipeDirection.Right:
					InitializeSwipeItems(SwipeDirection.Left);
					break;
				case SwipeDirection.Up:
					InitializeSwipeItems(SwipeDirection.Down);
					break;
				case SwipeDirection.Down:
					InitializeSwipeItems(SwipeDirection.Up);
					break;
			}
		}

		void Swipe(SwipeDirection swipeDirection, double swipeOffset)
		{
			switch (swipeDirection)
			{
				case SwipeDirection.Left:
					_view.TranslationX = ValidateSwipeOffset(swipeOffset);
					break;
				case SwipeDirection.Right:
					_view.TranslationX = ValidateSwipeOffset(swipeOffset);
					break;
				case SwipeDirection.Up:
					_view.TranslationY = ValidateSwipeOffset(swipeOffset);
					break;
				case SwipeDirection.Down:
					_view.TranslationY = ValidateSwipeOffset(swipeOffset);
					break;
			}
		}

		void ValidateSwipeThreshold(SwipeDirection swipeDirection)
		{
			var swipeThresholdPercent = 0.6 * GetSwipeThreshold(_swipeDirection);

			if (Math.Abs(_swipeOffset) >= swipeThresholdPercent)
			{
				switch (swipeDirection)
				{
					case SwipeDirection.Left:
						ValidateSwipeThreshold(SwipeDirection.Left, RightItems);
						break;
					case SwipeDirection.Right:
						ValidateSwipeThreshold(SwipeDirection.Right, LeftItems);
						break;
					case SwipeDirection.Up:
						ValidateSwipeThreshold(SwipeDirection.Up, BottomItems);
						break;
					case SwipeDirection.Down:
						ValidateSwipeThreshold(SwipeDirection.Down, TopItems);
						break;
				}
			}
			else
			{
				ResetSwipe(swipeDirection);
			}
		}

		void ValidateSwipeThreshold(SwipeDirection swipeDirection, SwipeItems swipeItems)
		{
			if (swipeItems == null)
				return;

			if (swipeItems.Mode == SwipeMode.Execute)
			{
				foreach (var swipeItem in swipeItems)
				{
					ICommand cmd = swipeItem.Command;
					object parameter = swipeItem.CommandParameter;

					if (cmd != null && cmd.CanExecute(parameter))
						cmd.Execute(parameter);

					swipeItem.OnInvoked();
				}

				if (swipeItems.SwipeBehaviorOnInvoked != SwipeBehaviorOnInvoked.RemainOpen)
					ResetSwipe(swipeDirection);
			}
			else
				CompleteSwipe(swipeDirection);
		}

		private void ResetSwipe(SwipeDirection swipeDirection)
		{
			switch (swipeDirection)
			{
				case SwipeDirection.Left:
				case SwipeDirection.Right:
					_view.TranslationX = 0;
					break;
				case SwipeDirection.Up:
				case SwipeDirection.Down:
					_view.TranslationY = 0;
					break;
			}
			DisposeSwipeItems();
			_view.InputTransparent = false;
			IsSwiping = false;
		}

		private void CompleteSwipe(SwipeDirection swipeDirection)
		{
			double swipeThreshold;
			switch (swipeDirection)
			{
				case SwipeDirection.Left:
					swipeThreshold = GetSwipeThreshold(swipeDirection, RightItems);
					_view.TranslationX = -swipeThreshold;
					break;
				case SwipeDirection.Right:
					swipeThreshold = GetSwipeThreshold(swipeDirection, LeftItems);
					_view.TranslationX = swipeThreshold;
					break;
				case SwipeDirection.Up:
					swipeThreshold = GetSwipeThreshold(swipeDirection, BottomItems);
					_view.TranslationY = -swipeThreshold;
					break;
				case SwipeDirection.Down:
					swipeThreshold = GetSwipeThreshold(swipeDirection, TopItems);
					_view.TranslationY = swipeThreshold;
					break;
			}

			_view.InputTransparent = true;
			IsSwiping = false;
		}
  
		void InitializeSwipeItems(SwipeDirection swipeDirection)
		{
			switch (swipeDirection)
			{
				case SwipeDirection.Left:
					InitializeSwipeItems(swipeDirection, LeftItems);
					break;
				case SwipeDirection.Right:
					InitializeSwipeItems(swipeDirection, RightItems);
					break;
				case SwipeDirection.Up:
					InitializeSwipeItems(swipeDirection, TopItems);
					break;
				case SwipeDirection.Down:
					InitializeSwipeItems(swipeDirection, BottomItems);
					break;
			}
		}

		void InitializeSwipeItems(SwipeDirection swipeDirection, SwipeItems swipeItems)
		{
			if (_content == null || _content.Children.Count > 1)
				return;

			var swipeItemsLayout = new SwipeViewLayout
			{
				Spacing = 0,
				Orientation = StackOrientation.Horizontal
			};

			switch (swipeDirection)
			{
				case SwipeDirection.Left:
					swipeItemsLayout.HorizontalOptions = LayoutOptions.StartAndExpand;
					break;
				case SwipeDirection.Right:
					swipeItemsLayout.HorizontalOptions = LayoutOptions.EndAndExpand;
					break;
				case SwipeDirection.Up:
					swipeItemsLayout.HorizontalOptions = LayoutOptions.EndAndExpand;
					break;
				case SwipeDirection.Down:
					swipeItemsLayout.HorizontalOptions = LayoutOptions.StartAndExpand;
					break;
			}

			if (swipeItems != null)
			{
				double swipeItemWidth = (swipeItems.Mode == SwipeMode.Reveal) ? SwipeItemWidth : SwipeThreshold / swipeItems.Count;

				if (swipeDirection == SwipeDirection.Up || swipeDirection == SwipeDirection.Down)
					swipeItemWidth = _view.WidthRequest / swipeItems.Count;

				foreach (SwipeItem item in swipeItems)
				{
					item.HeightRequest = _view.HeightRequest;

					if (item.WidthRequest <= 0)
						item.WidthRequest = swipeItemWidth;

					swipeItemsLayout.Children.Add(item);
				}
			}

			_content.Children.Add(swipeItemsLayout);
			_content.RaiseChild(_view);
		}

		void DisposeSwipeItems()
		{
			var disposeChildren = _content.Children.Where(v => v != _view).ToList();

			foreach (var child in disposeChildren)
				RemoveChild(child);

			disposeChildren = null;
		}

		void RemoveChild(View view)
		{
			if (_content == null)
				return;

			_content.Children.Remove(view);
		}

		void RaiseSwipeStarted()
		{
			var swipeStartedEventArgs = new SwipeStartedEventArgs(_swipeDirection, _swipeOffset);

			SwipeStarted?.Invoke(this, swipeStartedEventArgs);
		}

		void RaiseSwipeEnded()
		{
			var swipeEndedEventArgs = new SwipeEndedEventArgs(_swipeDirection);

			SwipeEnded?.Invoke(this, swipeEndedEventArgs);
		}

		public class SwipeStartedEventArgs : EventArgs
		{
			public SwipeStartedEventArgs(SwipeDirection swipeDirection, double offset)
			{
				SwipeDirection = swipeDirection;
				Offset = offset;
			}

			public SwipeDirection SwipeDirection { get; set; }
			public double Offset { get; set; }
		}

		public class SwipeEndedEventArgs : EventArgs
		{
			public SwipeEndedEventArgs(SwipeDirection swipeDirection)
			{
				SwipeDirection = swipeDirection;
			}

			public SwipeDirection SwipeDirection { get; set; }
		}
	}

	internal class SwipeViewLayout : StackLayout
	{

	}
}
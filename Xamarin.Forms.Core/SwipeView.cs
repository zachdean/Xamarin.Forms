using System;
using System.Diagnostics;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform;

namespace Xamarin.Forms
{
	[ContentProperty("View")]
	[RenderWith(typeof(_SwipeViewRenderer))]
	public class SwipeView : ContentView, IDisposable
	{
		private const double SwipeItemWidth = 80;
		private const double SwipeThreshold = 250;

		private bool _isTouchDown;
		private Point _initialPoint;
		private SwipeDirection _swipeDirection;
		private double _swipeOffset;
		private Grid _content;
		private Grid _view;
		private StackLayout _leftItems;
		private StackLayout _rightItems;
		private StackLayout _topItems;
		private StackLayout _bottomItems;

		public SwipeView()
		{
			IsClippedToBounds = true;

			_content = new Grid();
			_leftItems = new StackLayout();
			_rightItems = new StackLayout();
			_topItems = new StackLayout();
			_bottomItems = new StackLayout();
			_view = new Grid();

			_content.Children.Add(_leftItems);
			_content.Children.Add(_rightItems);
			_content.Children.Add(_topItems);
			_content.Children.Add(_bottomItems);
			_content.Children.Add(_view);

			Content = _content;
		}

		public static readonly BindableProperty ViewProperty = BindableProperty.Create(nameof(View), typeof(View), typeof(SwipeView), default(View), BindingMode.TwoWay, null, OnViewChanged);
		public static readonly BindableProperty LeftItemsProperty = BindableProperty.Create(nameof(LeftItems), typeof(SwipeItems), typeof(SwipeView), null, BindingMode.TwoWay, null, OnSwipeItemsChanged);
		public static readonly BindableProperty RightItemsProperty = BindableProperty.Create(nameof(RightItems), typeof(SwipeItems), typeof(SwipeView), null, BindingMode.TwoWay, null, OnSwipeItemsChanged);
		public static readonly BindableProperty TopItemsProperty = BindableProperty.Create(nameof(TopItems), typeof(SwipeItems), typeof(SwipeView), null, BindingMode.TwoWay, null, OnSwipeItemsChanged);
		public static readonly BindableProperty BottomItemsProperty = BindableProperty.Create(nameof(BottomItems), typeof(SwipeItems), typeof(SwipeView), null, BindingMode.TwoWay, null, OnSwipeItemsChanged);

		public View View
		{
			get { return (View)GetValue(ViewProperty); }
			set { SetValue(ViewProperty, value); }
		}

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

		internal bool IsSwiping { get; private set; }

		public event EventHandler<SwipeStartedEventArgs> SwipeStarted;
		public event EventHandler<SwipeEndedEventArgs> SwipeEnded;

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

			if (Parent == null)
				Dispose();
		}

		public void Dispose()
		{
			_content = null;
			_view = null;
			_leftItems = null;
			_rightItems = null;
			_topItems = null;
			_bottomItems = null;
		}

		[Preserve(Conditional = true)]
		public bool HandleTouchInteractions(GestureStatus status, Point point)
		{
			switch (status)
			{
				case GestureStatus.Started:
					Debug.WriteLine("Started");
					return !ProcessTouchDown(point);
				case GestureStatus.Running:
					Debug.WriteLine("Running");
					return !ProcessTouchMove(point);
				case GestureStatus.Completed:
					Debug.WriteLine("Completed");
					return !ProcessTouchUp();
			}

			_isTouchDown = false;

			return true;
		}

		private bool ProcessTouchDown(Point point)
		{
			if (IsSwiping || _isTouchDown)
				return false;

			ResetSwipe(_swipeDirection);

			_initialPoint = point;
			_isTouchDown = true;

			return true;
		}

		private bool ProcessTouchMove(Point point)
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

			Debug.WriteLine(_swipeOffset);

			if (Math.Abs(_swipeOffset) > double.Epsilon)
				Swipe(_swipeDirection, _swipeOffset);
			else
				ResetSwipe(_swipeDirection);

			return true;
		}

		private bool ProcessTouchUp()
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

		private SwipeDirection GetSwipeDirection(Point initialPoint, Point endPoint)
		{
			var angle = GetAngleFromPoints(initialPoint.X, initialPoint.Y, endPoint.X, endPoint.Y);
			return GetSwipeDirectionFromAngle(angle);
		}

		private double GetSwipeOffset(Point initialPoint, Point endPoint, SwipeDirection swipeDirection)
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

		private double GetSwipeThreshold(SwipeDirection swipeDirection)
		{
			double swipeThreshold = 0;

			switch (swipeDirection)
			{
				case SwipeDirection.Left:
					swipeThreshold = GetSwipeThreshold(RightItems);
					break;
				case SwipeDirection.Right:
					swipeThreshold = GetSwipeThreshold(LeftItems);
					break;
				case SwipeDirection.Up:
					swipeThreshold = GetSwipeThreshold(BottomItems);
					break;
				case SwipeDirection.Down:
					swipeThreshold = GetSwipeThreshold(TopItems);
					break;
			}

			return swipeThreshold;
		}

		private double GetSwipeThreshold(SwipeItems swipeItems)
		{
			double swipeThreshold = 0;

			if (swipeItems == null)
				return 0;

			if (swipeItems.Mode == SwipeMode.Reveal)
				foreach (var swipeItem in swipeItems)
					swipeThreshold += swipeItem.WidthRequest;
			else
				swipeThreshold = SwipeThreshold;

			return swipeThreshold;
		}

		private bool ValidateSwipeDirection(SwipeDirection swipeDirection)
		{
			switch (swipeDirection)
			{
				case SwipeDirection.Left:
					return _rightItems.Children.Count > 0;
				case SwipeDirection.Right:
					return _leftItems.Children.Count > 0;
				case SwipeDirection.Up:
					return _bottomItems.Children.Count > 0;
				case SwipeDirection.Down:
					return _topItems.Children.Count > 0;
			}

			return false;
		}

		private double ValidateSwipeOffset(double offset)
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
				case SwipeDirection.Down:
					break;
			}

			return offset;
		}

		public double GetAngleFromPoints(double x1, double y1, double x2, double y2)
		{
			double rad = Math.Atan2(y1 - y2, x2 - x1) + Math.PI;
			return (rad * 180 / Math.PI + 180) % 360;
		}

		public SwipeDirection GetSwipeDirectionFromAngle(double angle)
		{
			if (IsAngleInRange(angle, 45, 135))
				return SwipeDirection.Up;

			if (IsAngleInRange(angle, 0, 45) || IsAngleInRange(angle, 315, 360))
				return SwipeDirection.Right;

			if (IsAngleInRange(angle, 225, 315))
				return SwipeDirection.Down;

			return SwipeDirection.Left;
		}

		private bool IsAngleInRange(double angle, float init, float end)
		{
			return (angle >= init) && (angle < end);
		}

		private void InitializeSwipe(SwipeDirection swipeDirection)
		{
			switch (swipeDirection)
			{
				case SwipeDirection.Left:
					if (_rightItems != null)
						_rightItems.IsVisible = true;
					break;
				case SwipeDirection.Right:
					if (_leftItems != null)
						_leftItems.IsVisible = true;
					break;
				case SwipeDirection.Up:
					if (_bottomItems != null)
						_bottomItems.IsVisible = true;
					break;
				case SwipeDirection.Down:
					if (_topItems != null)
						_topItems.IsVisible = true;
					break;
			}
		}

		private void Swipe(SwipeDirection swipeDirection, double swipeOffset)
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

		private void ValidateSwipeThreshold(SwipeDirection swipeDirection)
		{
			var swipeThresholdPercent = 0.6 * GetSwipeThreshold(_swipeDirection);

			if (_swipeOffset >= swipeThresholdPercent)
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

		private void ValidateSwipeThreshold(SwipeDirection swipeDirection, SwipeItems swipeItems)
		{
			if (swipeItems == null)
				return;

			if (swipeItems.Mode == SwipeMode.Execute)
			{
				foreach (var swipeItem in swipeItems)
					swipeItem.Command?.Execute(swipeItem.CommandParameter);

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
					_rightItems.IsVisible = false;
					break;
				case SwipeDirection.Up:
				case SwipeDirection.Down:
					_view.TranslationY = 0;
					_bottomItems.IsVisible = false;
					break;
			}
			IsSwiping = false;
		}

		private void CompleteSwipe(SwipeDirection swipeDirection)
		{
			double swipeThreshold = 0;

			switch (swipeDirection)
			{
				case SwipeDirection.Left:
					swipeThreshold = GetSwipeThreshold(RightItems);
					break;
				case SwipeDirection.Right:
					swipeThreshold = GetSwipeThreshold(LeftItems);
					break;
				case SwipeDirection.Up:
					swipeThreshold = GetSwipeThreshold(BottomItems);
					break;
				case SwipeDirection.Down:
					swipeThreshold = GetSwipeThreshold(TopItems);
					break;
			}

			_view.TranslationX = swipeThreshold;
			IsSwiping = false;
		}

		private void InitializeContent()
		{
			if (_view == null)
				return;

			_view.Children.Clear();
			_view.Children.Add(View);
		}

		private void InitializeSwipeItems(SwipeDirection swipeDirection)
		{
			switch (swipeDirection)
			{
				case SwipeDirection.Left:
					if (_leftItems == null)
						return;
					_leftItems.HorizontalOptions = LayoutOptions.StartAndExpand;
					InitializeSwipeItems(_leftItems, LeftItems);
					break;
				case SwipeDirection.Right:
					if (_rightItems == null)
						return;
					_rightItems.HorizontalOptions = LayoutOptions.EndAndExpand;
					InitializeSwipeItems(_rightItems, RightItems);
					break;
				case SwipeDirection.Up:
					if (_topItems == null)
						return;
					_topItems.VerticalOptions = LayoutOptions.EndAndExpand;
					InitializeSwipeItems(_topItems, TopItems);
					break;
				case SwipeDirection.Down:
					if (_bottomItems == null)
						return;
					_bottomItems.VerticalOptions = LayoutOptions.StartAndExpand;
					InitializeSwipeItems(_bottomItems, BottomItems);
					break;
			}
		}

		private void InitializeSwipeItems(StackLayout stackLayout, SwipeItems swipeItems)
		{
			stackLayout.Spacing = 0;
			stackLayout.Orientation = StackOrientation.Horizontal;
			stackLayout.IsVisible = false;

			stackLayout.Children.Clear();

			if (swipeItems != null)
			{
				var swipeItemWidth = (swipeItems.Mode == SwipeMode.Reveal) ? SwipeItemWidth : SwipeThreshold / swipeItems.Count;

				if (_swipeDirection == SwipeDirection.Down || _swipeDirection == SwipeDirection.Up)
					swipeItemWidth = _content.WidthRequest / swipeItems.Count;

				foreach (SwipeItem swipeItem in swipeItems)
				{
					swipeItem.WidthRequest = swipeItemWidth;

					stackLayout.Children.Add(swipeItem);
				}
			}
		}

		private void RaiseSwipeStarted()
		{
			var swipeStartedEventArgs = new SwipeStartedEventArgs(_swipeDirection, _swipeOffset);

			SwipeStarted?.Invoke(this, swipeStartedEventArgs);
		}

		private void RaiseSwipeEnded()
		{
			var swipeEndedEventArgs = new SwipeEndedEventArgs(_swipeDirection);

			SwipeEnded?.Invoke(this, swipeEndedEventArgs);
		}

		private static void OnViewChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var swipeView = (SwipeView)bindable;

			if (Equals(newValue, null) && !Equals(oldValue, null))
				return;

			if (swipeView.View != null)
				swipeView.InitializeContent();
		}

		private static void OnSwipeItemsChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var swipeView = (SwipeView)bindable;

			if (Equals(newValue, null) && !Equals(oldValue, null))
				return;
				
			if (swipeView.LeftItems != null)
				swipeView.InitializeSwipeItems(SwipeDirection.Left);

			if (swipeView.RightItems != null)
				swipeView.InitializeSwipeItems(SwipeDirection.Right);

			if (swipeView.TopItems != null)
				swipeView.InitializeSwipeItems(SwipeDirection.Up);

			if (swipeView.BottomItems != null)
				swipeView.InitializeSwipeItems(SwipeDirection.Down);
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
}
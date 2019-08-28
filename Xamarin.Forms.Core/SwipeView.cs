using System;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform;

namespace Xamarin.Forms
{
	[RenderWith(typeof(_SwipeViewRenderer))]
	public class SwipeView : ContentView, IDisposable
	{
		private bool _isTouchDown;
		private bool _isInSwiping;
		private Point _initialPoint;
		private SwipeDirection _swipeDirection;
		private double _swipeOffset;
		private View _content;
		private View _leftItems;
		private View _rightItems;
		private View _topItems;
		private View _bottomItems;

		public SwipeView()
		{
			IsClippedToBounds = true;
		}

		public static readonly BindableProperty LeftItemsProperty = BindableProperty.Create(nameof(LeftItems), typeof(SwipeItems), typeof(SwipeView), default(SwipeItems));
		public static readonly BindableProperty RightItemsProperty = BindableProperty.Create(nameof(RightItems), typeof(SwipeItems), typeof(SwipeView), default(SwipeItems));
		public static readonly BindableProperty TopItemsProperty = BindableProperty.Create(nameof(TopItems), typeof(SwipeItems), typeof(SwipeView), default(SwipeItems));
		public static readonly BindableProperty BottomItemsProperty = BindableProperty.Create(nameof(BottomItems), typeof(SwipeItems), typeof(SwipeView), default(SwipeItems));

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

		public static event EventHandler<SwipeStartedEventArgs> SwipeStarted;
		public static event EventHandler<SwipeEndedEventArgs> SwipeEnded;

		protected override bool ShouldInvalidateOnChildAdded(View child)
		{
			return false;
		}

		protected override bool ShouldInvalidateOnChildRemoved(View child)
		{
			return false;
		}

		public void Dispose()
		{
			_content = null;
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
					System.Diagnostics.Debug.WriteLine("Started");
					return !ProcessTouchDown(point);
				case GestureStatus.Running:
					System.Diagnostics.Debug.WriteLine("Running");
					return !ProcessTouchMove(point);
				case GestureStatus.Completed:
					System.Diagnostics.Debug.WriteLine("Completed");
					return !ProcessTouchUp();
			}

			_isTouchDown = false;

			return true;
		}

		private bool ProcessTouchDown(Point point)
		{
			if (_isInSwiping || _isTouchDown)
				return false;

			_initialPoint = point;
			_isTouchDown = true;

			return true;
		}

		private bool ProcessTouchMove(Point point)
		{	
			if (!_isInSwiping)
			{
				_swipeDirection = GetSwipeDirection(_initialPoint, point);
				RaiseSwipeStarted();
				_isInSwiping = true;
			}

			_swipeOffset = GetSwipeOffset(_initialPoint, point, _swipeDirection);
			InitializeSwipeView(_swipeDirection);

			if (Math.Abs(_swipeOffset) > double.Epsilon)
				LayoutSwipeView(_swipeDirection, _swipeOffset);
			else
				ResetSwipe();

			return true;
		}

		private bool ProcessTouchUp()
		{
			_isTouchDown = false;

			if (!_isInSwiping)
				return false;

			_isInSwiping = false;

			ResetSwipe();
			RaiseSwipeEnded();

			return false;
		}

		private SwipeDirection GetSwipeDirection(Point initialPoint, Point endPoint)
		{
			var angle = GetAngleFromPoints(initialPoint.X, initialPoint.Y, endPoint.X, endPoint.Y);
			return  GetSwipeDirectionFromAngle(angle);
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

		private double ValidateOffset(double offset)
		{
			switch (_swipeDirection)
			{
				case SwipeDirection.Left:
				case SwipeDirection.Right:
					if (Math.Abs(offset) > _content.Width)
					{
						if (offset > 0)
							return _content.Width;

						return -(_content.Width);
					}
					break;
				case SwipeDirection.Up:
				case SwipeDirection.Down:
					if (Math.Abs(offset) > _content.Height)
					{
						if (offset > 0)
							return _content.Height;

						return -(_content.Height);
					}
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

		private void InitializeSwipeView(SwipeDirection swipeDirection)
		{
			if(_content == null)
				_content = Content;

			switch (swipeDirection)
			{
				case SwipeDirection.Left:
					if(_rightItems == null)
						_rightItems = CreateSwipeItems(RightItems);

					if (Children.IndexOf(_rightItems) == -1)
						LowerChild(_rightItems);

					_rightItems.IsVisible = true;
					break;
				case SwipeDirection.Right:
					if (_leftItems == null)
						_leftItems = CreateSwipeItems(LeftItems);

					if (Children.IndexOf(_leftItems) == -1)
						LowerChild(_leftItems);

					_leftItems.IsVisible = true;
					break;
				case SwipeDirection.Up:
					if (_bottomItems == null)
						_bottomItems = CreateSwipeItems(BottomItems);

					if (Children.IndexOf(_bottomItems) == -1)
						LowerChild(_bottomItems);

					_bottomItems.IsVisible = true;
					break;
				case SwipeDirection.Down:
					if (_topItems == null)
						_topItems = CreateSwipeItems(TopItems);

					if (Children.IndexOf(_topItems) == -1)
						LowerChild(_topItems);

					_topItems.IsVisible = true;
					break;
			}
		}

		private void LayoutSwipeView(SwipeDirection swipeDirection, double swipeOffset)
		{
			switch (swipeDirection)
			{
				case SwipeDirection.Left:
				case SwipeDirection.Right:
					_content.TranslationX = ValidateOffset(swipeOffset);
					break;
				case SwipeDirection.Up:
				case SwipeDirection.Down:
					_content.TranslationY = ValidateOffset(swipeOffset);
					break;
			}
		}

		private void ResetSwipe()
		{
			_content.TranslationX = 0;
			_content.TranslationY = 0;
			_isInSwiping = false;
		}

		private StackLayout CreateSwipeItems(SwipeItems swipeItems)
		{
			var swipeItemsLayout = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
				IsVisible = false
			};

			if (swipeItems != null)
			{
				foreach (SwipeItem swipeItem in swipeItems)
				{
					swipeItemsLayout.Children.Add(swipeItem);
				}
			}

			return swipeItemsLayout;
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
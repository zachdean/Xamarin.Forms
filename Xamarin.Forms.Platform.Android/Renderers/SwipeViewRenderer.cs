using System;
using Android.Content;
using Android.Views;
using AView = Android.Views.View;

namespace Xamarin.Forms.Platform.Android
{
	public class SwipeViewRenderer : ViewRenderer<SwipeView, AView>, GestureDetector.IOnGestureListener
	{
		private bool _isDisposed;
		private float _downX;
		private float _downY;
		private float _density;
		private GestureDetector _detector;

		public SwipeViewRenderer(Context context) : base(context)
		{

		}

		protected override void OnElementChanged(ElementChangedEventArgs<SwipeView> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement != null)
			{
				if (Control == null)
				{
					_density = Resources.DisplayMetrics.Density;

					_detector = new GestureDetector(Context, this);
				}
			}
		}

		public override bool OnTouchEvent(MotionEvent e)
		{
			base.OnTouchEvent(e);

			var density = Resources.DisplayMetrics.Density;
			float x = Math.Abs((_downX - e.GetX()) / density);
			float y = Math.Abs((_downY - e.GetY()) / density);

			if (e.Action != MotionEventActions.Move | (x > 10f || y > 10f))
			{
				_detector.OnTouchEvent(e);
			}

			ProcessSwipingInteractions(e);

			return true;
		}

		protected override void Dispose(bool disposing)
		{
			if (_isDisposed)
			{
				return;
			}

			if (disposing)
			{
				if (_detector != null)
				{
					_detector.Dispose();
					_detector = null;
				}
			}

			_isDisposed = true;

			base.Dispose(disposing);
		}

		public bool OnDown(MotionEvent e)
		{
			return true;
		}

		public bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
		{
			return true;
		}

		public void OnLongPress(MotionEvent e)
		{

		}

		public bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
		{
			return true;
		}

		public void OnShowPress(MotionEvent e)
		{

		}

		public bool OnSingleTapUp(MotionEvent e)
		{
			return true;
		}

		private bool ProcessSwipingInteractions(MotionEvent e)
		{
			bool handled = true;
			var point = new Point(e.GetX() / _density, e.GetY() / _density);

			switch (e.Action)
			{
				case MotionEventActions.Down:
					_downX = e.RawX;
					_downY = e.RawY;

					handled = Element.HandleTouchInteractions(GestureStatus.Started, point);
					break;
				case MotionEventActions.Up:
					handled = Element.HandleTouchInteractions(GestureStatus.Completed, point);

					if (Parent == null)
						break;

					Parent.RequestDisallowInterceptTouchEvent(false);
					break;
				case MotionEventActions.Move:
					handled = Element.HandleTouchInteractions(GestureStatus.Running, point);

					if (handled || Parent == null)
						break;

					Parent.RequestDisallowInterceptTouchEvent(true);
					break;
				case MotionEventActions.Cancel:
					handled = Element.HandleTouchInteractions(GestureStatus.Canceled, point);

					if (Parent == null)
						break;

					Parent.RequestDisallowInterceptTouchEvent(false);
					break;
			}
			return !handled;
		}
	}
}
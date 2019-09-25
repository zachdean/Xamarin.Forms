using System;
using System.Linq;
using Foundation;
using UIKit;

namespace Xamarin.Forms.Platform.iOS
{
	public class SwipeViewRenderer : ViewRenderer<SwipeView, UIView>
	{
		internal const string SwipeView = "Xamarin.SwipeView";
		internal const string CloseSwipeView = "Xamarin.CloseSwipeView";

		private bool _isDisposed;

		protected override void OnElementChanged(ElementChangedEventArgs<SwipeView> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement != null)
			{
				if (Control == null)
				{
					MessagingCenter.Subscribe<string>(SwipeView, CloseSwipeView, OnClose);
				}
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (_isDisposed)
			{
				return;
			}

			if (disposing)
			{
				MessagingCenter.Unsubscribe<string>(SwipeView, CloseSwipeView);
			}

			_isDisposed = true;

			base.Dispose(disposing);
		}

		public override void TouchesBegan(NSSet touches, UIEvent evt)
		{
			var navigationController = GetUINavigationController(GetViewController());

			if (navigationController != null)
				navigationController.InteractivePopGestureRecognizer.Enabled = false;

			HandleTouchInteractions(touches, GestureStatus.Started);
			base.TouchesBegan(touches, evt);
		}

		public override void TouchesMoved(NSSet touches, UIEvent evt)
		{
			HandleTouchInteractions(touches, GestureStatus.Running);
			base.TouchesMoved(touches, evt);
		}

		public override void TouchesCancelled(NSSet touches, UIEvent evt)
		{
			var navigationController = GetUINavigationController(GetViewController());

			if (navigationController != null)
				navigationController.InteractivePopGestureRecognizer.Enabled = true;

			HandleTouchInteractions(touches, GestureStatus.Completed);
			base.TouchesCancelled(touches, evt);
		}

		public override void TouchesEnded(NSSet touches, UIEvent evt)
		{
			var navigationController = GetUINavigationController(GetViewController());

			if (navigationController != null)
				navigationController.InteractivePopGestureRecognizer.Enabled = true;

			HandleTouchInteractions(touches, GestureStatus.Completed);
			base.TouchesEnded(touches, evt);
		}

		void HandleTouchInteractions(NSSet touches, GestureStatus gestureStatus)
		{
			var anyObject = touches.AnyObject as UITouch;
			nfloat x = anyObject.LocationInView(this).X;
			nfloat y = anyObject.LocationInView(this).Y;

			if (Element is ISwipeViewController swipeViewController)
				swipeViewController.HandleTouchInteractions(gestureStatus, new Point(x, y));
		}

		void OnClose(object sender)
		{
			if (sender == null)
				return;

			var swipeViewController = Element as ISwipeViewController;
			swipeViewController?.CloseSwipe();
		}

		UIViewController GetViewController()
		{
			var window = UIApplication.SharedApplication.KeyWindow;
			var viewController = window.RootViewController;

			while (viewController.PresentedViewController != null)
				viewController = viewController.PresentedViewController;

			return viewController;
		}

		UINavigationController GetUINavigationController(UIViewController controller)
		{
			if (controller != null)
			{
				if (controller is UINavigationController)
				{
					return (controller as UINavigationController);
				}

				if (controller.ChildViewControllers.Any())
				{
					var childs = controller.ChildViewControllers.Count();

					for (int i = 0; i < childs; i++)
					{
						var child = GetUINavigationController(controller.ChildViewControllers[i]);

	  					if (child is UINavigationController)
						{
							return (child as UINavigationController);
						}
					}
				}
			}

			return null;
		}
	}
}
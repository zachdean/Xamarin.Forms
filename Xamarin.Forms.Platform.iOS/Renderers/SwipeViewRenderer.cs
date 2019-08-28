using System;
using Foundation;
using UIKit;

namespace Xamarin.Forms.Platform.iOS
{
	public class SwipeViewRenderer : ViewRenderer
	{
		public override void TouchesBegan(NSSet touches, UIEvent evt)
		{
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
			HandleTouchInteractions(touches, GestureStatus.Completed);
			base.TouchesCancelled(touches, evt);
		}

		public override void TouchesEnded(NSSet touches, UIEvent evt)
		{
			HandleTouchInteractions(touches, GestureStatus.Completed);
			base.TouchesEnded(touches, evt);
		}

		private void HandleTouchInteractions(NSSet touches, GestureStatus gestureStatus)
		{
			var anyObject = touches.AnyObject as UITouch;
			nfloat x = anyObject.LocationInView(this).X;
			nfloat y = anyObject.LocationInView(this).Y;

			if(Element is SwipeView swipeView)
				swipeView.HandleTouchInteractions(gestureStatus, new Point(x, y));
		}
	}
}
using CoreGraphics;
using UIKit;

namespace Xamarin.Platform
{
	public class NativeFrame : UIView
	{
		public override void RemoveFromSuperview()
		{
			for (var i = Subviews.Length - 1; i >= 0; i--)
			{
				var item = Subviews[i];
				item.RemoveFromSuperview();
			}
		}

		public override bool PointInside(CGPoint point, UIEvent? uievent)
		{
			foreach (var view in Subviews)
			{
				if (view.HitTest(ConvertPointToView(point, view), uievent) != null)
					return true;
			}

			return false;
		}
	}
}
using CoreGraphics;
using UIKit;
using XColor = Xamarin.Forms.Color;

namespace Xamarin.Platform
{
	public static class FrameExtensions
	{
		public static void UpdateBackgroundColor(this UIView nativeView, IFrame frame)
		{
			nativeView.UpdateBackgroundColor(frame, null);
		}

		public static void UpdateBackgroundColor(this UIView nativeView, IFrame frame, NativeFrame? nativeFrame)
		{
			nativeView.SetupLayer(frame, nativeFrame);
		}

		public static void UpdateBorderColor(this UIView nativeView, IFrame frame)
		{
			nativeView.UpdateBorderColor(frame, null);
		}

		public static void UpdateBorderColor(this UIView nativeView, IFrame frame, NativeFrame? nativeFrame)
		{
			nativeView.SetupLayer(frame, nativeFrame);
		}

		public static void UpdateHasShadow(this UIView nativeView, IFrame frame)
		{
			nativeView.UpdateHasShadow(frame, null);
		}

		public static void UpdateHasShadow(this UIView nativeView, IFrame frame, NativeFrame? nativeFrame)
		{
			nativeView.SetupLayer(frame, nativeFrame);
		}

		public static void UpdateCornerRadius(this UIView nativeView, IFrame frame)
		{
			nativeView.UpdateCornerRadius(frame, null);
		}

		public static void UpdateCornerRadius(this UIView nativeView, IFrame frame, NativeFrame? nativeFrame)
		{
			nativeView.SetupLayer(frame, nativeFrame);
		}

		internal static void SetupLayer(this UIView nativeView, IFrame frame, NativeFrame? nativeFrame)
		{
			if (nativeView == null || nativeFrame == null)
				return;

			float cornerRadius = frame.CornerRadius;

			if (cornerRadius == -1f)
				cornerRadius = 5f; // Default corner radius

			nativeFrame.Layer.CornerRadius = cornerRadius;
			nativeFrame.Layer.MasksToBounds = cornerRadius > 0;

			if (frame.BackgroundColor == XColor.Default)
				nativeFrame.Layer.BackgroundColor = ColorExtensions.BackgroundColor.CGColor;
			else
			{
				// BackgroundColor gets set on the base class too which messes with
				// the corner radius, shadow, etc. so override that behaviour here
				nativeView.BackgroundColor = UIColor.Clear;
				nativeFrame.Layer.BackgroundColor = frame.BackgroundColor.ToCGColor();
			}

			if (frame.BorderColor == XColor.Default)
				nativeFrame.Layer.BorderColor = UIColor.Clear.CGColor;
			else
			{
				nativeFrame.Layer.BorderColor = frame.BorderColor.ToCGColor();
				nativeFrame.Layer.BorderWidth = 1;
			}

			if (frame.HasShadow)
			{
				nativeView.Layer.ShadowRadius = 5;
				nativeView.Layer.ShadowColor = UIColor.Black.CGColor;
				nativeView.Layer.ShadowOpacity = 0.8f;
				nativeView.Layer.ShadowOffset = CGSize.Empty;
			}
			else
			{
				nativeView.Layer.ShadowOpacity = 0;
			}

			nativeView.Layer.RasterizationScale = UIScreen.MainScreen.Scale;
			nativeView.Layer.ShouldRasterize = true;
			nativeView.Layer.MasksToBounds = false;

			nativeFrame.Layer.RasterizationScale = UIScreen.MainScreen.Scale;
			nativeFrame.Layer.ShouldRasterize = true;
		}
	}
}
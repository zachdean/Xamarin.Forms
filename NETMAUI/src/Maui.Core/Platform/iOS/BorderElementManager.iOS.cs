using UIKit;

namespace System.Maui.Platform
{
	internal class BorderElementManager
	{
		static nfloat DefaultCornerRadius = 5;

		static IViewHandler Handler;
		static IBorder Border;

		public BorderElementManager(IViewHandler handler, IBorder border)
		{
			Handler = handler;
			Border = border;
		}

		public void Dispose()
		{
			Handler = null;
			Border = null;
		}

		public void UpdateBorder()
		{
			if (!(Handler.NativeView is UIView control))
				return;

			if (Border.BorderColor != Color.Default)
				control.Layer.BorderColor = Border.BorderColor.ToCGColor();

			control.Layer.BorderWidth = Math.Max(0f, (float)Border.BorderWidth);

			nfloat cornerRadius = DefaultCornerRadius;

			if (Border.CornerRadius != 0.0d)
				cornerRadius = Border.CornerRadius;

			control.Layer.CornerRadius = cornerRadius;
		}
	}
}
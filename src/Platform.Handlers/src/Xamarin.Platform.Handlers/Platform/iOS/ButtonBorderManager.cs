using System;
using UIKit;
using Xamarin.Forms;

namespace Xamarin.Platform
{
	public static class BorderElementManager
	{
		static nfloat DefaultCornerRadius = 5;
	
		public static void UpdateBorder(UIButton nativeView, IBorder border)
		{
			if (nativeView == null)
				return;

			if (border.BorderColor != Color.Default)
				nativeView.Layer.BorderColor = border.BorderColor.ToCGColor();

			nativeView.Layer.BorderWidth = Math.Max(0f, (float)border.BorderWidth);

			nfloat cornerRadius = DefaultCornerRadius;

			if (border.CornerRadius != -1)
				cornerRadius = border.CornerRadius;

			nativeView.Layer.CornerRadius = cornerRadius;
		}
	}
}
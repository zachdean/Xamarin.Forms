using Android.Graphics.Drawables;
using AndroidX.CardView.Widget;
using AColor = Android.Graphics.Color;
using XColor = Xamarin.Forms.Color;

namespace Xamarin.Platform
{
	public static class FrameExtensions
	{
		public static void UpdateBackgroundColor(this CardView cardView, IFrame frame)
		{
			var gradientdrawable = cardView.Background as GradientDrawable;
			cardView.UpdateBorderColor(frame, gradientdrawable);
		}

		public static void UpdateBackgroundColor(this CardView cardView, IFrame frame, GradientDrawable? gradientDrawable)
		{
			XColor backgroundColor = frame.BackgroundColor;
			gradientDrawable?.SetColor(backgroundColor.IsDefault ? AColor.White : backgroundColor.ToNative());
		}

		public static void UpdateBorderColor(this CardView cardView, IFrame frame)
		{
			var gradientdrawable = cardView.Background as GradientDrawable;
			cardView.UpdateBorderColor(frame, gradientdrawable);
		}

		public static void UpdateBorderColor(this CardView cardView, IFrame frame, GradientDrawable? gradientDrawable)
		{
			if (cardView == null)
				return;

			XColor borderColor = frame.BorderColor;

			if (borderColor.IsDefault)
				gradientDrawable?.SetStroke(0, AColor.Transparent);
			else
				gradientDrawable?.SetStroke(3, borderColor.ToNative());
		}

		public static void UpdateHasShadow(this CardView cardView, IFrame frame)
		{
			cardView.UpdateHasShadow(frame, null);
		}

		public static void UpdateHasShadow(this CardView cardView, IFrame frame, float? defaultElevation)
		{
			if (defaultElevation == -1f)
				defaultElevation = cardView.CardElevation;

			float elevation = defaultElevation ?? -1f;

			if (elevation == -1f)
				elevation = cardView.CardElevation;

			if (frame.HasShadow)
				cardView.CardElevation = elevation;
			else
				cardView.CardElevation = 0f;
		}

		public static void UpdateCornerRadius(this CardView cardView, IFrame frame)
		{
			var gradientdrawable = cardView.Background as GradientDrawable;
			cardView.UpdateCornerRadius(frame, gradientdrawable, null);
		}

		public static void UpdateCornerRadius(this CardView cardView, IFrame frame, GradientDrawable? gradientDrawable, float? defaultCornerRadius)
		{
			if (defaultCornerRadius == -1f)
				defaultCornerRadius = cardView.Radius;

			float cornerRadius = frame.CornerRadius;

			if (cornerRadius == -1f)
				cornerRadius = defaultCornerRadius ?? 0;
			else
				cornerRadius = cardView.Context?.ToPixels(cornerRadius) ?? 0;

			gradientDrawable?.SetCornerRadius(cornerRadius);
		}
	}
}
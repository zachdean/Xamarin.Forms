using System;
using ADrawable = Android.Graphics.Drawables.Drawable;
using AColorFilter = Android.Graphics.ColorFilter;
using AColor = Android.Graphics.Color;
using ADrawableCompat = Android.Support.V4.Graphics.Drawable.DrawableCompat;
using Android.Graphics;

namespace Xamarin.Forms.Platform.Android
{
	enum FilterMode
	{
		SrcIn,
		Multiply,
		SrcAtop
	}

	internal static class DrawableExtensions
	{

#if __ANDROID_29__
		public static BlendMode GetFilterMode(FilterMode mode)
		{
			switch (mode)
			{
				case FilterMode.SrcIn:
					return BlendMode.SrcIn;
				case FilterMode.Multiply:
					return BlendMode.Multiply;
				case FilterMode.SrcAtop:
					return BlendMode.SrcAtop;
			}

			throw new Exception("Invalid Mode");
		}
#else
		public static PorterDuff.Mode GetFilterMode(FilterMode mode)
		{
			switch (mode)
			{
				case FilterMode.SrcIn:
					return PorterDuff.Mode.SrcIn;
				case FilterMode.Multiply:
					return PorterDuff.Mode.Multiply;
				case FilterMode.SrcAtop:
					return PorterDuff.Mode.SrcAtop;
			}

			throw new Exception("Invalid Mode");
		}
#endif

		public static AColorFilter GetColorFilter(this ADrawable drawable)
		{
			if (drawable == null)
				return null;

			return ADrawableCompat.GetColorFilter(drawable);
		}

		public static void SetColorFilter(this ADrawable drawable, AColorFilter colorFilter)
		{
			if (drawable == null)
				return;

			if (colorFilter == null)
				ADrawableCompat.ClearColorFilter(drawable);

			drawable.SetColorFilter(colorFilter);
		}


		public static void SetColorFilter(this ADrawable drawable, Color color, AColorFilter defaultColorFilter, FilterMode mode)
		{
			if (drawable == null)
				return;

			if (color == Color.Default)
			{
				SetColorFilter(drawable, defaultColorFilter);
				return;
			}

			drawable.SetColorFilter(color.ToAndroid(), mode);
		}

		public static void SetColorFilter(this ADrawable drawable, Color color, FilterMode mode)
		{
			drawable.SetColorFilter(color.ToAndroid(), mode);
		}

		public static void SetColorFilter(this ADrawable drawable, AColor color, FilterMode mode)
		{
#if __ANDROID_29__
			drawable.SetColorFilter(new BlendModeColorFilter(color, GetFilterMode(mode)));
#else
			drawable.SetColorFilter(color, GetFilterMode(mode));
#endif
		}

	}
}

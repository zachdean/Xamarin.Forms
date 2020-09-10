using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.OS;
using AView = Android.Views.View;

namespace System.Maui.Platform
{
	public static class ViewExtensions
	{
		static readonly int _apiLevel;

		static ViewExtensions()
		{
			_apiLevel = (int)Build.VERSION.SdkInt;
		}

		public static void SetTextColor(this AndroidX.AppCompat.Widget.AppCompatButton button, Color color, Color defaultColor)
			=> button.SetTextColor(color.Cleanse(defaultColor).ToNative());

		public static void SetTextColor(this AndroidX.AppCompat.Widget.AppCompatButton button, Color color, ColorStateList defaultColor)
		{
			if (color.IsDefault)
				button.SetTextColor(defaultColor);
			else
				button.SetTextColor(color.ToNative());
		}
		static Color Cleanse(this Color color, Color defaultColor) => color.IsDefault ? defaultColor : color;

		public static void SetText(this AndroidX.AppCompat.Widget.AppCompatButton button, string text)
			=> button.Text = text;

		public static void SetBackground(this AView view, Drawable drawable)
		{
			if (_apiLevel < 16)
			{
#pragma warning disable 618 // Using older method for compatibility with API 15
				view.SetBackgroundDrawable(drawable);
#pragma warning restore 618
			}
			else
			{
				view.Background = drawable;
			}
		}
	}
}

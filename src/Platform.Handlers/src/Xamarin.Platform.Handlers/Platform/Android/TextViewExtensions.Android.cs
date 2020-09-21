using System;
using System.Collections.Generic;
using System.Text;
using Android.Widget;

namespace Xamarin.Platform
{
	public static class TextViewExtensions
	{
		public static void UpdateText(this TextView textView, IText text)
		{
			textView.Text = text.Text;
		}

		public static void UpdateTextColor(this TextView textView, IText text)
		{
			textView.SetTextColor(text.TextColor.ToNative());
		}
	}
}

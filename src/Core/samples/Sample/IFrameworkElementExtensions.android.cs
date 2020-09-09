using System;
using System.Collections.Generic;
using System.Maui;
using System.Text;
using Android.Content;

namespace Sample
{
	public static class IFrameworkElementExtensions
	{
		public static Context GetContext(this IFrameworkElement frameworkElement)
		{
			return (frameworkElement.Renderer.NativeView as Android.Views.View).Context;
		}
	}
}

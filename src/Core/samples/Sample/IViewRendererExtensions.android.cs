using System;
using System.Collections.Generic;
using System.Maui;
using System.Text;
using Android.Content;

namespace Sample
{
	public static class IViewRendererExtensions
	{
		public static Context GetContext(this IViewRenderer viewRenderer)
		{
			return (viewRenderer.NativeView as Android.Views.View).Context;
		}
	}
}

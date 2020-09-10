using System;
using Xamarin.Platform.Core;
using Android.Content;
using AView = Android.Views.View;

namespace Xamarin.Platform {
	public static class RendererExtensions {
		public static AView ToNative (this IFrameworkElement view, Context context)
		{
			if (view == null)
				return null;
			var handler = view.Renderer;
			if (handler == null)
			{
				handler = Registrar.Handlers.GetHandler(view.GetType());
				if (handler is IAndroidViewRenderer arenderer)
					arenderer.SetContext (context);
				view.Renderer = handler;
			}
			handler.SetView (view);
			return handler.NativeView as AView;

			//return handler?.ContainerView ?? handler.NativeView as AView;

		}

	}
}

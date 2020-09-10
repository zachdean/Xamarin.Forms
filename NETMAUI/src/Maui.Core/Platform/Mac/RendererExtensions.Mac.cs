using System;
using AppKit;

namespace System.Maui
{
	public static class HandlerExtensions
	{
		public static NSView ToNative(this IView view)
		{
			if (view == null)
				return null;

			var handler = view.Handler;

			if (handler == null)
			{
				handler = Registrar.Handlers.GetHandler(view.GetType());
				view.Handler = handler;
			}

			if (handler == null)
				throw new InvalidOperationException("No handler was registered for this view type");

			handler.SetView(view);

			return handler.ContainerView ?? handler.NativeView as NSView;
		}
	}
}

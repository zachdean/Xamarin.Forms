using UIKit;

namespace System.Maui.Platform
{
	public static class HandlerExtensions
	{
		public static UIView ToNative(this IView view)
		{
			if (view == null)
				return null;

			var handler = view.Handler;

			if (handler == null)
			{
				handler = Registrar.Handlers.GetHandler(view.GetType());
				view.Handler = handler;
			}

			handler.SetView(view);

			return handler?.ContainerView ?? handler.NativeView as UIView;
		}
	}
}
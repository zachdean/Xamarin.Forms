using Android.Graphics.Drawables;
using AView = Android.Views.View;

namespace System.Maui.Platform
{
    public partial class ViewHandler
	{
		public static void MapPropertyIsEnabled(IViewHandler Handler, IView view)
		{
			if (Handler.NativeView is AView nativeView)
				nativeView.Enabled = view.IsEnabled;
		}

		public static void MapBackgroundColor(IViewHandler Handler, IView view)
		{
			var aview = Handler.NativeView as AView;
			var backgroundColor = view.BackgroundColor;

			if (backgroundColor.IsDefault)
				aview.Background = null;
			else
				aview.Background = new ColorDrawable { Color = backgroundColor.ToNative() };
		}
	}
}
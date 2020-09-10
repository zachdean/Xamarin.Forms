using System.Maui.Platform;

#if __MOBILE__
using NativeColor = UIKit.UIColor;
using NativeControl = UIKit.UIControl;
using NativeView = UIKit.UIView;

#else
using NativeView = AppKit.NSView;
using NativeColor = CoreGraphics.CGColor;
using NativeControl = AppKit.NSControl;

#endif

namespace System.Maui.Platform
{
	public partial class ViewHandler
	{
		public static void MapPropertyIsEnabled(IViewHandler Handler, IView view)
		{
			if (!(Handler.NativeView is NativeControl uiControl))
				return;
			uiControl.Enabled = view.IsEnabled;
		}

		public static void MapBackgroundColor(IViewHandler Handler, IView view)
		{
			var nativeView = (NativeView)Handler.NativeView;
			var color = view.BackgroundColor;

			if (color != null && !color.IsDefault)
				nativeView.SetBackgroundColor(color.ToNative());
		}
	}
}
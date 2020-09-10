using CoreGraphics;
using UIKit;

namespace System.Maui.Platform
{
	public partial class ActivityIndicatorHandler : AbstractViewHandler<IActivityIndicator, UIActivityIndicatorView>
	{
		protected override UIActivityIndicatorView CreateView()
		{
#if __XCODE11__
			if(NativeVersion.Supports(NativeApi.UIActivityIndicatorViewStyleMedium))
				return new UIActivityIndicatorView(CGRect.Empty) { ActivityIndicatorViewStyle = UIActivityIndicatorViewStyle.Medium };
			else
#endif
			return new UIActivityIndicatorView(CGRect.Empty) { ActivityIndicatorViewStyle = UIActivityIndicatorViewStyle.Gray };
		}

		public static void MapPropertyIsRunning(IViewHandler Handler, IActivityIndicator activityIndicator)
		{
			if (!(Handler.NativeView is UIActivityIndicatorView uIActivityIndicatorView))
				return;

			if (activityIndicator.IsRunning)
				uIActivityIndicatorView.StartAnimating();
			else
				uIActivityIndicatorView.StopAnimating();
		}

		public static void MapPropertyColor(IViewHandler Handler, IActivityIndicator activityIndicator)
		{
			if (!(Handler.NativeView is UIActivityIndicatorView uIActivityIndicatorView))
				return;

			uIActivityIndicatorView.Color = activityIndicator.Color == Color.Default ? null : activityIndicator.Color.ToNative();
		}
	}
}
using System.Linq;
using AppKit;
using CoreGraphics;
using CoreImage;

namespace System.Maui.Platform
{
	public partial class ActivityIndicatorHandler : AbstractViewHandler<IActivityIndicator, NSProgressIndicator>
	{
		CIColorPolynomial _currentColorFilter;
		NSColor _currentColor;

		protected override NSProgressIndicator CreateView()
		{
			return new NSProgressIndicator(CGRect.Empty) { Style = NSProgressIndicatorStyle.Spinning };
		}

		public static void MapPropertyIsRunning(IViewHandler Handler, IActivityIndicator activityIndicator)
		{
			if (!(Handler.NativeView is NSProgressIndicator nSProgressIndicator))
				return;

			if (activityIndicator.IsRunning)
				nSProgressIndicator.StartAnimation(Handler.ContainerView);
			else
				nSProgressIndicator.StopAnimation(Handler.ContainerView);
		}

		public static void MapPropertyColor(IViewHandler Handler, IActivityIndicator activityIndicator)
		{
			if (!(Handler is ActivityIndicatorHandler activityIndicatorHandler) || !(Handler.NativeView is NSProgressIndicator nSProgressIndicator))
				return;

			var color = activityIndicator.Color;

			if (activityIndicatorHandler._currentColorFilter == null && color.IsDefault)
				return;

			if (color.IsDefault)
				nSProgressIndicator.ContentFilters = new CIFilter[0];

			var newColor = activityIndicator.Color.ToNative();

			if (Equals(activityIndicatorHandler._currentColor, newColor))
			{
				if (nSProgressIndicator.ContentFilters?.FirstOrDefault() != activityIndicatorHandler._currentColorFilter)
				{
					nSProgressIndicator.ContentFilters = new CIFilter[] { activityIndicatorHandler._currentColorFilter };
				}
				return;
			}

			activityIndicatorHandler._currentColor = newColor;

			activityIndicatorHandler._currentColorFilter = new CIColorPolynomial
			{
				RedCoefficients = new CIVector(activityIndicatorHandler._currentColor.RedComponent),
				BlueCoefficients = new CIVector(activityIndicatorHandler._currentColor.BlueComponent),
				GreenCoefficients = new CIVector(activityIndicatorHandler._currentColor.GreenComponent)
			};

			nSProgressIndicator.ContentFilters = new CIFilter[] { activityIndicatorHandler._currentColorFilter };
		}
	}
}
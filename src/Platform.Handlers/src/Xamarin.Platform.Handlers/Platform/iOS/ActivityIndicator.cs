using UIKit;
using Xamarin.Forms;

namespace Xamarin.Platform
{
	public static class ActivityIndicatorExtensions
	{
		public static void UpdateIsRunning(this UIActivityIndicatorView activityIndicatorView, IActivityIndicator activityIndicator)
		{
			if (activityIndicator.IsRunning)
				activityIndicatorView.StartAnimating();
			else
				activityIndicatorView.StopAnimating();
		}

		public static void UpdateColor(this UIActivityIndicatorView activityIndicatorView, IActivityIndicator activityIndicator)
		{
			activityIndicatorView.Color = activityIndicator.Color == Color.Default ? null : activityIndicator.Color.ToNative();
		}
	}
}
using System.Linq;
using AppKit;
using CoreImage;

namespace Xamarin.Platform
{
	public static class ActivityIndicatorExtensions
	{
		static CIColorPolynomial? CurrentColorFilter;
		static NSColor? CurrentColor;

		public static void UpdateIsRunning(this NSProgressIndicator nSProgressIndicator, IActivityIndicator activityIndicator)
		{
			if (activityIndicator.IsRunning)
				nSProgressIndicator.StartAnimation(nSProgressIndicator);
			else
				nSProgressIndicator.StopAnimation(nSProgressIndicator);
		}

		public static void UpdateColor(this NSProgressIndicator nSProgressIndicator, IActivityIndicator activityIndicator)
		{
			var color = activityIndicator.Color;

			if (CurrentColorFilter == null && color.IsDefault)
				return;

			if (color.IsDefault)
				nSProgressIndicator.ContentFilters = new CIFilter[0];

			var newColor = activityIndicator.Color.ToNative();

			if (Equals(CurrentColor, newColor))
			{
				if (CurrentColorFilter != null && nSProgressIndicator.ContentFilters?.FirstOrDefault() != CurrentColorFilter)
				{
					nSProgressIndicator.ContentFilters = new CIFilter[] { CurrentColorFilter };
				}
				return;
			}

			CurrentColor = newColor;

			CurrentColorFilter = new CIColorPolynomial
			{
				RedCoefficients = new CIVector(CurrentColor.RedComponent),
				BlueCoefficients = new CIVector(CurrentColor.BlueComponent),
				GreenCoefficients = new CIVector(CurrentColor.GreenComponent)
			};

			nSProgressIndicator.ContentFilters = new CIFilter[] { CurrentColorFilter };
		}
	}
}
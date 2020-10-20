using System.Linq;
using AppKit;
using CoreImage;

namespace Xamarin.Platform
{
	public static class ProgressBarExtensions
	{
		static CIColorPolynomial? CurrentColorFilter;
		static NSColor? CurrentColor;

		public static void UpdateProgress(this NSProgressIndicator nativeProgressBar, IProgress progress)
		{
			nativeProgressBar.DoubleValue = progress.Progress;
		}

		public static void UpdateProgressColor(this NSProgressIndicator nativeProgressBar, IProgress progress)
		{
			var progressColor = progress.ProgressColor;

			if (progressColor.IsDefault)
				return;

			var newProgressColor = progressColor.ToNative();

			if (Equals(CurrentColor, newProgressColor))
			{
				if (CurrentColorFilter != null && nativeProgressBar.ContentFilters?.FirstOrDefault() != CurrentColorFilter)
				{
					nativeProgressBar.ContentFilters = new CIFilter[] { CurrentColorFilter };
				}

				return;
			}

			CurrentColor = newProgressColor;

			CurrentColorFilter = new CIColorPolynomial
			{
				RedCoefficients = new CIVector(CurrentColor.RedComponent),
				BlueCoefficients = new CIVector(CurrentColor.BlueComponent),
				GreenCoefficients = new CIVector(CurrentColor.GreenComponent)
			};

			nativeProgressBar.ContentFilters = new CIFilter[] { CurrentColorFilter };
		}
	}
}
using System;

namespace Xamarin.Platform
{
	public static class SliderExtensions
	{
		public static void UpdateMinimum(this NativeSlider nativeSlider, ISlider slider)
		{
			nativeSlider.MinValue = (float)slider.Minimum;
		}

		public static void UpdateMaximum(this NativeSlider nativeSlider, ISlider slider)
		{
			nativeSlider.MaxValue = (float)slider.Maximum;
		}

		public static void UpdateValue(this NativeSlider nativeSlider, ISlider slider)
		{
			if (Math.Abs(slider.Value - nativeSlider.DoubleValue) > 0)
				nativeSlider.DoubleValue = (float)slider.Value;
		}

		public static void UpdateMinimumTrackColor(this NativeSlider nativeSlider, ISlider slider)
		{
			// Cell could be overwritten with an other custom cell
			if (nativeSlider.Cell is NativeSliderCell sliderCell)
			{
				sliderCell.MinimumTrackColor = slider.MinimumTrackColor;
			}
		}

		public static void UpdateMaximumTrackColor(this NativeSlider nativeSlider, ISlider slider)
		{
			// Cell could be overwritten with an other custom cell
			if (nativeSlider.Cell is NativeSliderCell sliderCell)
			{
				sliderCell.MaximumTrackColor = slider.MaximumTrackColor;
			}
		}

		public static void UpdateThumbColor(this NativeSlider nativeSlider, ISlider slider)
		{
			// Cell could be overwritten with an other custom cell
			if (nativeSlider.Cell is NativeSliderCell sliderCell)
			{
				sliderCell.ThumbColor = slider.ThumbColor;
			}
		}
	}
}
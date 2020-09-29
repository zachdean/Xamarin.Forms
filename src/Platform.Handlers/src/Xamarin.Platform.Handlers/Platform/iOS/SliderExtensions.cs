using UIKit;
using Xamarin.Forms;
using Xamarin.Platform.Handlers;

namespace Xamarin.Platform
{
	public static class SliderExtensions
	{ 
		public static void UpdateMinimum(this UISlider uiSlider, ISlider slider)
		{
			uiSlider.MaxValue = (float)slider.Maximum;
		}

		public static void UpdateMaximum(this UISlider uiSlider, ISlider slider)
		{
			uiSlider.MinValue = (float)slider.Minimum;
		}

		public static void UpdateValue(this UISlider uiSlider, ISlider slider)
		{
			if ((float)slider.Value != uiSlider.Value)
				uiSlider.Value = (float)slider.Value;
		}

		public static void UpdateMinimumTrackColor(this UISlider uiSlider, SliderHandler sliderHandler, ISlider slider)
		{
			if (slider.MinimumTrackColor == Color.Default)
				uiSlider.MinimumTrackTintColor = sliderHandler._defaultMinTrackColor;
			else
				uiSlider.MinimumTrackTintColor = slider.MinimumTrackColor.ToNative();
		}

		public static void UpdateMaximumTrackColor(this UISlider uiSlider, SliderHandler sliderHandler, ISlider slider)
		{
			if (slider.MaximumTrackColor == Color.Default)
				uiSlider.MaximumTrackTintColor = sliderHandler._defaultMaxTrackColor;
			else
				uiSlider.MaximumTrackTintColor = slider.MaximumTrackColor.ToNative();
		}

		public static void UpdateThumbColor(this UISlider uiSlider, SliderHandler sliderHandler, ISlider slider)
		{
			if (slider.ThumbColor == Color.Default)
				uiSlider.ThumbTintColor = sliderHandler._defaultThumbColor;
			else
				uiSlider.ThumbTintColor = slider.ThumbColor.ToNative();
		}
	}
}
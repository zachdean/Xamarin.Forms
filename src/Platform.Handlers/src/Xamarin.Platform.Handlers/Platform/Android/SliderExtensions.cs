using Android.Content.Res;
using Android.Graphics;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Platform.Handlers;

namespace Xamarin.Platform
{
	public static class SliderExtensions
	{ 
		public static void UpdateMinimum(this SeekBar seekBar, ISlider slider)
		{
			seekBar.Min = (int)slider.Minimum;
		}

		public static void UpdateMaximum(this SeekBar seekBar, ISlider slider)
		{
			seekBar.Max = (int)slider.Maximum;
		}

		public static void UpdateValue(this SeekBar seekBar, ISlider slider)
		{
			var min = slider.Minimum;
			var max = slider.Maximum;
			var value = slider.Value;

			seekBar.Progress = (int)((value - min) / (max - min) * 1000.0);
		}

		public static void UpdateMinimumTrackColor(this SeekBar seekBar, SliderHandler sliderHandler, ISlider slider)
		{
			if (slider.MinimumTrackColor == Forms.Color.Default)
			{
				seekBar.ProgressTintList = sliderHandler._defaultProgressTintList;
				seekBar.ProgressTintMode = sliderHandler._defaultProgressTintMode;
			}
			else
			{
				seekBar.ProgressTintList = ColorStateList.ValueOf(slider.MinimumTrackColor.ToNative());
				seekBar.ProgressTintMode = PorterDuff.Mode.SrcIn;
			}
		}

		public static void UpdateMaximumTrackColor(this SeekBar seekBar, SliderHandler sliderHandler, ISlider slider)
		{
			if (slider.MaximumTrackColor == Forms.Color.Default)
			{
				seekBar.ProgressBackgroundTintList = sliderHandler._defaultProgressBackgroundTintList;
				seekBar.ProgressBackgroundTintMode = sliderHandler._defaultProgressBackgroundTintMode;
			}
			else
			{
				seekBar.ProgressBackgroundTintList = ColorStateList.ValueOf(slider.MaximumTrackColor.ToNative());
				seekBar.ProgressBackgroundTintMode = PorterDuff.Mode.SrcIn;
			}
		}

		public static void UpdateThumbColor(this SeekBar seekBar, SliderHandler sliderHandler, ISlider slider)
		{
			seekBar.Thumb?.SetColorFilter(slider.ThumbColor, sliderHandler._defaultThumbColorFilter, FilterMode.SrcIn);
		}
	}
}
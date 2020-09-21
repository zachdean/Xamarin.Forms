using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using Xamarin.Forms;

namespace Xamarin.Platform.Handlers
{
	public partial class SliderHandler : AbstractViewHandler<ISlider, SeekBar>
	{
		ColorStateList _defaultProgressTintList;
		ColorStateList _defaultProgressBackgroundTintList;
		PorterDuff.Mode _defaultProgressTintMode;
		PorterDuff.Mode _defaultProgressBackgroundTintMode;
		ColorFilter _defaultThumbColorFilter;

		protected override SeekBar CreateView()
		{
			var slider = new SeekBar(Context);

			slider.SetOnSeekBarChangeListener(new MauiSeekBarListener(VirtualView));

			return slider;
		}

		protected override void SetupDefaults()
		{
			base.SetupDefaults();

			var NativeSlider = TypedNativeView;

			if (NativeSlider == null)
				return;

			if (Build.VERSION.SdkInt > BuildVersionCodes.Kitkat)
			{
				_defaultThumbColorFilter = NativeSlider.Thumb.GetColorFilter();
				_defaultProgressTintMode = NativeSlider.ProgressTintMode;
				_defaultProgressBackgroundTintMode = NativeSlider.ProgressBackgroundTintMode;
				_defaultProgressTintList = NativeSlider.ProgressTintList;
				_defaultProgressBackgroundTintList = NativeSlider.ProgressBackgroundTintList;
			}
		}

		public static void MapMinimum(IViewHandler handler, ISlider slider)
		{
			if (!(handler.NativeView is SeekBar NativeSlider))
				return;

			NativeSlider.Min = (int)slider.Minimum;
		}

		public static void MapMaximum(IViewHandler handler, ISlider slider)
		{
			if (!(handler.NativeView is SeekBar NativeSlider))
				return;

			NativeSlider.Max = (int)slider.Maximum;
		}

		public static void MapValue(IViewHandler handler, ISlider slider)
		{
			if (!(handler.NativeView is SeekBar NativeSlider))
				return;

			NativeSlider.Progress = (int)slider.Value;
		}

		public static void MapMinimumTrackColor(IViewHandler handler, ISlider slider)
		{
			if (!(handler is SliderHandler sliderHandler) || !(handler.NativeView is SeekBar NativeSlider))
				return;

			if (slider.MinimumTrackColor == Forms.Color.Default)
			{
				NativeSlider.ProgressTintList = sliderHandler._defaultProgressTintList;
				NativeSlider.ProgressTintMode = sliderHandler._defaultProgressTintMode;
			}
			else
			{
				NativeSlider.ProgressTintList = ColorStateList.ValueOf(slider.MinimumTrackColor.ToNative());
				NativeSlider.ProgressTintMode = PorterDuff.Mode.SrcIn;
			}
		}

		public static void MapMaximumTrackColor(IViewHandler handler, ISlider slider)
		{
			if (!(handler is SliderHandler sliderHandler) || !(handler.NativeView is SeekBar NativeSlider))
				return;

				if (slider.MaximumTrackColor == Forms.Color.Default)
				{
					NativeSlider.ProgressBackgroundTintList = sliderHandler._defaultProgressBackgroundTintList;
					NativeSlider.ProgressBackgroundTintMode = sliderHandler._defaultProgressBackgroundTintMode;
				}
				else
				{
					NativeSlider.ProgressBackgroundTintList = ColorStateList.ValueOf(slider.MaximumTrackColor.ToNative());
					NativeSlider.ProgressBackgroundTintMode = PorterDuff.Mode.SrcIn;
				}
		}

		public static void MapThumbColor(IViewHandler handler, ISlider slider)
		{
			if (!(handler is SliderHandler sliderHandler) || !(handler.NativeView is SeekBar NativeSlider))
				return;

			NativeSlider.Thumb.SetColorFilter(slider.ThumbColor, sliderHandler._defaultThumbColorFilter, FilterMode.SrcIn);
		}
		
		internal class MauiSeekBarListener : Java.Lang.Object, SeekBar.IOnSeekBarChangeListener
		{
			readonly ISlider _virtualView;

			public MauiSeekBarListener(ISlider virtualView)
			{
				_virtualView = virtualView;
			}

			public void OnProgressChanged(SeekBar seekBar, int progress, bool fromUser)
			{
				_virtualView.Value = progress;
				_virtualView.ValueChanged();
			}

			public void OnStartTrackingTouch(SeekBar seekBar)
			{
				_virtualView.DragStarted();
			}

			public void OnStopTrackingTouch(SeekBar seekBar)
			{
				_virtualView.DragCompleted();
			}
		}
	}
}
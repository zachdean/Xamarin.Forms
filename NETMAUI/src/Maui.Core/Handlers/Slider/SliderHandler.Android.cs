using System.Maui.Controls.Primitives;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Widget;

namespace System.Maui.Platform
{
	public partial class SliderHandler : AbstractViewHandler<ISlider, NativeSlider>
	{
		ColorStateList _defaultProgressTintList;
		ColorStateList _defaultProgressBackgroundTintList;
		PorterDuff.Mode _defaultProgressTintMode;
		PorterDuff.Mode _defaultProgressBackgroundTintMode;
		ColorFilter _defaultThumbColorFilter;

		protected override NativeSlider CreateView()
		{
			var slider = new NativeSlider(Context);

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

		public static void MapPropertyMinimum(IViewHandler Handler, ISlider slider)
		{
			if (!(Handler.NativeView is NativeSlider NativeSlider))
				return;

			NativeSlider.Min = (int)slider.Minimum;
		}

		public static void MapPropertyMaximum(IViewHandler Handler, ISlider slider)
		{
			if (!(Handler.NativeView is NativeSlider NativeSlider))
				return;

			NativeSlider.Max = (int)slider.Maximum;
		}

		public static void MapPropertyValue(IViewHandler Handler, ISlider slider)
		{
			if (!(Handler.NativeView is NativeSlider NativeSlider))
				return;

			NativeSlider.Progress = (int)slider.Value;
		}

		public static void MapPropertyMinimumTrackColor(IViewHandler Handler, ISlider slider)
		{
			if (!(Handler is SliderHandler sliderHandler) || !(Handler.NativeView is NativeSlider NativeSlider))
				return;

			if (slider.MinimumTrackColor == Color.Default)
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

		public static void MapPropertyMaximumTrackColor(IViewHandler Handler, ISlider slider)
		{
			if (!(Handler is SliderHandler sliderHandler) || !(Handler.NativeView is NativeSlider NativeSlider))
				return;

				if (slider.MaximumTrackColor == Color.Default)
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

		public static void MapPropertyThumbColor(IViewHandler Handler, ISlider slider)
		{
			if (!(Handler is SliderHandler sliderHandler) || !(Handler.NativeView is NativeSlider NativeSlider))
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
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Widget;

namespace Xamarin.Platform.Handlers
{
	public partial class SliderHandler : AbstractViewHandler<ISlider, SeekBar>
	{
		static ColorStateList? DefaultProgressTintList { get; set; }
		static ColorStateList? DefaultProgressBackgroundTintList { get; set; }
		static PorterDuff.Mode? DefaultProgressTintMode { get; set; }
		static PorterDuff.Mode? DefaultProgressBackgroundTintMode { get; set; }
		static ColorFilter? DefaultThumbColorFilter { get; set; }

		protected override SeekBar CreateView()
		{
			var slider = new SeekBar(Context);

			slider.SetOnSeekBarChangeListener(new SeekBarChangeListener(VirtualView));

			return slider;
		}

		protected override void SetupDefaults()
		{
			base.SetupDefaults();

			var seekBar = TypedNativeView;

			if (seekBar == null)
				return;

			if (Build.VERSION.SdkInt > BuildVersionCodes.Kitkat)
			{
				DefaultThumbColorFilter = seekBar.Thumb?.GetColorFilter();
				DefaultProgressTintMode = seekBar.ProgressTintMode;
				DefaultProgressBackgroundTintMode = seekBar.ProgressBackgroundTintMode;
				DefaultProgressTintList = seekBar.ProgressTintList;
				DefaultProgressBackgroundTintList = seekBar.ProgressBackgroundTintList;
			}
		}

		public static void MapMinimum(SliderHandler handler, ISlider slider)
		{
			ViewHandler.CheckParameters(handler, slider);

			handler.TypedNativeView?.UpdateMinimum(slider);
		}

		public static void MapMaximum(SliderHandler handler, ISlider slider)
		{
			ViewHandler.CheckParameters(handler, slider);

			handler.TypedNativeView?.UpdateMaximum(slider);
		}

		public static void MapValue(SliderHandler handler, ISlider slider)
		{
			ViewHandler.CheckParameters(handler, slider);

			handler.TypedNativeView?.UpdateValue(slider);
		}

		public static void MapMinimumTrackColor(SliderHandler handler, ISlider slider)
		{
			ViewHandler.CheckParameters(handler, slider);

			handler.TypedNativeView?.UpdateMinimumTrackColor(slider, DefaultProgressBackgroundTintList, DefaultProgressBackgroundTintMode);
		}

		public static void MapMaximumTrackColor(SliderHandler handler, ISlider slider)
		{
			ViewHandler.CheckParameters(handler, slider);

			handler.TypedNativeView?.UpdateMaximumTrackColor(slider, DefaultProgressTintList, DefaultProgressTintMode);
		}

		public static void MapThumbColor(SliderHandler handler, ISlider slider)
		{
			ViewHandler.CheckParameters(handler, slider);

			handler.TypedNativeView?.UpdateThumbColor(slider, DefaultThumbColorFilter);
		}

		internal class SeekBarChangeListener : Java.Lang.Object, SeekBar.IOnSeekBarChangeListener
		{
			readonly ISlider? _virtualView;

			public SeekBarChangeListener(ISlider? virtualView)
			{
				_virtualView = virtualView;
			}

			public void OnProgressChanged(SeekBar? seekBar, int progress, bool fromUser)
			{
				if (_virtualView == null)
					return;

				_virtualView.Value = progress;
			}

			public void OnStartTrackingTouch(SeekBar? seekBar)
			{
				_virtualView?.DragStarted();
			}

			public void OnStopTrackingTouch(SeekBar? seekBar)
			{
				_virtualView?.DragCompleted();
			}
		}
	}
}
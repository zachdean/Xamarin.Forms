using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using Xamarin.Forms;

namespace Xamarin.Platform.Handlers
{
	public partial class SliderHandler : AbstractViewHandler<ISlider, SeekBar>
	{
		int _max, _min;
		ColorStateList? _defaultProgressTintList;
		ColorStateList? _defaultProgressBackgroundTintList;
		PorterDuff.Mode? _defaultProgressTintMode;
		PorterDuff.Mode? _defaultProgressBackgroundTintMode;
		ColorFilter? _defaultThumbColorFilter;

		double Value
		{
			get { return _min + (_max - _min) * ((TypedNativeView != null) ? TypedNativeView.Progress : 0 / 1000.0); }
			set
			{
				if (TypedNativeView != null)
					TypedNativeView.Progress = (int)((value - _min) / (_max - _min) * 1000.0);
			}
		}

		protected override SeekBar CreateView()
		{
			var slider = new SeekBar(Context);

			slider.SetOnSeekBarChangeListener(new SeekBarChangeListener(VirtualView));

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
				_defaultThumbColorFilter = NativeSlider.Thumb?.GetColorFilter();
				_defaultProgressTintMode = NativeSlider.ProgressTintMode;
				_defaultProgressBackgroundTintMode = NativeSlider.ProgressBackgroundTintMode;
				_defaultProgressTintList = NativeSlider.ProgressTintList;
				_defaultProgressBackgroundTintList = NativeSlider.ProgressBackgroundTintList;
			}
		}

		public static void MapMinimum(IViewHandler handler, ISlider slider)
		{
			(handler as SliderHandler)?.UpdateMinimum();
		}

		public static void MapMaximum(IViewHandler handler, ISlider slider)
		{
			(handler as SliderHandler)?.UpdateMaximum();
		}

		public static void MapValue(IViewHandler handler, ISlider slider)
		{
			(handler as SliderHandler)?.UpdateValue();
		}

		public static void MapMinimumTrackColor(IViewHandler handler, ISlider slider)
		{
			(handler as SliderHandler)?.UpdateMinimumTrackColor();
		}

		public static void MapMaximumTrackColor(IViewHandler handler, ISlider slider)
		{
			(handler as SliderHandler)?.UpdateMaximumTrackColor();
		}

		public static void MapThumbColor(IViewHandler handler, ISlider slider)
		{
			(handler as SliderHandler)?.UpdateThumbColor();
		}

		void UpdateMinimum()
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			_min = (int)VirtualView.Minimum;

			TypedNativeView.Min = _min;
		}

		void UpdateMaximum()
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			_max = (int)VirtualView.Maximum;

			TypedNativeView.Max = _max;
		}

		void UpdateValue()
		{
			if (VirtualView == null)
				return;

			if (Value != VirtualView.Value)
				Value = VirtualView.Value;
		}

		void UpdateMinimumTrackColor()
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			if (VirtualView.MinimumTrackColor == Forms.Color.Default)
			{
				TypedNativeView.ProgressTintList = _defaultProgressTintList;
				TypedNativeView.ProgressTintMode = _defaultProgressTintMode;
			}
			else
			{
				TypedNativeView.ProgressTintList = ColorStateList.ValueOf(VirtualView.MinimumTrackColor.ToNative());
				TypedNativeView.ProgressTintMode = PorterDuff.Mode.SrcIn;
			}
		}

		private void UpdateMaximumTrackColor()
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			if (VirtualView.MaximumTrackColor == Forms.Color.Default)
			{
				TypedNativeView.ProgressBackgroundTintList = _defaultProgressBackgroundTintList;
				TypedNativeView.ProgressBackgroundTintMode = _defaultProgressBackgroundTintMode;
			}
			else
			{
				TypedNativeView.ProgressBackgroundTintList = ColorStateList.ValueOf(VirtualView.MaximumTrackColor.ToNative());
				TypedNativeView.ProgressBackgroundTintMode = PorterDuff.Mode.SrcIn;
			}
		}

		void UpdateThumbColor()
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			TypedNativeView.Thumb?.SetColorFilter(VirtualView.ThumbColor, _defaultThumbColorFilter, FilterMode.SrcIn);
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
				_virtualView.ValueChanged();
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
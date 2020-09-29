using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Widget;

namespace Xamarin.Platform.Handlers
{
	public partial class SliderHandler : AbstractViewHandler<ISlider, SeekBar>
	{
		internal ColorStateList? _defaultProgressTintList;
		internal ColorStateList? _defaultProgressBackgroundTintList;
		internal PorterDuff.Mode? _defaultProgressTintMode;
		internal PorterDuff.Mode? _defaultProgressBackgroundTintMode;
		internal ColorFilter? _defaultThumbColorFilter;

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
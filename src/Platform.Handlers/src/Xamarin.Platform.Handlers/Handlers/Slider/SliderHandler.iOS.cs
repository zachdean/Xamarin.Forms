using System;
using UIKit;
using Xamarin.Forms;

namespace Xamarin.Platform.Handlers
{
	public partial class SliderHandler : AbstractViewHandler<ISlider, UISlider>
	{
		UIColor _defaultMinTrackColor;
		UIColor _defaultMaxTrackColor;
		UIColor _defaultThumbColor;

		protected override UISlider CreateView()
		{
			var slider = new UISlider();

			UpdateDefaultColors(slider);

			slider.ValueChanged += OnControlValueChanged;

			slider.AddTarget(OnTouchDownControlEvent, UIControlEvent.TouchDown);
			slider.AddTarget(OnTouchUpControlEvent, UIControlEvent.TouchUpInside | UIControlEvent.TouchUpOutside);

			return slider;
		}

		protected override void DisposeView(UISlider slider)
		{
			slider.ValueChanged -= OnControlValueChanged;
			slider.RemoveTarget(OnTouchDownControlEvent, UIControlEvent.TouchDown);
			slider.RemoveTarget(OnTouchUpControlEvent, UIControlEvent.TouchUpInside | UIControlEvent.TouchUpOutside);

			base.DisposeView(slider);
		}

		public static void MapMinimum(IViewHandler handler, ISlider slider)
		{
			if (!(handler.NativeView is UISlider uISlider))
				return;

			uISlider.MinValue = (float)slider.Minimum;
		}

		public static void MapMaximum(IViewHandler handler, ISlider slider)
		{
			if (!(handler.NativeView is UISlider uISlider))
				return;

			uISlider.MaxValue = (float)slider.Maximum;
		}

		public static void MapValue(IViewHandler handler, ISlider slider)
		{
			if (!(handler.NativeView is UISlider uISlider))
				return;

			if ((float)slider.Value != uISlider.Value)
				uISlider.Value = (float)slider.Value;
		}

		public static void MapMinimumTrackColor(IViewHandler handler, ISlider slider)
		{
			if (!(handler is SliderHandler sliderHandler) || !(handler.NativeView is UISlider uISlider))
				return;

			if (slider.MinimumTrackColor == Color.Default)
				uISlider.MinimumTrackTintColor = sliderHandler._defaultMinTrackColor;
			else
				uISlider.MinimumTrackTintColor = slider.MinimumTrackColor.ToNative();
		}

		public static void MapMaximumTrackColor(IViewHandler handler, ISlider slider)
		{
			if (!(handler is SliderHandler sliderHandler) || !(handler.NativeView is UISlider uISlider))
				return;

			if (slider.MaximumTrackColor == Color.Default)
				uISlider.MaximumTrackTintColor = sliderHandler._defaultMaxTrackColor;
			else
				uISlider.MaximumTrackTintColor = slider.MaximumTrackColor.ToNative();
		}

		public static void MapThumbColor(IViewHandler handler, ISlider slider)
		{
			if (!(handler is SliderHandler sliderHandler) || !(handler.NativeView is UISlider uISlider))
				return;

			if (slider.ThumbColor == Color.Default)
				uISlider.ThumbTintColor = sliderHandler._defaultThumbColor;
			else
				uISlider.ThumbTintColor = slider.ThumbColor.ToNative();
		}

		void UpdateDefaultColors(UISlider uISlider)
		{
			_defaultMinTrackColor = uISlider.MinimumTrackTintColor;
			_defaultMaxTrackColor = uISlider.MaximumTrackTintColor;
			_defaultThumbColor = uISlider.ThumbTintColor;
		}

		void OnControlValueChanged(object sender, EventArgs eventArgs)
		{
			VirtualView.Value = TypedNativeView.Value;
			VirtualView.ValueChanged();
		}

		void OnTouchDownControlEvent(object sender, EventArgs e)
		{
			VirtualView.DragStarted();
		}

		void OnTouchUpControlEvent(object sender, EventArgs e)
		{
			VirtualView.DragCompleted();
		}
	}
}
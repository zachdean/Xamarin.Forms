using System;
using UIKit;
using Xamarin.Forms;

namespace Xamarin.Platform.Handlers
{
	public partial class SliderHandler : AbstractViewHandler<ISlider, UISlider>
	{
		UIColor? _defaultMinTrackColor;
		UIColor? _defaultMaxTrackColor;
		UIColor? _defaultThumbColor;

		protected override UISlider CreateView()
		{
			var slider = new UISlider();

			UpdateDefaultColors(slider);

			slider.ValueChanged += OnControlValueChanged;

			slider.AddTarget(OnTouchDownControlEvent, UIControlEvent.TouchDown);
			slider.AddTarget(OnTouchUpControlEvent, UIControlEvent.TouchUpInside | UIControlEvent.TouchUpOutside);

			return slider;
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

		void UpdateMaximum()
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			TypedNativeView.MaxValue = (float)VirtualView.Maximum;
		}

		void UpdateMinimum()
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			TypedNativeView.MinValue = (float)VirtualView.Minimum;
		}

		void UpdateValue()
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			if ((float)VirtualView.Value != TypedNativeView.Value)
				TypedNativeView.Value = (float)VirtualView.Value;
		}

		void UpdateMinimumTrackColor()
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			if (VirtualView.MinimumTrackColor == Color.Default)
				TypedNativeView.MinimumTrackTintColor = _defaultMinTrackColor;
			else
				TypedNativeView.MinimumTrackTintColor = VirtualView.MinimumTrackColor.ToNative();
		}

		void UpdateMaximumTrackColor()
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			if (VirtualView.MaximumTrackColor == Color.Default)
				TypedNativeView.MaximumTrackTintColor = _defaultMaxTrackColor;
			else
				TypedNativeView.MaximumTrackTintColor = VirtualView.MaximumTrackColor.ToNative();
		}

		void UpdateThumbColor()
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			if (VirtualView.ThumbColor == Color.Default)
				TypedNativeView.ThumbTintColor = _defaultThumbColor;
			else
				TypedNativeView.ThumbTintColor = VirtualView.ThumbColor.ToNative();
		}

		void UpdateDefaultColors(UISlider uISlider)
		{
			if (uISlider == null)
				return;

			_defaultMinTrackColor = uISlider.MinimumTrackTintColor;
			_defaultMaxTrackColor = uISlider.MaximumTrackTintColor;
			_defaultThumbColor = uISlider.ThumbTintColor;
		}

		void OnControlValueChanged(object sender, EventArgs eventArgs)
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			VirtualView.Value = TypedNativeView.Value;
			VirtualView.ValueChanged();
		}

		void OnTouchDownControlEvent(object sender, EventArgs e)
		{
			VirtualView?.DragStarted();
		}

		void OnTouchUpControlEvent(object sender, EventArgs e)
		{
			VirtualView?.DragCompleted();
		}
	}
}
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
			TypedNativeView.MaxValue = (float)VirtualView.Maximum;
		}

		void UpdateMinimum()
		{
			TypedNativeView.MinValue = (float)VirtualView.Minimum;
		}

		void UpdateValue()
		{
			if ((float)VirtualView.Value != TypedNativeView.Value)
				TypedNativeView.Value = (float)VirtualView.Value;
		}

		void UpdateMinimumTrackColor()
		{
			if (VirtualView != null)
			{
				if (VirtualView.MinimumTrackColor == Color.Default)
					TypedNativeView.MinimumTrackTintColor = _defaultMinTrackColor;
				else
					TypedNativeView.MinimumTrackTintColor = VirtualView.MinimumTrackColor.ToNative();
			}
		}

		void UpdateMaximumTrackColor()
		{
			if (VirtualView != null)
			{
				if (VirtualView.MaximumTrackColor == Color.Default)
					TypedNativeView.MaximumTrackTintColor = _defaultMaxTrackColor;
				else
					TypedNativeView.MaximumTrackTintColor = VirtualView.MaximumTrackColor.ToNative();
			}
		}

		void UpdateThumbColor()
		{
			if (VirtualView != null)
			{
				if (VirtualView.ThumbColor == Color.Default)
					TypedNativeView.ThumbTintColor = _defaultThumbColor;
				else
					TypedNativeView.ThumbTintColor = VirtualView.ThumbColor.ToNative();
			}
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
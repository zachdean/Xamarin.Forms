using System;
using UIKit;

namespace Xamarin.Platform.Handlers
{
	public partial class SliderHandler : AbstractViewHandler<ISlider, UISlider>
	{
		internal UIColor? _defaultMinTrackColor;
		internal UIColor? _defaultMaxTrackColor;
		internal UIColor? _defaultThumbColor;

		protected override UISlider CreateView()
		{
			var slider = new UISlider();

			UpdateDefaultColors(slider);

			slider.ValueChanged += OnControlValueChanged;

			slider.AddTarget(OnTouchDownControlEvent, UIControlEvent.TouchDown);
			slider.AddTarget(OnTouchUpControlEvent, UIControlEvent.TouchUpInside | UIControlEvent.TouchUpOutside);

			return slider;
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
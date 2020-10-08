using System;
using UIKit;

namespace Xamarin.Platform.Handlers
{
	public partial class SliderHandler : AbstractViewHandler<ISlider, UISlider>
	{
		static UIColor? DefaultMinTrackColor;
		static UIColor? DefaultMaxTrackColor;
		static UIColor? DefaultThumbColor;

		protected override UISlider CreateView()
		{
			var slider = new UISlider();

			slider.ValueChanged += OnControlValueChanged;

			slider.AddTarget(OnTouchDownControlEvent, UIControlEvent.TouchDown);
			slider.AddTarget(OnTouchUpControlEvent, UIControlEvent.TouchUpInside | UIControlEvent.TouchUpOutside);

			return slider;
		}

		protected override void SetupDefaults()
		{
			base.SetupDefaults();

			var slider = TypedNativeView;

			if (slider == null)
				return;

			DefaultMinTrackColor = slider.MinimumTrackTintColor;
			DefaultMaxTrackColor = slider.MaximumTrackTintColor;
			DefaultThumbColor = slider.ThumbTintColor;
		}

		public static void MapMinimum(IViewHandler handler, ISlider slider)
		{
			ViewHandler.CheckParameters(handler, slider);

			if (handler is SliderHandler sliderHandler)
				sliderHandler.TypedNativeView?.UpdateMinimum(slider);
		}

		public static void MapMaximum(IViewHandler handler, ISlider slider)
		{
			ViewHandler.CheckParameters(handler, slider);

			if (handler is SliderHandler sliderHandler)
				sliderHandler.TypedNativeView?.UpdateMaximum(slider);
		}

		public static void MapValue(IViewHandler handler, ISlider slider)
		{
			ViewHandler.CheckParameters(handler, slider);

			if (handler is SliderHandler sliderHandler)
				sliderHandler.TypedNativeView?.UpdateValue(slider);
		}

		public static void MapMinimumTrackColor(IViewHandler handler, ISlider slider)
		{
			ViewHandler.CheckParameters(handler, slider);

			if (handler is SliderHandler sliderHandler)
				sliderHandler.TypedNativeView?.UpdateMinimumTrackColor(slider, DefaultMinTrackColor);
		}

		public static void MapMaximumTrackColor(IViewHandler handler, ISlider slider)
		{
			ViewHandler.CheckParameters(handler, slider);

			if (handler is SliderHandler sliderHandler)
				sliderHandler.TypedNativeView?.UpdateMaximumTrackColor(slider, DefaultMaxTrackColor);
		}

		public static void MapThumbColor(IViewHandler handler, ISlider slider)
		{
			ViewHandler.CheckParameters(handler, slider);

			if (handler is SliderHandler sliderHandler)
				sliderHandler.TypedNativeView?.UpdateThumbColor(slider, DefaultThumbColor);
		}

		void OnControlValueChanged(object sender, EventArgs eventArgs)
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			VirtualView.Value = TypedNativeView.Value;
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
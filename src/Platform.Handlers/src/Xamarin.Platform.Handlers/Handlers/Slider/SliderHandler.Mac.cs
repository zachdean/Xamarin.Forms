using AppKit;
using Foundation;

namespace Xamarin.Platform.Handlers
{
	public partial class SliderHandler : AbstractViewHandler<ISlider, NativeSlider>
	{
		protected override NativeSlider CreateView()
		{
			var slider = new NativeSlider
			{
				Cell = new NativeSliderCell(),
				Action = new ObjCRuntime.Selector(nameof(ValueChanged))
			};

			return slider;
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

			handler.TypedNativeView?.UpdateMinimumTrackColor(slider);
		}

		public static void MapMaximumTrackColor(SliderHandler handler, ISlider slider)
		{
			ViewHandler.CheckParameters(handler, slider);

			handler.TypedNativeView?.UpdateMaximumTrackColor(slider);
		}

		public static void MapThumbColor(SliderHandler handler, ISlider slider)
		{
			ViewHandler.CheckParameters(handler, slider);

			handler.TypedNativeView?.UpdateThumbColor(slider);
		}

		[Export(nameof(ValueChanged))]
		void ValueChanged()
		{
			if (VirtualView == null || TypedNativeView == null)
				return;

			VirtualView.Value = TypedNativeView.DoubleValue;

			var controlEvent = NSApplication.SharedApplication.CurrentEvent;

			if (controlEvent.Type == NSEventType.LeftMouseDown)
				VirtualView.DragStarted();
			else if (controlEvent.Type == NSEventType.LeftMouseUp)
				VirtualView.DragCompleted();
		}
	}
}
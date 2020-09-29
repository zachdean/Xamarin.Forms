namespace Xamarin.Platform.Handlers
{
	public partial class SliderHandler
	{
		public static PropertyMapper<ISlider, SliderHandler> SliderMapper = new PropertyMapper<ISlider, SliderHandler>(ViewHandler.ViewMapper)
		{
			[nameof(ISlider.Minimum)] = MapMinimum,
			[nameof(ISlider.Maximum)] = MapMaximum,
			[nameof(ISlider.Value)] = MapValue,
			[nameof(ISlider.MinimumTrackColor)] = MapMinimumTrackColor,
			[nameof(ISlider.MaximumTrackColor)] = MapMaximumTrackColor,
			[nameof(ISlider.ThumbColor)] = MapThumbColor
		};

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
			handler.TypedNativeView?.UpdateMinimumTrackColor(handler, slider);
		}

		public static void MapMaximumTrackColor(SliderHandler handler, ISlider slider)
		{
			ViewHandler.CheckParameters(handler, slider);
			handler.TypedNativeView?.UpdateMaximumTrackColor(handler, slider);
		}

		public static void MapThumbColor(SliderHandler handler, ISlider slider)
		{
			ViewHandler.CheckParameters(handler, slider);
			handler.TypedNativeView?.UpdateThumbColor(handler, slider);
		}

		public SliderHandler() : base(SliderMapper)
		{

		}

		public SliderHandler(PropertyMapper mapper) : base(mapper ?? SliderMapper)
		{

		}
	}
}

namespace Xamarin.Platform.Handlers
{
	public partial class SliderHandler
	{
		public static PropertyMapper<ISlider> SliderMapper = new PropertyMapper<ISlider>(ViewHandler.ViewMapper)
		{
			[nameof(ISlider.Minimum)] = MapMinimum,
			[nameof(ISlider.Maximum)] = MapMaximum,
			[nameof(ISlider.Value)] = MapValue,
			[nameof(ISlider.MinimumTrackColor)] = MapMinimumTrackColor,
			[nameof(ISlider.MaximumTrackColor)] = MapMaximumTrackColor,
			[nameof(ISlider.ThumbColor)] = MapThumbColor
		};

		public SliderHandler() : base(SliderMapper)
		{

		}

		public SliderHandler(PropertyMapper mapper) : base(mapper ?? SliderMapper)
		{

		}
	}
}

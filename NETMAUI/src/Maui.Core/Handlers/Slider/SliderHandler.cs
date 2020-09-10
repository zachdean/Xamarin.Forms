namespace System.Maui.Platform
{
	public partial class SliderHandler
	{
		public static PropertyMapper<ISlider> SliderMapper = new PropertyMapper<ISlider>(ViewHandler.ViewMapper)
		{
			[nameof(ISlider.Minimum)] = MapPropertyMinimum,
			[nameof(ISlider.Maximum)] = MapPropertyMaximum,
			[nameof(ISlider.Value)] = MapPropertyValue,
			[nameof(ISlider.MinimumTrackColor)] = MapPropertyMinimumTrackColor,
			[nameof(ISlider.MaximumTrackColor)] = MapPropertyMaximumTrackColor,
			[nameof(ISlider.ThumbColor)] = MapPropertyThumbColor
		};

		public SliderHandler() : base(SliderMapper)
		{

		}

		public SliderHandler(PropertyMapper mapper) : base(mapper ?? SliderMapper)
		{

		}
	}
}

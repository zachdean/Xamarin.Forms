namespace System.Maui.Platform
{
	public partial class SwitchHandler
	{
		public static PropertyMapper<ISwitch> SwitchMapper = new PropertyMapper<ISwitch>(ViewHandler.ViewMapper)
		{
			[nameof(ISwitch.IsToggled)] = MapPropertyIsToggled,
			[nameof(ISwitch.OnColor)] = MapPropertyOnColor,
			[nameof(ISwitch.ThumbColor)] = MapPropertyThumbColor
		};

		public SwitchHandler() : base(SwitchMapper)
		{

		}

		public SwitchHandler(PropertyMapper mapper) : base(mapper ?? SwitchMapper)
		{

		}
	}
}
namespace Xamarin.Platform.Handlers
{
	public partial class ButtonHandler 
	{
		public static PropertyMapper<IButton> ButtonMapper = new PropertyMapper<IButton>(ViewHandler.ViewMapper)
		{
			[nameof(IButton.Text)] = MapPropertyText
		};

		public ButtonHandler() : base(ButtonMapper)
		{

		}
		public ButtonHandler(PropertyMapper mapper) : base(mapper ?? ButtonMapper)
		{

		}
	}
}
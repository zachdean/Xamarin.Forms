namespace Xamarin.Platform.Handlers
{
	public partial class ButtonHandler 
	{
		public static PropertyMapper<IButton> ButtonMapper = new PropertyMapper<IButton>(ViewHandler.ViewMapper)
		{
			[nameof(IButton.Text)] = MapText
		};

		public ButtonHandler() : base(ButtonMapper)
		{

		}
		public ButtonHandler(PropertyMapper mapper) : base(mapper ?? ButtonMapper)
		{

		}
	}
}
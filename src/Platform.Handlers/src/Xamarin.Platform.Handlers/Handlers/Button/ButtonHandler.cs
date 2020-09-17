namespace Xamarin.Platform.Handlers
{
	public partial class ButtonHandler
	{
		public static PropertyMapper<IButton> ButtonMapper = new PropertyMapper<IButton>(ViewHandler.ViewMapper)
		{
			[nameof(IButton.Text)] = MapText,
			Actions = {
				["DemoAction"] = DemoAction
			}
		};

		public ButtonHandler() : base(ButtonMapper)
		{

		}

		public ButtonHandler(PropertyMapper mapper) : base(mapper ?? ButtonMapper)
		{

		}

		static void DemoAction(IViewHandler arg1, IButton arg2)
		{

		}
	}
}
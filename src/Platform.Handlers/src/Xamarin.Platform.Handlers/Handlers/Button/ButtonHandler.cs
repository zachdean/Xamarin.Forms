namespace Xamarin.Platform.Handlers
{
#if __IOS__
	using NativeView = UIKit.UIButton;
#elif MONOANDROID
	using NativeView = AndroidX.AppCompat.Widget.AppCompatButton;
#elif NETSTANDARD
	using NativeView = System.Object;
#endif

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
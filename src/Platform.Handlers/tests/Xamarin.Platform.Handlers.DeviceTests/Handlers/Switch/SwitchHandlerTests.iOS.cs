using UIKit;

namespace Xamarin.Platform.Handlers.DeviceTests
{
	public partial class SwitchHandlerTests
	{
		UISwitch GetNativeSwitch(SwitchHandler switchHandler) =>
			(UISwitch)switchHandler.View;

		bool GetNativeIsChecked(SwitchHandler switchHandler) =>
		  GetNativeSwitch(switchHandler).On;
	}
}
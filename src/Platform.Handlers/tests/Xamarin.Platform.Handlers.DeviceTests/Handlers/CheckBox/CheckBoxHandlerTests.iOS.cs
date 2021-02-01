namespace Xamarin.Platform.Handlers.DeviceTests
{
	public partial class CheckBoxHandlerTests
	{
		NativeCheckBox GetNativeCheckBox(CheckBoxHandler checkBoxHandler) =>
			(NativeCheckBox)checkBoxHandler.View;

		bool GetNativeIsChecked(CheckBoxHandler checkBoxHandler) =>
		  GetNativeCheckBox(checkBoxHandler).IsChecked;
	}
}
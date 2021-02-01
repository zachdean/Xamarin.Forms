using AndroidX.AppCompat.Widget;

namespace Xamarin.Platform.Handlers.DeviceTests
{
	public partial class CheckBoxHandlerTests
	{
		AppCompatCheckBox GetNativeCheckBox(CheckBoxHandler checkBoxHandler) =>
			(AppCompatCheckBox)checkBoxHandler.View;

		bool GetNativeIsChecked(CheckBoxHandler checkBoxHandler) =>
			GetNativeCheckBox(checkBoxHandler).Checked;
	}
}
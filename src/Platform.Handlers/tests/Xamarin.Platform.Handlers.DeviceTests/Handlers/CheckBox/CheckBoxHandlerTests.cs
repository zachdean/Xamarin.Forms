using System.Threading.Tasks;
using Xamarin.Platform.Handlers.DeviceTests.Stubs;
using Xunit;

namespace Xamarin.Platform.Handlers.DeviceTests
{
	public partial class CheckBoxHandlerTests : HandlerTestBase<CheckBoxHandler>
	{
		[Fact]
		public async Task IsCheckedInitializesCorrectly()
		{
			var checkBox = new CheckBoxStub()
			{
				IsChecked = true
			};

			await ValidatePropertyInitValue(checkBox, () => checkBox.IsChecked, GetNativeIsChecked, checkBox.IsChecked);
		}
	}
}
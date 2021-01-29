using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Platform.Handlers.DeviceTests.Stubs;
using Xunit;

namespace Xamarin.Platform.Handlers.DeviceTests
{
	public partial class SwitchHandlerTests : HandlerTestBase<SwitchHandler>
	{
		[Fact]
		public async Task IsToggledInitializesCorrectly()
		{
			var switchStub = new SwitchStub()
			{
				IsToggled = true
			};

			await ValidatePropertyInitValue(switchStub, () => switchStub.IsToggled, GetNativeIsChecked, switchStub.IsToggled);
		}

#if __ANDROID__
		[Fact(Skip = "Currently Fails on Android")]
#else
		[Fact()]
#endif
		public async Task OnColorInitializesCorrectly()
		{
			var switchStub = new SwitchStub()
			{
				IsToggled = true,
				OnColor = Color.Red
			};

			await ValidateOnColor(switchStub, Color.Red);
		}

#if __ANDROID__
		[Fact(Skip = "Currently Fails on Android")]
#else
		[Fact()]
#endif
		public async Task ThumbColorInitializesCorrectly()
		{
			var switchStub = new SwitchStub()
			{
				IsToggled = true,
				ThumbColor = Color.Blue
			};

			await ValidateThumbColor(switchStub, Color.Blue);
		}
	}
}
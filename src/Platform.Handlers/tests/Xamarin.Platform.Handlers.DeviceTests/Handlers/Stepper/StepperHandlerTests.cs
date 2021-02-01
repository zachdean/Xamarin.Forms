using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Platform.Handlers.DeviceTests.Stubs;
using Xunit;

namespace Xamarin.Platform.Handlers.DeviceTests
{
	public partial class StepperHandlerTests : HandlerTestBase<StepperHandler>
	{
		[Fact()]
		public async Task ValueInitializesCorrectly()
		{
			var stepper = new StepperStub()
			{
				Maximum = 100,
				Minimum = 0,
				Value = 50
			};

			await ValidatePropertyInitValue(stepper, () => stepper.Value, GetNativeValue, stepper.Value);
		}

		[Fact]
		public async Task MaximumInitializesCorrectly()
		{
			var stepper = new StepperStub()
			{
				Maximum = 50
			};

			await ValidatePropertyInitValue(stepper, () => stepper.Maximum, GetNativeMaximum, stepper.Maximum);
		}

		[Fact]
		public async Task MinimumInitializesCorrectly()
		{
			var stepper = new StepperStub()
			{
				Minimum = 10
			};

			await ValidatePropertyInitValue(stepper, () => stepper.Minimum, GetNativeMinimum, stepper.Minimum);
		}

		[Fact]
		public async Task BackgroundColorInitializesCorrectly()
		{
			var stepper = new StepperStub()
			{
				BackgroundColor = Color.Red
			};

			await ValidateNativeBackgroundColor(stepper, Color.Red);
		}
	}
}
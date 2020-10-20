using System;
using AppKit;

namespace Xamarin.Platform.Handlers
{
	public partial class StepperHandler : AbstractViewHandler<IStepper, NSStepper>
	{
		protected override NSStepper CreateNativeView()
		{
			return new NSStepper();
		}

		protected override void ConnectHandler(NSStepper nativeView)
		{
			nativeView.Activated += OnStepperActivated;
		}

		protected override void DisconnectHandler(NSStepper nativeView)
		{
			nativeView.Activated -= OnStepperActivated;
		}

		void OnStepperActivated(object sender, EventArgs e)
		{
			if (VirtualView == null || TypedNativeView == null)
				return;

			VirtualView.Value = TypedNativeView.DoubleValue;
		}
	}
}
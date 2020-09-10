using AppKit;

namespace System.Maui.Platform
{
	public partial class StepperHandler : AbstractViewHandler<IStepper, NSStepper>
	{
		protected override NSStepper CreateView()
		{
			var nSStepper = new NSStepper();
			nSStepper.Activated += OnStepperActivated;
			return nSStepper;
		}

		protected override void DisposeView(NSStepper nSStepper)
		{
			nSStepper.Activated -= OnStepperActivated;
			base.DisposeView(nSStepper);
		}

		public static void MapPropertyMinimum(IViewHandler Handler, IStepper slider) => (Handler as StepperHandler)?.UpdateMinimum();
		public static void MapPropertyMaximum(IViewHandler Handler, IStepper slider) => (Handler as StepperHandler)?.UpdateMaximum();
		public static void MapPropertyIncrement(IViewHandler Handler, IStepper slider) => (Handler as StepperHandler)?.UpdateIncrement();
		public static void MapPropertyValue(IViewHandler Handler, IStepper slider) => (Handler as StepperHandler)?.UpdateValue();

		public virtual void UpdateIncrement()
		{
			TypedNativeView.Increment = VirtualView.Increment;
		}

		public virtual void UpdateMaximum()
		{
			TypedNativeView.MaxValue = VirtualView.Maximum;
		}

		public virtual void UpdateMinimum()
		{
			TypedNativeView.MinValue = VirtualView.Minimum;
		}

		public virtual void UpdateValue()
		{
			if (Math.Abs(TypedNativeView.DoubleValue - VirtualView.Value) > 0)
				TypedNativeView.DoubleValue = VirtualView.Value;
		}

		void OnStepperActivated(object sender, EventArgs e) =>
			VirtualView.Value = TypedNativeView.DoubleValue;
	}
}
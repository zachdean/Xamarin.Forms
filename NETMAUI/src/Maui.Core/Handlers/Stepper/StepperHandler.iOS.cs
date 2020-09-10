using System.Drawing;
using UIKit;

namespace System.Maui.Platform
{
	public partial class StepperHandler : AbstractViewHandler<IStepper, UIStepper>
	{
		protected override UIStepper CreateView()
		{
			var uIStepper = new UIStepper(RectangleF.Empty);
			uIStepper.ValueChanged += OnValueChanged;
			return uIStepper;
		}

		protected override void DisposeView(UIStepper uIStepper)
		{
			uIStepper.ValueChanged -= OnValueChanged;
			base.DisposeView(uIStepper);
		}

		public static void MapPropertyMinimum(IViewHandler Handler, IStepper slider) => (Handler as StepperHandler)?.UpdateMinimum();
		public static void MapPropertyMaximum(IViewHandler Handler, IStepper slider) => (Handler as StepperHandler)?.UpdateMaximum();
		public static void MapPropertyIncrement(IViewHandler Handler, IStepper slider) => (Handler as StepperHandler)?.UpdateIncrement();
		public static void MapPropertyValue(IViewHandler Handler, IStepper slider) => (Handler as StepperHandler)?.UpdateValue();

		void OnValueChanged(object sender, EventArgs e)
			=> VirtualView.Value = TypedNativeView.Value;

		public virtual void UpdateIncrement()
		{
			TypedNativeView.StepValue = VirtualView.Increment;
		}

		public virtual void UpdateMaximum()
		{
			TypedNativeView.MaximumValue = VirtualView.Maximum;
		}

		public virtual void UpdateMinimum()
		{
			TypedNativeView.MinimumValue = VirtualView.Minimum;
		}

		public virtual void UpdateValue()
		{
			if (TypedNativeView.Value != VirtualView.Value)
				TypedNativeView.Value = VirtualView.Value;
		}
	}
}
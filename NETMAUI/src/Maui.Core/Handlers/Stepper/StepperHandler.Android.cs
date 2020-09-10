using Android.Widget;
using Android.Views;
using AButton = Android.Widget.Button;

namespace System.Maui.Platform
{
	public partial class StepperHandler : AbstractViewHandler<IStepper, LinearLayout> , IStepperHandler
	{
		AButton _downButton;
		AButton _upButton;

		protected override LinearLayout CreateView()
		{
			var aStepper = new LinearLayout(Context)
			{
				Orientation = Android.Widget.Orientation.Horizontal,
				Focusable = true,
				DescendantFocusability = DescendantFocusability.AfterDescendants
			};

			StepperHandlerManager.CreateStepperButtons(this, out _downButton, out _upButton);
			aStepper.AddView(_downButton, new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.MatchParent));
			aStepper.AddView(_upButton, new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.MatchParent));

			return aStepper;
		}

		public static void MapPropertyMinimum(IViewHandler Handler, IStepper slider) => (Handler as StepperHandler)?.UpdateButtons();
		public static void MapPropertyMaximum(IViewHandler Handler, IStepper slider) => (Handler as StepperHandler)?.UpdateButtons();
		public static void MapPropertyIncrement(IViewHandler Handler, IStepper slider) => (Handler as StepperHandler)?.UpdateButtons();
		public static void MapPropertyValue(IViewHandler Handler, IStepper slider) =>  (Handler as StepperHandler)?.UpdateButtons();

		public static void MapPropertyIsEnabled(IViewHandler Handler, IStepper slider)
		{
			ViewHandler.MapPropertyIsEnabled(Handler, slider);
			(Handler as StepperHandler)?.UpdateButtons();
		}

		public virtual void UpdateButtons()
		{
			StepperHandlerManager.UpdateButtons(this, _downButton, _upButton);
		}

		IStepper IStepperHandler.Element => VirtualView;

		AButton IStepperHandler.UpButton => _upButton;

		AButton IStepperHandler.DownButton => _downButton;

		AButton IStepperHandler.CreateButton()
		{
			var button = new AButton(Context);

			return button;
		}
	}
}
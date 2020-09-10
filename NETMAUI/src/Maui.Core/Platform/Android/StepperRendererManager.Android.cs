using System.ComponentModel;
using Android.Views;
using AButton = Android.Widget.Button;
using AView = Android.Views.View;

namespace System.Maui.Platform
{
	public interface IStepperHandler
	{
		AButton UpButton { get; }

		AButton DownButton { get; }

		AButton CreateButton();

		IStepper Element { get; }
	}

	class StepperHandlerHolder : Java.Lang.Object
	{
		internal IStepperHandler _Handler;
		public StepperHandlerHolder(IStepperHandler Handler)
		{
			_Handler = Handler;
		}
	}

	public static class StepperHandlerManager
	{
		public static void CreateStepperButtons<TButton>(IStepperHandler Handler, out TButton downButton, out TButton upButton)
			where TButton : AButton
		{
			downButton = (TButton)Handler.CreateButton();
			downButton.Focusable = true;

			upButton = (TButton)Handler.CreateButton();
			upButton.Focusable = true;

			downButton.Gravity = GravityFlags.Center;
			downButton.Tag = new StepperHandlerHolder(Handler);
			downButton.SetOnClickListener(StepperListener.Instance);
			upButton.Gravity = GravityFlags.Center;
			upButton.Tag = new StepperHandlerHolder(Handler);
			upButton.SetOnClickListener(StepperListener.Instance);

			// IMPORTANT:
			// Do not be decieved. These are NOT the same characters. Neither are a "minus" either.
			// The Text is a visually pleasing "minus", and the description is the phonetically correct "minus".
			// The little key on your keyboard is a dash/hyphen.
			downButton.Text = "－";
			downButton.ContentDescription = "−";

			// IMPORTANT:
			// Do not be decieved. These are NOT the same characters.
			// The Text is a visually pleasing "plus", and the description is the phonetically correct "plus"
			// (which, unlike the minus, IS found on your keyboard).
			upButton.Text = "＋";
			upButton.ContentDescription = "+";

			downButton.NextFocusForwardId = upButton.Id;
		}

		public static void UpdateButtons<TButton>(IStepperHandler Handler, TButton downButton, TButton upButton, PropertyChangedEventArgs e = null)
			where TButton : AButton
		{
			if (!(Handler?.Element is IStepper stepper))
				return;
			// NOTE: a value of `null` means that we are forcing an update
			downButton.Enabled = stepper.IsEnabled && stepper.Value > stepper.Minimum;
			upButton.Enabled = stepper.IsEnabled && stepper.Value < stepper.Maximum;

		}

		class StepperListener : Java.Lang.Object, AView.IOnClickListener
		{
			public static readonly StepperListener Instance = new StepperListener();

			public void OnClick(AView v)
			{
				if (!(v?.Tag is StepperHandlerHolder HandlerHolder))
					return;

				if (!(HandlerHolder._Handler?.Element is IStepper stepper))
					return;

				var increment = stepper.Increment;
				if (v == HandlerHolder._Handler.DownButton)
					increment = -increment;

				HandlerHolder._Handler.Element.Value = stepper.Value + increment;
				UpdateButtons(HandlerHolder._Handler, HandlerHolder._Handler.DownButton, HandlerHolder._Handler.UpButton);
			}
		}
	}
}

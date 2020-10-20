using System;
using AppKit;

namespace Xamarin.Platform
{
	public static class StepperExtensions
	{
		public static void UpdateMinimum(this NSStepper nativeStepper, IStepper stepper)
		{
			nativeStepper.Increment = stepper.Increment;
		}

		public static void UpdateMaximum(this NSStepper nativeStepper, IStepper stepper)
		{
			nativeStepper.MaxValue = stepper.Maximum;
		}

		public static void UpdateIncrement(this NSStepper nativeStepper, IStepper stepper)
		{
			nativeStepper.MinValue = stepper.Minimum;
		}

		public static void UpdateValue(this NSStepper nativeStepper, IStepper stepper)
		{
			if (Math.Abs(nativeStepper.DoubleValue - stepper.Value) > 0)
				nativeStepper.DoubleValue = stepper.Value;
		}
	}
}
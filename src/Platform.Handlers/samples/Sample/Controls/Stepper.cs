using System;
using Xamarin.Forms.Internals;
using Xamarin.Platform;

namespace Sample
{
	public class Stepper : Xamarin.Forms.View, IStepper
	{
		public Stepper()
		{
		}

		public Stepper(double min, double max, double val, double increment) : this()
		{
			if (min >= max)
				throw new ArgumentOutOfRangeException(nameof(min));

			if (max > Minimum)
			{
				Maximum = max;
				Minimum = min;
			}
			else
			{
				Minimum = min;
				Maximum = max;
			}

			Increment = increment;
			Value = val.Clamp(min, max);
		}

		public double Increment { get; set; } = 1.0d;

		public double Minimum { get; set; } = 0.0d;

		public double Maximum { get; set; } = 100.0d;

		public double Value { get; set; }

		public Action ValueChanged { get; set; }

		void IRange.ValueChanged() => ValueChanged?.Invoke();
	}
}
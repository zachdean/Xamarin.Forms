namespace System.Maui.Controls
{
	public class Stepper : View, IStepper
	{
		public Stepper()
		{
		}

		public double Increment { get; set; } = 1.0d;

		public double Minimum { get; set; } = 0.0d;

		public double Maximum { get; set; } = 100.0d;

		public double Value { get; set; }
	}
}
using Xamarin.Platform;

namespace Xamarin.Forms
{
	public partial class Stepper : IStepper
	{
		static double OldValue;

		void IRange.ValueChanged()
		{
			ValueChanged.Invoke(this, new ValueChangedEventArgs(OldValue, Value));
		}
	}
}
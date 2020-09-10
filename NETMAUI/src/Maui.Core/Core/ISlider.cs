using System.Windows.Input;

namespace System.Maui
{
	public interface ISlider : IView
	{
		double Minimum { get; }
		double Maximum { get; }
		double Value { get; set; }

		Color MinimumTrackColor { get; }
		Color MaximumTrackColor { get; }
		Color ThumbColor { get; }

		ICommand DragStartedCommand { get; }
		ICommand DragCompletedCommand { get; }

		void ValueChanged();
		void DragStarted();
		void DragCompleted();
	}
}
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Xamarin.Forms.Platform.UWP;

namespace Xamarin.Forms.Platform.UWP
{
	public class SwipeViewRenderer : ViewRenderer<SwipeView, FrameworkElement>
	{
		private bool _isDisposed;

		protected override void OnElementChanged(ElementChangedEventArgs<SwipeView> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement != null)
			{
				if (Control == null)
				{
					ManipulationMode = ManipulationModes.All;

					ManipulationStarted += OnManipulationStarted;
					ManipulationDelta += OnManipulationDelta;
					ManipulationCompleted += OnManipulationCompleted;
				}
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (_isDisposed)
			{
				return;
			}

			if (disposing)
			{
				ManipulationStarted -= OnManipulationStarted;
				ManipulationDelta -= OnManipulationDelta;
				ManipulationCompleted -= OnManipulationCompleted;
			}

			_isDisposed = true;

			base.Dispose(disposing);
		}

		private void OnManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
		{
			if (Element.HandleTouchInteractions(GestureStatus.Started, new Point(e.Position.X, e.Position.Y)))
			{
				e.Complete();
				e.Handled = false;
			}
		}

		private void OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
		{
			double x = e.Position.X;
			double y = e.Position.Y;

			if (!Element.HandleTouchInteractions(GestureStatus.Running, new Point(x, y)))
			{
				e.Handled = true;
			}
		}

		private void OnManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
		{
			double x = e.Position.X;
			double y = e.Position.Y;
			bool handled = Element.HandleTouchInteractions(GestureStatus.Completed, new Point(x, y));
			e.Handled = handled;
		}
	}
}
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

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

					PointerPressed += OnPointerPressed;
					PointerReleased += OnPointerReleased;
					PointerCanceled += OnPointerCanceled;
					ManipulationDelta += OnManipulationDelta;
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
				PointerPressed -= OnPointerPressed;
				PointerReleased -= OnPointerReleased;
				PointerCanceled -= OnPointerCanceled;
				ManipulationDelta -= OnManipulationDelta;
			}

			_isDisposed = true;

			base.Dispose(disposing);
		}
		private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
		{
			var x = e.GetCurrentPoint(this).Position.X;
			var y = e.GetCurrentPoint(this).Position.Y;

			if (Element.HandleTouchInteractions(GestureStatus.Started, new Point(x, y)))
			{
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

		private void OnPointerReleased(object sender, PointerRoutedEventArgs e)
		{
			var x = e.GetCurrentPoint(this).Position.X;
			var y = e.GetCurrentPoint(this).Position.Y;
			bool handled = Element.HandleTouchInteractions(GestureStatus.Completed, new Point(x, y));
			e.Handled = handled;
		}

		private void OnPointerCanceled(object sender, PointerRoutedEventArgs e)
		{
			var x = e.GetCurrentPoint(this).Position.X;
			var y = e.GetCurrentPoint(this).Position.Y;
			bool handled = Element.HandleTouchInteractions(GestureStatus.Canceled, new Point(x, y));
			e.Handled = handled;
		}
	}
}
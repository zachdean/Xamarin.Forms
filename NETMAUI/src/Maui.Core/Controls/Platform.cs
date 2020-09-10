using System.Maui.Platform;
using System.Maui.Shapes;

namespace System.Maui.Controls
{
	public class Platform
	{
		static bool HasInit;

		public static void Init()
		{
			if (HasInit)
				return;

			HasInit = true;

			Registrar.Handlers.Register<ActivityIndicator, ActivityIndicatorHandler>();
			Registrar.Handlers.Register<Button, ButtonHandler>();
			Registrar.Handlers.Register<Ellipse, EllipseHandler>();
			Registrar.Handlers.Register<Entry, EntryHandler>();
			Registrar.Handlers.Register<Label, LabelHandler>();
			Registrar.Handlers.Register<Line, LineHandler>();
			Registrar.Handlers.Register<Path, PathHandler>();
			Registrar.Handlers.Register<Polygon, PolygonHandler>();
			Registrar.Handlers.Register<Polyline, PolylineHandler>();
			Registrar.Handlers.Register<ProgressBar, ProgressBarHandler>();
			Registrar.Handlers.Register<Rectangle, RectangleHandler>();
			Registrar.Handlers.Register<Slider, SliderHandler>();
			Registrar.Handlers.Register<Stepper, StepperHandler>();
			Registrar.Handlers.Register<Switch, SwitchHandler>();
		}
	}
}
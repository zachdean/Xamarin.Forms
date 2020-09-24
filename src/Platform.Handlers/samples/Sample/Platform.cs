using System.Collections;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Platform;
using Xamarin.Platform.Handlers;
using RegistrarHandlers = Xamarin.Platform.Registrar;

namespace Sample
{
	public class Platform
	{
		static bool HasInit;

		public static void Init()
		{
			if (HasInit)
				return;

			HasInit = true;

			RegistrarHandlers.Handlers.Register<Button, ButtonHandler>();
			RegistrarHandlers.Handlers.Register<Slider, SliderHandler>();
			RegistrarHandlers.Handlers.Register<Xamarin.Platform.VerticalStackLayout, LayoutHandler>();
			RegistrarHandlers.Handlers.Register<Xamarin.Platform.HorizontalStackLayout, LayoutHandler>();
			RegistrarHandlers.Handlers.Register<Xamarin.Forms.FlexLayout, LayoutHandler>();
			RegistrarHandlers.Handlers.Register<Xamarin.Forms.StackLayout, LayoutHandler>();
			//RegistrarHandlers.Handlers.Register<Entry, EntryHandler>();
			RegistrarHandlers.Handlers.Register<Label, LabelHandler>();
			RegistrarHandlers.Handlers.Register<Ellipse, EllipseHandler>();
			RegistrarHandlers.Handlers.Register<Line, LineHandler>();
			RegistrarHandlers.Handlers.Register<Polygon, PolygonHandler>();
			RegistrarHandlers.Handlers.Register<Polyline, PolylineHandler>();
			RegistrarHandlers.Handlers.Register<Rectangle, RectangleHandler>();
		}
	}
}
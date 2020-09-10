using System.Maui;
using System.Maui.Controls;
using System.Maui.Shapes;

namespace Sample
{
	public class MyApp : App
	{
		public IView[] TestActivityIndicator = new IView[]
		{
			new Label { Text = "ActivityIndicator Gallery", FontSize = 24 }, 
			new ActivityIndicator { BackgroundColor = Color.Orange },
			new ActivityIndicator { Color = Color.Red }
		};

		public IView[] TestButton = new IView[]
		{
			new Label { Text = "Button Gallery", FontSize = 24 },
			new Button { Text = "Button", BackgroundColor = Color.Blue },
			new Button { Text = "BackgroundColor", BackgroundColor = Color.Red },
			new Button { Text = "CornerRadius", BackgroundColor = Color.Pink, CornerRadius = 12 },
			new Button { Text = "BorderColor", BackgroundColor = Color.Pink, BorderColor = Color.Purple, BorderWidth = 1 },
			new Button { Text = "BorderWidth", BackgroundColor = Color.Pink, BorderColor = Color.Purple, BorderWidth = 4 },
			new Button { Text = "TextColor", BackgroundColor = Color.Red, TextColor = Color.Yellow },
			new Button { Text = "FontAttributes", BackgroundColor = Color.Blue, FontAttributes =  FontAttributes.Bold },
			new Button { Text = "FontSize", BackgroundColor = Color.Blue, FontSize = 24 },
			new Button { Text = "CharacterSpacing", BackgroundColor = Color.Blue, CharacterSpacing = 12 }
		};

		public IView[] TestEllipse = new IView[]
		{
			new Label { Text = "Ellipse Gallery", FontSize = 24 },
			new Ellipse { Fill = Color.Red, Frame = new Rect(0, 0, 100, 50) },
			new Ellipse { Fill = Color.Green, Stroke = Color.Black, Frame = new Rect(0, 0, 100, 50) },
			new Ellipse { Fill = Color.Purple, Stroke = Color.Black, StrokeThickness = 4, Frame = new Rect(0, 0, 100, 50) },
			new Ellipse { Fill = Color.Orange, Stroke = Color.Black, StrokeThickness = 4, StrokeDashArray = new DoubleCollection { 1, 1 }, Frame = new Rect(0, 0, 100, 50) }
		};

		public IView[] TestEntry = new IView[]
		{
			new Label { Text = "Entry Gallery", FontSize = 24 },
			new Entry { BackgroundColor = Color.Blue },
			new Entry { Text = "Text" },
			new Entry { Text = "TextTransform (Uppercase)", TextTransform = TextTransform.Uppercase },
			new Entry { Text = "TextColor", TextColor = Color.OrangeRed },
			new Entry { Placeholder = "Placeholder" },
			new Entry { Placeholder = "PlaceholderColor", PlaceholderColor = Color.AliceBlue },
			new Entry { Text = "FontAttributes", FontAttributes = FontAttributes.Bold },
			new Entry { Text = "FontSize", FontSize = 24 },
			new Entry { Text = "CharacterSpacing", BackgroundColor = Color.Blue, CharacterSpacing = 12 },
			new Entry { Text = "HorizontalTextAlignment", HorizontalTextAlignment = TextAlignment.End },
		};

		public IView[] TestLabel = new IView[]
		{
			new Label { Text = "Label Gallery", FontSize = 24 },
			new Label { Text = "Label" },
			new Label { Text = "BackgroundColor", BackgroundColor = Color.Orange },
			new Label { Text = "TextColor", TextColor = Color.Red },
			new Label { Text = "FontAttributes", FontAttributes =  FontAttributes.Bold },
			new Label { Text = "FontSize", FontSize = 24 },
			new Label { Text = "LineHeight", LineHeight = 12 },
			new Label { Text = "CharacterSpacing", CharacterSpacing = 12 },
			new Label { Text = "TextDecorations", TextDecorations = TextDecorations.Underline },
			new Label { Text = "TextTransform (Uppercase)", TextTransform = TextTransform.Uppercase },
			new Label { Text = "HorizontalTextAlignment", HorizontalTextAlignment = TextAlignment.End },
			new Label
			{
				Text = "This is <strong style=\"color:red\">HTML</strong> text.",
				TextType = TextType.Html
			},
			CreateFormattedLabel()
		};

		public IView[] TestLine = new IView[]
		{
			new Label { Text = "Line Gallery", FontSize = 24 },
			new Line { X1 = 0, Y1 = 0, X2 = 70, Y2 = 50, Stroke = Color.Black, StrokeThickness = 4, Frame = new Rect(0, 0, 100, 50) },
			new Line { X1 = 100, Y1 = 0, X2 = 0, Y2 = 50, Fill = Color.Orange, Stroke = Color.Black, StrokeThickness = 4, StrokeDashArray = new DoubleCollection { 1, 1 }, Frame = new Rect(0, 0, 100, 50) }
		};

		public IView[] TestPath = new IView[]
		{
			new Label { Text = "Path Gallery", FontSize = 24 },
			new Path { Data = CreateGeometry(), Stroke = Color.Black, StrokeThickness = 4, Frame = new Rect(0, 0, 100, 50) }
		};

		public IView[] TestPolygon = new IView[]
		{
			new Label { Text = "Polygon Gallery", FontSize = 24 },
			new Polygon { Points = new  PointCollection { new Point(0, 0), new Point(50, 20), new Point(70, 10), new Point(100, 50) }, Stroke = Color.Black, StrokeThickness = 4, Frame = new Rect(0, 0, 100, 50) },
			new Polygon { Points = new  PointCollection { new Point(0, 0), new Point(50, 20), new Point(70, 10), new Point(100, 50) }, Fill = Color.Orange, Stroke = Color.Black, StrokeThickness = 4, StrokeDashArray = new DoubleCollection { 1, 1 }, Frame = new Rect(0, 0, 100, 50) }
		};

		public IView[] TestPolyline = new IView[]
		{
			new Label { Text = "Polyline Gallery", FontSize = 24 },
			new Polyline { Points = new  PointCollection { new Point(0, 0), new Point(50, 20), new Point(70, 10), new Point(100, 50) }, Stroke = Color.Black, StrokeThickness = 4, Frame = new Rect(0, 0, 100, 50) },
			new Polyline { Points = new  PointCollection { new Point(0, 0), new Point(50, 20), new Point(70, 10), new Point(100, 50) }, Fill = Color.Orange, Stroke = Color.Black, StrokeThickness = 4, StrokeDashArray = new DoubleCollection { 1, 1 }, Frame = new Rect(0, 0, 100, 50) }
		};

		public IView[] TestProgressBar = new IView[]
		{
			new Label { Text = "ProgressBar Gallery", FontSize = 24 },
			new ProgressBar { BackgroundColor = Color.Orange },
			new ProgressBar { Progress = 0.5 },
			new ProgressBar { Progress = 0.75, ProgressColor = Color.Green }
		};

		public IView[] TestRectangle = new IView[]
		{
			new Label { Text = "Rectangle Gallery", FontSize = 24 },
			new Rectangle { Fill = Color.Red, Frame = new Rect(0, 0, 100, 50) },
			new Rectangle { Fill = Color.Green, Stroke = Color.Black, Frame = new Rect(0, 0, 100, 50) },
			new Rectangle { Fill = Color.Purple, Stroke = Color.Black, StrokeThickness = 4, Frame = new Rect(0, 0, 100, 50) },
			new Rectangle { Fill = Color.Orange, Stroke = Color.Black, StrokeThickness = 4, StrokeDashArray = new DoubleCollection { 1, 1 }, Frame = new Rect(0, 0, 100, 50) },
			new Rectangle { RadiusX = 12, RadiusY = 12, Fill = Color.Red, Stroke = Color.Black, StrokeThickness = 4, StrokeDashArray = new DoubleCollection { 1, 1 }, Frame = new Rect(0, 0, 100, 50) }
		};

		public IView[] TestSlider = new IView[]
		{
			new Label { Text = "Slider Gallery", FontSize = 24 },
			new Slider (),
			new Slider { BackgroundColor = Color.Orange },
			new Slider { Minimum = 0, Maximum = 100 },
			new Slider { Minimum = 0, Maximum = 100, Value = 75 },
			new Slider { Minimum = 0, MinimumTrackColor = Color.Blue, Maximum = 100, MaximumTrackColor = Color.Red, Value = 45 },
			new Slider { Minimum = 0, MinimumTrackColor = Color.Blue, Maximum = 100, MaximumTrackColor = Color.Red, Value = 60, ThumbColor = Color.YellowGreen }
		};

		public IView[] TestStepper = new IView[]
		{
			new Label { Text = "Stepper Gallery", FontSize = 24 },
			new Stepper (),
			new Stepper { BackgroundColor = Color.Orange },
			new Stepper { Minimum = 0, Maximum = 100 },
			new Stepper { Minimum = 0, Maximum = 100, Value = 75 }
		};

		public IView[] TestSwitch = new IView[]
		{
			new Label { Text = "Switch Gallery", FontSize = 24 },
			new Switch (),
			new Switch { BackgroundColor = Color.Orange },
			new Switch { IsToggled = true },
			new Switch { OnColor = Color.Purple, IsToggled = true },
			new Switch { ThumbColor = Color.DarkOliveGreen, IsToggled = true }
		};

		static Label CreateFormattedLabel()
		{
			FormattedString formattedString = new FormattedString();

			formattedString.Spans.Add(new Span
			{
				Text = "Lorem ipsum"
			});

			formattedString.Spans.Add(new Span
			{
				Text = "dolor sit amet."
			});

			Label label = new Label
			{
				FormattedText = formattedString
			};

			return label;
		}

		static Geometry CreateGeometry()
        {
            PathFigure pathFigure = new PathFigure
            {
                StartPoint = new Point(10, 10)
            };

            LineSegment lineSegment = new LineSegment
            {
                Point = new Point(100, 50)
            };

            PathSegmentCollection pathSegmentCollection = new PathSegmentCollection
            {
                lineSegment
            };

            pathFigure.Segments = pathSegmentCollection;

            PathFigureCollection pathFigureCollection = new PathFigureCollection
            {
                pathFigure
            };

            PathGeometry pathGeometry = new PathGeometry
            {
                Figures = pathFigureCollection
            };

            return pathGeometry;
		}
	}
}
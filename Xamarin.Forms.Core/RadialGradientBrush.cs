namespace Xamarin.Forms
{
	public class RadialGradientBrush : GradientBrush
	{
		public static readonly BindableProperty CenterProperty = BindableProperty.Create(
			nameof(Center), typeof(Point), typeof(RadialGradientBrush), default(Point));

		public Point Center
		{
			get => (Point)GetValue(CenterProperty);
			set => SetValue(CenterProperty, value);
		}

		public static readonly BindableProperty GradientOriginProperty = BindableProperty.Create(
			nameof(GradientOrigin), typeof(Point), typeof(RadialGradientBrush), default(Point));

		public Point GradientOrigin
		{
			get => (Point)GetValue(GradientOriginProperty);
			set => SetValue(GradientOriginProperty, value);
		}

		public static readonly BindableProperty RadiusXProperty = BindableProperty.Create(
			nameof(RadiusX), typeof(double), typeof(RadialGradientBrush), default(Point));

		public double RadiusX
		{
			get => (double)GetValue(RadiusXProperty);
			set => SetValue(RadiusXProperty, value);
		}

		public static readonly BindableProperty RadiusYProperty = BindableProperty.Create(
			nameof(RadiusY), typeof(double), typeof(RadialGradientBrush), default(Point));

		public double RadiusY
		{
			get => (double)GetValue(RadiusYProperty);
			set => SetValue(RadiusYProperty, value);
		}
	}
}
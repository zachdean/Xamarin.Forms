using System.Collections.Generic;

namespace Xamarin.Forms
{
	[ContentProperty(nameof(GradientStops))]
	public class GradientBrush : Brush
	{
		public static readonly BindableProperty GradientStopsProperty = BindableProperty.Create(
			nameof(GradientStops), typeof(IList<GradientStop>), typeof(GradientBrush), null);

		public IList<GradientStop> GradientStops
		{
			get => (IList<GradientStop>)GetValue(GradientStopsProperty);
			set => SetValue(GradientStopsProperty, value);
		}
	}
}

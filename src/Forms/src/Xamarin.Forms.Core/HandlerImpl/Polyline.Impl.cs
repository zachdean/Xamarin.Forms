using Xamarin.Platform;

namespace Xamarin.Forms.Shapes
{
	public partial class Polyline : IPolyline
	{
		Color IShape.Fill { get; }

		Color IShape.Stroke { get; }
	}
}

using Xamarin.Platform;

namespace Xamarin.Forms.Shapes
{
	public partial class Polygon : IPolygon
	{
		Color IShape.Fill { get; }

		Color IShape.Stroke { get; }
	}
}
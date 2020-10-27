using Xamarin.Platform;

namespace Xamarin.Forms.Shapes
{
	public partial class Ellipse : IEllipse
	{
		Color IShape.Fill { get; }

		Color IShape.Stroke { get; }
	}
}
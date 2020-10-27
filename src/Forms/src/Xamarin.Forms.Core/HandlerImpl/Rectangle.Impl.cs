using Xamarin.Platform;

namespace Xamarin.Forms.Shapes
{
	public partial class Rectangle : IRectangle
	{
		Color IShape.Fill { get; }

		Color IShape.Stroke { get; }
	}
}
using Xamarin.Platform;

namespace Xamarin.Forms.Shapes
{
	public partial class Path : IPath
	{
		Color IShape.Fill { get; }

		Color IShape.Stroke { get; }
	}
}
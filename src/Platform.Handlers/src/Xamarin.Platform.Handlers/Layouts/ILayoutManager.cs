using Xamarin.Forms;

namespace Xamarin.Platform.Layouts
{
	public interface ILayoutManager
	{
		Size Measure(double widthConstraint, double heightConstraint);
		void ArrangeChildren(Rectangle childBounds);
	}
}

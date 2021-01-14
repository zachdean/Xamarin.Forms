using System.Collections.Generic;
using NSubstitute;
using Xamarin.Forms;

namespace Xamarin.Platform.Handlers.UnitTests.Layouts
{
	public abstract class StackLayoutManagerTests
	{
		protected IStackLayout CreateTestLayout()
		{
			var stack = Substitute.For<IStackLayout>();
			stack.Height.Returns(-1);
			stack.Width.Returns(-1);
			stack.Spacing.Returns(0);

			return stack;
		}

		protected IStackLayout BuildStack(int viewCount, double viewWidth, double viewHeight)
		{
			var stack = CreateTestLayout();

			var children = new List<IView>();

			for (int n = 0; n < viewCount; n++)
			{
				var view = LayoutTestHelpers.CreateTestView(new Size(viewWidth, viewHeight));
				children.Add(view);
			}

			stack.Children.Returns(children.AsReadOnly());

			return stack;
		}
	}
}

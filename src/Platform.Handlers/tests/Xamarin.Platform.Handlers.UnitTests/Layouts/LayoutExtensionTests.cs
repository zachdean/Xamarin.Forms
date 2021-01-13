using NUnit.Framework;
using Xamarin.Platform.Handlers.Tests;
using NSubstitute;
using Xamarin.Forms;
using Xamarin.Platform.Layouts;

namespace Xamarin.Platform.Handlers.UnitTests.Layouts
{
	[TestFixture(Category = TestCategory.Layout)]
	public class LayoutExtensionTests
	{
		[Test]
		public void FrameExcludesMargin()
		{
			var element = Substitute.For<IFrameworkElement>();
			var margin = new Thickness(20);
			element.Margin.Returns(margin);

			var bounds = new Rectangle(0, 0, 100, 100);
			var frame = element.ComputeFrame(bounds);

			// With a margin of 20 all the way around, we expect the actual frame
			// to be 60x60, with an x,y position of 20,20

			Assert.That(frame.Top, Is.EqualTo(20));
			Assert.That(frame.Left, Is.EqualTo(20));
			Assert.That(frame.Width, Is.EqualTo(60));
			Assert.That(frame.Height, Is.EqualTo(60));
		}

		[Test]
		public void FrameSizeGoesToZeroWhenMarginsExceedBounds()
		{
			var element = Substitute.For<IFrameworkElement>();
			var margin = new Thickness(200);
			element.Margin.Returns(margin);

			var bounds = new Rectangle(0, 0, 100, 100);
			var frame = element.ComputeFrame(bounds);

			// The margin is simply too large for the bounds; since negative widths/heights on a frame don't make sense,
			// we expect them to collapse to zero

			Assert.That(frame.Width, Is.EqualTo(0));
			Assert.That(frame.Height, Is.EqualTo(0));
		}

		[Test]
		public void DesiredSizeIncludesMargin()
		{
			var widthConstraint = 400;
			var heightConstraint = 655;

			var handler = Substitute.For<IViewHandler>();
			var element = Substitute.For<IFrameworkElement>();
			var margin = new Thickness(20);

			// Our "native" control will request a size of (100,50) when we call GetDesiredSize
			handler.GetDesiredSize(Arg.Any<double>(), Arg.Any<double>()).Returns(new Size(100, 50));
			element.Handler.Returns(handler);
			element.Margin.Returns(margin);

			var desiredSize = element.ComputeDesiredSize(widthConstraint, heightConstraint);

			// Because the actual ("native") measurement comes back with (100,50)
			// and the margin on the IFrameworkElement is 20, the expected width is (100 + 20 + 20) = 140
			// and the expected height is (50 + 20 + 20) = 90

			Assert.That(desiredSize.Width, Is.EqualTo(140));
			Assert.That(desiredSize.Height, Is.EqualTo(90));
		}

		[Test]
		public void MarginsAccountForFlowDirectionRTL()
		{
			var element = Substitute.For<IFrameworkElement>();
			element.FlowDirection.Returns(FlowDirection.RightToLeft);
			var margin = new Thickness(10, 0, 15, 0);
			element.Margin.Returns(margin);

			var bounds = new Rectangle(0, 0, 100, 100);
			var frame = element.ComputeFrame(bounds);

			// The top and bottom margins are zero, so we expect the Frame to have the full height of 100
			Assert.That(frame.Top, Is.EqualTo(0));
			Assert.That(frame.Height, Is.EqualTo(100));

			// The left and right margins together are 25, so we expect the Frame to have a width of 75
			Assert.That(frame.Width, Is.EqualTo(75));

			// The left and right margins should be swapped (because of RTL) to we expect the Frame location
			// to have a Left value of 15 (the "right side" margin)
			Assert.That(frame.Left, Is.EqualTo(15));
		}
	}
}

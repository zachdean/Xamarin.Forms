using System.Maui.Controls.Primitives;

#if __MOBILE__
using NativeColor = UIKit.UIColor;
#else
using NativeColor = AppKit.NSColor;
#endif

namespace System.Maui.Platform
{
	public partial class AbstractViewHandler<TVirtualView, TNativeView> : INativeViewHandler
	{
		public void SetFrame(Rect rect)
		{
			View.Frame = rect.ToCGRect();
		}

		public virtual SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			var size = TypedNativeView.SizeThatFits(new CoreGraphics.CGSize((float)widthConstraint, (float)heightConstraint));

			var request = new Size(
				size.Width == float.PositiveInfinity ? double.PositiveInfinity : size.Width,
				size.Height == float.PositiveInfinity ? double.PositiveInfinity : size.Height);

			return new SizeRequest(request);
		}

		void SetupContainer()
		{
			var oldParent = TypedNativeView.Superview;
			ContainerView ??= new ContainerView();

			if (oldParent == ContainerView)
				return;

			ContainerView.MainView = TypedNativeView;
		}

		void RemoveContainer()
		{

		}
	}
}
using Xamarin.Forms;

namespace Xamarin.Platform.Handlers
{
	public partial class EllipseHandler : AbstractViewHandler<IEllipse, NativeEllipse>
	{
		protected override NativeEllipse CreateNativeView() => new NativeEllipse(Context);

		public override Size GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			if (TypedNativeView != null)
			{
				var desiredSize = TypedNativeView.GetDesiredSize();
				return desiredSize.Request;
			}

			return base.GetDesiredSize(widthConstraint, heightConstraint);
		}
	}
}
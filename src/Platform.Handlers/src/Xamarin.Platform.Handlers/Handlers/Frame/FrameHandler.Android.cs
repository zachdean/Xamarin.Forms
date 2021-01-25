using AndroidX.CardView.Widget;

namespace Xamarin.Platform.Handlers
{
	public partial class FrameHandler : AbstractViewHandler<IFrame, CardView>
	{
		protected override CardView CreateNativeView()
		{
			return new CardView(Context);
		}

		public static void MapBorderColor(FrameHandler handler, IFrame frame)
		{
			ViewHandler.CheckParameters(handler, frame);

			handler.TypedNativeView?.UpdateBorderColor(frame);
		}

		public static void MapHasShadow(FrameHandler handler, IFrame frame)
		{
			ViewHandler.CheckParameters(handler, frame);

			handler.TypedNativeView?.UpdateHasShadow(frame);
		}

		public static void MapCornerRadius(FrameHandler handler, IFrame frame)
		{
			ViewHandler.CheckParameters(handler, frame);

			handler.TypedNativeView?.UpdateCornerRadius(frame);
		}
	}
}
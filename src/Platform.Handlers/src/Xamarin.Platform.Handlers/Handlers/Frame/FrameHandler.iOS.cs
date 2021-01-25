using UIKit;

namespace Xamarin.Platform.Handlers
{
	public partial class FrameHandler : AbstractViewHandler<IFrame, UIView>
	{
		static NativeFrame? ActualView;

		protected override UIView CreateNativeView()
		{
			return new UIView();
		}

		protected override void ConnectHandler(UIView nativeView)
		{
			ActualView = new NativeFrame();
			nativeView.AddSubview(ActualView);
		}

		protected override void DisconnectHandler(UIView nativeView)
		{
			if (ActualView != null)
			{
				ActualView.RemoveFromSuperview();
				ActualView.Dispose();
				ActualView = null;
			}
		}

		public static void MapBackgroundColor(FrameHandler handler, IFrame frame)
		{
			ViewHandler.CheckParameters(handler, frame);

			handler.TypedNativeView?.UpdateBackgroundColor(frame, ActualView);
		}

		public static void MapBorderColor(FrameHandler handler, IFrame frame)
		{
			ViewHandler.CheckParameters(handler, frame);

			handler.TypedNativeView?.UpdateBorderColor(frame, ActualView);
		}

		public static void MapHasShadow(FrameHandler handler, IFrame frame)
		{
			ViewHandler.CheckParameters(handler, frame);

			handler.TypedNativeView?.UpdateHasShadow(frame, ActualView);
		}

		public static void MapCornerRadius(FrameHandler handler, IFrame frame)
		{
			ViewHandler.CheckParameters(handler, frame);

			handler.TypedNativeView?.UpdateCornerRadius(frame, ActualView);
		}
	}
}
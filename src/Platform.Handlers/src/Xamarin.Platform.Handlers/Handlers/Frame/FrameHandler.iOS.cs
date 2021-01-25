namespace Xamarin.Platform.Handlers
{
	public partial class FrameHandler : AbstractViewHandler<IFrame, NativeFrame>
	{
		protected override NativeFrame CreateNativeView()
		{
			return new NativeFrame();
		}

		public static void MapBackgroundColor(FrameHandler handler, IFrame frame)
		{
			ViewHandler.CheckParameters(handler, frame);

			handler.TypedNativeView?.UpdateBackgroundColor(frame);
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
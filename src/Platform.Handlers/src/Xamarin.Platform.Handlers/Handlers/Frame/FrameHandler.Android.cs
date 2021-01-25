using Android.Graphics.Drawables;
using AndroidX.CardView.Widget;

namespace Xamarin.Platform.Handlers
{
	public partial class FrameHandler : AbstractViewHandler<IFrame, CardView>
	{
		static float? DefaultElevation;
		static float? DefaultCornerRadius;

		static GradientDrawable? BackgroundDrawable;

		protected override CardView CreateNativeView()
		{
			var cardView = new CardView(Context);

			BackgroundDrawable = new GradientDrawable();
			BackgroundDrawable.SetShape(ShapeType.Rectangle);

			cardView.Background = BackgroundDrawable;

			return cardView;
		}

		protected override void SetupDefaults(CardView nativeView)
		{
			DefaultElevation = -1f;
			DefaultCornerRadius = -1f;
		}

		public static void MapBackgroundColor(FrameHandler handler, IFrame frame)
		{
			ViewHandler.CheckParameters(handler, frame);

			handler.TypedNativeView?.UpdateBackgroundColor(frame, BackgroundDrawable);
		}

		public static void MapBorderColor(FrameHandler handler, IFrame frame)
		{
			ViewHandler.CheckParameters(handler, frame);

			handler.TypedNativeView?.UpdateBorderColor(frame, BackgroundDrawable);
		}

		public static void MapHasShadow(FrameHandler handler, IFrame frame)
		{
			ViewHandler.CheckParameters(handler, frame);

			handler.TypedNativeView?.UpdateHasShadow(frame, DefaultElevation);
		}

		public static void MapCornerRadius(FrameHandler handler, IFrame frame)
		{
			ViewHandler.CheckParameters(handler, frame);

			handler.TypedNativeView?.UpdateCornerRadius(frame, BackgroundDrawable, DefaultCornerRadius);
		}
	}
}
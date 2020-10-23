using AndroidX.AppCompat.Widget;
using AView = Android.Views.View;

namespace Xamarin.Platform.Handlers
{
	public partial class ButtonHandler : AbstractViewHandler<IButton, AppCompatButton>
	{
		static ButtonBorderBackgroundManager? BackgroundTracker { get; set; }
		static ButtonLayoutManager? ButtonLayoutManager { get; set; }

		ButtonAttachStateChangeListener AttachStateChangeListener { get; } = new ButtonAttachStateChangeListener();
		ButtonClickListener ClickListener { get; } = new ButtonClickListener();
		ButtonTouchListener TouchListener { get; } = new ButtonTouchListener();

		protected override AppCompatButton CreateNativeView()
		{
			AppCompatButton nativeButton = new AppCompatButton(Context)
			{
				SoundEffectsEnabled = false
			};

			return nativeButton;
		}

		protected override void ConnectHandler(AppCompatButton nativeView)
		{
			nativeView.LayoutChange += OnLayoutChange;

			nativeView.AddOnAttachStateChangeListener(AttachStateChangeListener);

			ClickListener.Handler = this;
			nativeView.SetOnClickListener(ClickListener);

			TouchListener.Handler = this;
			nativeView.SetOnTouchListener(TouchListener);

			BackgroundTracker = new ButtonBorderBackgroundManager(nativeView, VirtualView);
			ButtonLayoutManager = new ButtonLayoutManager(nativeView, VirtualView);

			base.ConnectHandler(nativeView);
		}

		protected override void DisconnectHandler(AppCompatButton nativeView)
		{
			nativeView.LayoutChange -= OnLayoutChange;

			nativeView.RemoveOnAttachStateChangeListener(AttachStateChangeListener);

			ClickListener.Handler = null;
			nativeView.SetOnClickListener(null);

			TouchListener.Handler = null;
			nativeView.SetOnTouchListener(null);

			BackgroundTracker?.Dispose();
			BackgroundTracker = null;

			ButtonLayoutManager?.Dispose();
			ButtonLayoutManager = null;

			base.DisconnectHandler(nativeView);
		}

		public static void MapBackgroundColor(ButtonHandler handler, IButton button)
		{
			ViewHandler.CheckParameters(handler, button);
			handler.TypedNativeView?.UpdateBackgroundColor(button, BackgroundTracker);
		}

		public static void MapColor(ButtonHandler handler, IButton button)
		{
			ViewHandler.CheckParameters(handler, button);
			handler.TypedNativeView?.UpdateColor(button);
		}

		public static void MapText(ButtonHandler handler, IButton button)
		{
			ViewHandler.CheckParameters(handler, button);
			handler.TypedNativeView?.UpdateText(button);
		}

		public static void MapFont(ButtonHandler handler, IButton button)
		{
			ViewHandler.CheckParameters(handler, button);
			handler.TypedNativeView?.UpdateFont(button);
		}

		public static void MapCharacterSpacing(ButtonHandler handler, IButton button)
		{
			ViewHandler.CheckParameters(handler, button);
			handler.TypedNativeView?.UpdateCharacterSpacing(button);
		}

		public static void MapCornerRadius(ButtonHandler handler, IButton button)
		{
			ViewHandler.CheckParameters(handler, button);
			handler.TypedNativeView?.UpdateCornerRadius(button, BackgroundTracker);
		}

		public static void MapBorderColor(ButtonHandler handler, IButton button)
		{
			ViewHandler.CheckParameters(handler, button);
			handler.TypedNativeView?.UpdateBorderColor(button, BackgroundTracker);
		}

		public static void MapBorderWidth(ButtonHandler handler, IButton button)
		{
			ViewHandler.CheckParameters(handler, button);
			handler.TypedNativeView?.UpdateBorderWidth(button, BackgroundTracker);
		}

		public static void MapContentLayout(ButtonHandler handler, IButton button)
		{
			ViewHandler.CheckParameters(handler, button);
			handler.TypedNativeView?.UpdateContentLayout(button, ButtonLayoutManager);
		}

		public static void MapPadding(ButtonHandler handler, IButton button)
		{
			ViewHandler.CheckParameters(handler, button);
			handler.TypedNativeView?.UpdatePadding(button, ButtonLayoutManager);
		}

		void OnLayoutChange(object sender, AView.LayoutChangeEventArgs e)
		{
			ButtonLayoutManager?.OnLayout(true, e.Left, e.Top, e.Right, e.Bottom);
		}

		public class ButtonAttachStateChangeListener : Java.Lang.Object, AView.IOnAttachStateChangeListener
		{
			public void OnViewAttachedToWindow(AView? attachedView)
			{
				ButtonLayoutManager?.OnViewAttachedToWindow(attachedView);
			}

			public void OnViewDetachedFromWindow(AView? detachedView)
			{
				ButtonLayoutManager?.OnViewDetachedFromWindow(detachedView);
			}
		}

		public class ButtonClickListener : Java.Lang.Object, AView.IOnClickListener
		{
			public ButtonHandler? Handler { get; set; }

			public void OnClick(AView? v)
			{
				ButtonManager.OnClick(Handler?.VirtualView, v);
			}
		}

		public class ButtonTouchListener : Java.Lang.Object, AView.IOnTouchListener
		{
			public ButtonHandler? Handler { get; set; }

			public bool OnTouch(AView? v, global::Android.Views.MotionEvent? e)
			{
				ButtonManager.OnTouch(Handler?.VirtualView, v, e);
				return true;
			}
		}
	}
}
using System;
using UIKit;

namespace Xamarin.Platform.Handlers
{
	public partial class ButtonHandler : AbstractViewHandler<IButton, UIButton>
	{
		static readonly UIControlState[] ControlStates = { UIControlState.Normal, UIControlState.Highlighted, UIControlState.Disabled };

		static ButtonLayoutManager? ButtonLayoutManager;

		static UIColor? ButtonTextColorDefaultDisabled;
		static UIColor? ButtonTextColorDefaultHighlighted;
		static UIColor? ButtonTextColorDefaultNormal;

		protected override UIButton CreateNativeView()
		{
			SetControlPropertiesFromProxy();
			return new UIButton(UIButtonType.System);
		}

		protected override void ConnectHandler(UIButton nativeView)
		{
			nativeView.TouchUpInside += OnButtonTouchUpInside;
			nativeView.TouchUpOutside += OnButtonTouchUpOutside;
			nativeView.TouchDown += OnButtonTouchDown;

			ButtonLayoutManager = new ButtonLayoutManager(nativeView, VirtualView);
			ButtonLayoutManager.Update();

			base.ConnectHandler(nativeView);
		}

		protected override void DisconnectHandler(UIButton nativeView)
		{
			nativeView.TouchUpInside -= OnButtonTouchUpInside;
			nativeView.TouchUpOutside -= OnButtonTouchUpOutside;
			nativeView.TouchDown -= OnButtonTouchDown;

			ButtonLayoutManager?.Dispose();
			ButtonLayoutManager = null;

			base.DisconnectHandler(nativeView);
		}

		protected override void SetupDefaults(UIButton nativeView)
		{
			ButtonTextColorDefaultNormal = nativeView.TitleColor(UIControlState.Normal);
			ButtonTextColorDefaultHighlighted = nativeView.TitleColor(UIControlState.Highlighted);
			ButtonTextColorDefaultDisabled = nativeView.TitleColor(UIControlState.Disabled);

			base.SetupDefaults(nativeView);
		}
				
		public static void MapColor(ButtonHandler handler, IButton button)
		{
			ViewHandler.CheckParameters(handler, button);
			handler.TypedNativeView?.UpdateColor(button, ButtonTextColorDefaultNormal, ButtonTextColorDefaultHighlighted, ButtonTextColorDefaultDisabled);
		}

		public static void MapText(ButtonHandler handler, IButton button)
		{
			ViewHandler.CheckParameters(handler, button);
			handler.TypedNativeView?.UpdateText(button, ButtonLayoutManager);
		}

		public static void MapFont(ButtonHandler handler, IButton button)
		{
			ViewHandler.CheckParameters(handler, button);
			handler.TypedNativeView?.UpdateFont(button);
		}

		public static void MapCharacterSpacing(ButtonHandler handler, IButton button)
		{
			ViewHandler.CheckParameters(handler, button);
			handler.TypedNativeView?.UpdateCharacterSpacing(button, ButtonLayoutManager);
		}

		public static void MapCornerRadius(ButtonHandler handler, IButton button)
		{
			ViewHandler.CheckParameters(handler, button);
			handler.TypedNativeView?.UpdateCornerRadius(button);
		}

		public static void MapBorderColor(ButtonHandler handler, IButton button)
		{
			ViewHandler.CheckParameters(handler, button);
			handler.TypedNativeView?.UpdateBorderColor(button);
		}

		public static void MapBorderWidth(ButtonHandler handler, IButton button)
		{
			ViewHandler.CheckParameters(handler, button);
			handler.TypedNativeView?.UpdateBorderWidth(button, ButtonLayoutManager);
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

		void SetControlPropertiesFromProxy()
		{
			if (TypedNativeView == null)
				return;

			foreach (UIControlState uiControlState in ControlStates)
			{
				TypedNativeView.SetTitleColor(UIButton.Appearance.TitleColor(uiControlState), uiControlState); // If new values are null, old values are preserved.
				TypedNativeView.SetTitleShadowColor(UIButton.Appearance.TitleShadowColor(uiControlState), uiControlState);
				TypedNativeView.SetBackgroundImage(UIButton.Appearance.BackgroundImageForState(uiControlState), uiControlState);
			}
		}

		void OnButtonTouchUpInside(object sender, EventArgs e)
		{
			ButtonManager.OnTouchUpInside(VirtualView);
		}

		void OnButtonTouchUpOutside(object sender, EventArgs e)
		{
			ButtonManager.OnTouchUpOutside(VirtualView);
		}

		void OnButtonTouchDown(object sender, EventArgs e)
		{
			ButtonManager.OnTouchDown(VirtualView);
		}
	}
}
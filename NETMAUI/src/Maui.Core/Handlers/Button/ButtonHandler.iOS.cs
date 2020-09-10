using UIKit;

namespace System.Maui.Platform
{
	public partial class ButtonHandler : AbstractViewHandler<IButton, UIButton>
	{
		UIColor _buttonTextColorDefaultDisabled;
		UIColor _buttonTextColorDefaultHighlighted;
		UIColor _buttonTextColorDefaultNormal;

		ButtonLayoutManager _buttonLayoutManager;
		BorderElementManager _borderElementManager;

		static readonly UIControlState[] ControlStates = { UIControlState.Normal, UIControlState.Highlighted, UIControlState.Disabled };

		protected override UIButton CreateView()
		{
			var nativeButton = new UIButton(UIButtonType.System);

			nativeButton.TouchUpInside += OnButtonTouchUpInside;
			nativeButton.TouchUpOutside += OnButtonTouchUpOutside;
			nativeButton.TouchDown += OnButtonTouchDown;

			return nativeButton;
		}

		protected override void SetupDefaults()
		{
			SetControlPropertiesFromProxy();

			_buttonTextColorDefaultNormal = TypedNativeView.TitleColor(UIControlState.Normal);
			_buttonTextColorDefaultHighlighted = TypedNativeView.TitleColor(UIControlState.Highlighted);
			_buttonTextColorDefaultDisabled = TypedNativeView.TitleColor(UIControlState.Disabled);

			_buttonLayoutManager = new ButtonLayoutManager(this, VirtualView);
			_borderElementManager = new BorderElementManager(this, VirtualView);

			base.SetupDefaults();
		}

		protected override void DisposeView(UIButton nativeButton)
		{
			nativeButton.TouchUpInside -= OnButtonTouchUpInside;
			nativeButton.TouchUpOutside -= OnButtonTouchUpOutside;
			nativeButton.TouchDown -= OnButtonTouchDown;

			_buttonLayoutManager?.Dispose();
			_buttonLayoutManager = null;

			_borderElementManager?.Dispose();
			_borderElementManager = null;

			base.DisposeView(nativeButton);
		}

		public static void MapPropertyText(IViewHandler Handler, IButton view)
		{
			(Handler as ButtonHandler)?.UpdateText();
		}

		public static void MapPropertyTextColor(IViewHandler Handler, IButton view)
		{
			(Handler as ButtonHandler)?.UpdateTextColor();
		}

		public static void MapPropertyCornerRadius(IViewHandler Handler, IButton view)
		{
			(Handler as ButtonHandler)?.UpdateBorder();
		}

		public static void MapPropertyBorderColor(IViewHandler Handler, IButton view)
		{
			(Handler as ButtonHandler)?.UpdateBorder();
		}

		public static void MapPropertyBorderWidth(IViewHandler Handler, IButton view)
		{
			(Handler as ButtonHandler)?.UpdateBorder();
		}

		public static void MapPropertyFont(IViewHandler Handler, IButton view)
		{
			(Handler as ButtonHandler)?.UpdateFont();
		}

		public static void MapPropertyFontSize(IViewHandler Handler, IButton view)
		{
			(Handler as ButtonHandler)?.UpdateFont();
		}

		public static void MapPropertyFontAttributes(IViewHandler Handler, IButton view)
		{
			(Handler as ButtonHandler)?.UpdateFont();
		}

		public static void MapPropertyCharacterSpacing(IViewHandler Handler, IButton view)
		{
			(Handler as ButtonHandler)?.UpdateText();
		}

		public virtual void UpdateText()
		{
			if (_buttonLayoutManager == null)
				_buttonLayoutManager = new ButtonLayoutManager(this, VirtualView);

			_buttonLayoutManager?.UpdateText();
		}

		public virtual void UpdateTextColor()
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			if (VirtualView.TextColor == Color.Default)
			{
				TypedNativeView.SetTitleColor(_buttonTextColorDefaultNormal, UIControlState.Normal);
				TypedNativeView.SetTitleColor(_buttonTextColorDefaultHighlighted, UIControlState.Highlighted);
				TypedNativeView.SetTitleColor(_buttonTextColorDefaultDisabled, UIControlState.Disabled);
			}
			else
			{
				var color = VirtualView.TextColor.ToNative();

				TypedNativeView.SetTitleColor(color, UIControlState.Normal);
				TypedNativeView.SetTitleColor(color, UIControlState.Highlighted);
				TypedNativeView.SetTitleColor(color, UIControlState.Disabled);

				TypedNativeView.TintColor = color;
			}
		}
		public virtual void UpdateBorder()
		{
			if (_borderElementManager == null)
				_borderElementManager = new BorderElementManager(this, VirtualView);

			_borderElementManager?.UpdateBorder();
		}

		public virtual void UpdateFont()
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			TypedNativeView.TitleLabel.Font = VirtualView.ToUIFont();
		}

		void SetControlPropertiesFromProxy()
		{
			foreach (UIControlState uiControlState in ControlStates)
			{
				TypedNativeView.SetTitleColor(UIButton.Appearance.TitleColor(uiControlState), uiControlState); // if new values are null, old values are preserved.
				TypedNativeView.SetTitleShadowColor(UIButton.Appearance.TitleShadowColor(uiControlState), uiControlState);
				TypedNativeView.SetBackgroundImage(UIButton.Appearance.BackgroundImageForState(uiControlState), uiControlState);
			}
		}

		void OnButtonTouchUpInside(object sender, EventArgs eventArgs)
		{
		
		}

		void OnButtonTouchUpOutside(object sender, EventArgs eventArgs)
		{
		
		}

		void OnButtonTouchDown(object sender, EventArgs eventArgs)
		{
			
		}
	}
}

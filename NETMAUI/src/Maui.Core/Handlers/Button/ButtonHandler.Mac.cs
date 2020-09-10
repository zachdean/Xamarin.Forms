using System.Maui.Controls.Primitives;
using AppKit;
using Foundation;

namespace System.Maui.Platform
{
	public partial class ButtonHandler : AbstractViewHandler<IButton, NativeButton>
	{
		const float DefaultCornerRadius = 6;

		protected override NativeButton CreateView()
		{
			var nativeButton = new NativeButton
			{
				WantsLayer = true
			};

			nativeButton.SetButtonType(NSButtonType.MomentaryPushIn);
			nativeButton.BezelStyle = NSBezelStyle.RoundRect;

			nativeButton.Pressed -= HandleButtonPressed;
			nativeButton.Released -= HandleButtonReleased;
			nativeButton.Activated += HandleButtonActivated;

			return nativeButton;
		}

		protected override void DisposeView(NativeButton nativeView)
		{
			nativeView.Pressed -= HandleButtonPressed;
			nativeView.Released -= HandleButtonReleased;
			nativeView.Activated -= HandleButtonActivated;

			base.DisposeView(nativeView);
		}

		public static void MapPropertyBackgroundColor(IViewHandler Handler, IButton view)
		{
			(Handler as ButtonHandler)?.UpdateBackgroundColor();
		}

		public static void MapPropertyText(IViewHandler Handler, IButton view)
		{
			(Handler as ButtonHandler)?.UpdateText();
		}

		public static void MapPropertyTextColor(IViewHandler Handler, IButton view)
		{
			(Handler as ButtonHandler)?.UpdateText();
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
			(Handler as ButtonHandler)?.UpdateCharacterSpacing();
		}

		public virtual void UpdateBackgroundColor()
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			var backgroundColor = VirtualView.BackgroundColor;
	
			if (backgroundColor != Color.Default)
			{
				TypedNativeView.Cell.BackgroundColor = backgroundColor.ToNative();
			}
		}

		public virtual void UpdateText()
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			var color = VirtualView.TextColor;
			var text = GetTextTransformText() ?? string.Empty;
			if (color == Color.Default)
			{
				TypedNativeView.Title = text;
			}
			else
			{
				var textWithColor = new NSAttributedString(text ?? "", font: VirtualView.Font.ToNSFont(), foregroundColor: color.ToNative(), paragraphStyle: new NSMutableParagraphStyle() { Alignment = NSTextAlignment.Center });
				textWithColor = textWithColor.AddCharacterSpacing(VirtualView.Text ?? string.Empty, VirtualView.CharacterSpacing);
				TypedNativeView.AttributedTitle = textWithColor;
			}
		}

		public virtual void UpdateBorder()
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			if (VirtualView.BorderColor != Color.Default)
				TypedNativeView.Layer.BorderColor = VirtualView.BorderColor.ToCGColor();

			TypedNativeView.Layer.BorderWidth = (float)VirtualView.BorderWidth;
			TypedNativeView.Layer.CornerRadius = VirtualView.CornerRadius > 0 ? VirtualView.CornerRadius : DefaultCornerRadius;

			UpdateBackgroundColor();
		}

		public virtual void UpdateFont()
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			TypedNativeView.Font = VirtualView.ToNSFont();
		}

		public virtual void UpdateCharacterSpacing()
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			TypedNativeView.AttributedTitle = TypedNativeView.AttributedTitle.AddCharacterSpacing(VirtualView.Text ?? string.Empty, VirtualView.CharacterSpacing);
		}

		string GetTextTransformText()
		{
			if (TypedNativeView == null || VirtualView == null)
				return string.Empty;

			string text = VirtualView.TextTransform switch
			{
				TextTransform.Lowercase => VirtualView.Text.ToLowerInvariant(),
				TextTransform.Uppercase => VirtualView.Text.ToUpperInvariant(),
				_ => VirtualView.Text,
			};

			return text;
		}

		void HandleButtonPressed()
		{
			VirtualView?.Pressed();
		}

		void HandleButtonReleased()
		{
			VirtualView?.Released();
		}

		void HandleButtonActivated(object sender, EventArgs eventArgs)
		{
			VirtualView?.Clicked();
		}
	}
}

using UIKit;
using Xamarin.Forms;

namespace Xamarin.Platform
{
	public static class ButtonExtensions
	{
		public static void UpdateText(this UIButton nativeButton, IButton button)
		{
			nativeButton.UpdateText(button, null);
		}

		public static void UpdateText(this UIButton nativeButton, IButton button, ButtonLayoutManager? buttonLayoutManager)
		{
			if (buttonLayoutManager == null)
				buttonLayoutManager = new ButtonLayoutManager(nativeButton, button);

			buttonLayoutManager.UpdateText();
		}

		public static void UpdateColor(this UIButton nativeButton, IButton button)
			=> nativeButton.UpdateColor(button);

		public static void UpdateColor(this UIButton nativeButton, IButton button, UIColor? buttonTextColorDefaultNormal, UIColor? buttonTextColorDefaultHighlighted, UIColor? buttonTextColorDefaultDisabled)
		{
			if (button.Color == Color.Default)
			{
				nativeButton.SetTitleColor(buttonTextColorDefaultNormal, UIControlState.Normal);
				nativeButton.SetTitleColor(buttonTextColorDefaultHighlighted, UIControlState.Highlighted);
				nativeButton.SetTitleColor(buttonTextColorDefaultDisabled, UIControlState.Disabled);
			}
			else
			{
				var color = button.Color.ToNative();

				nativeButton.SetTitleColor(color, UIControlState.Normal);
				nativeButton.SetTitleColor(color, UIControlState.Highlighted);
				nativeButton.SetTitleColor(color, UIControlState.Disabled);

				nativeButton.TintColor = color;
			}
		}

		public static void UpdateFont(this UIButton nativeButton, IButton button)
		{
			nativeButton.TitleLabel.Font = button.ToUIFont();
		}

		public static void UpdateCharacterSpacing(this UIButton nativeButton, IButton button)
		{
			nativeButton.UpdateCharacterSpacing(button, null);
		}

		public static void UpdateCharacterSpacing(this UIButton nativeButton, IButton button, ButtonLayoutManager? buttonLayoutManager)
		{
			if (buttonLayoutManager == null)
				buttonLayoutManager = new ButtonLayoutManager(nativeButton, button);

			buttonLayoutManager.UpdateText();
		}

		public static void UpdateCornerRadius(this UIButton nativeButton, IButton button)
		{
			BorderElementManager.UpdateBorder(nativeButton, button);
		}

		public static void UpdateBorderColor(this UIButton nativeButton, IButton button)
		{
			BorderElementManager.UpdateBorder(nativeButton, button);
		}

		public static void UpdateBorderWidth(this UIButton nativeButton, IButton button)
		{
			nativeButton.UpdateBorderWidth(button, null);
		}

		public static void UpdateBorderWidth(this UIButton nativeButton, IButton button, ButtonLayoutManager? buttonLayoutManager)
		{
			if (buttonLayoutManager == null)
				buttonLayoutManager = new ButtonLayoutManager(nativeButton, button);

			buttonLayoutManager.UpdateEdgeInsets();
		}

		public static void UpdateContentLayout(this UIButton nativeButton, IButton button)
		{
			nativeButton.UpdateContentLayout(button, null);
		}

		public static void UpdateContentLayout(this UIButton nativeButton, IButton button, ButtonLayoutManager? buttonLayoutManager)
		{
			if (buttonLayoutManager == null)
				buttonLayoutManager = new ButtonLayoutManager(nativeButton, button);

			buttonLayoutManager.UpdateEdgeInsets();
		}

		public static void UpdatePadding(this UIButton nativeButton, IButton button)
		{
			nativeButton.UpdatePadding(button, null);
		}

		public static void UpdatePadding(this UIButton nativeButton, IButton button, ButtonLayoutManager? buttonLayoutManager)
		{
			if (buttonLayoutManager == null)
				buttonLayoutManager = new ButtonLayoutManager(nativeButton, button);

			buttonLayoutManager.UpdatePadding();
		}
	}
}
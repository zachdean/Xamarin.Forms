using UIKit;
using Xamarin.Forms;

namespace Xamarin.Platform
{
	public static class SearchBarExtensions
	{
		public static void UpdateSearchCommand(this UISearchBar searchBar, ISearch search)
		{

		}

		public static void UpdateSearchCommandParameter(this UISearchBar searchBar, ISearch search)
		{

		}
		public static void UpdateCancelButtonColor(this UISearchBar searchBar, ISearch search)
		{
			searchBar.UpdateCancelButtonColor(search, null, null, null);
		}

		public static void UpdateCancelButtonColor(this UISearchBar searchBar, ISearch search, UIColor? cancelButtonTextColorDefaultNormal, UIColor? cancelButtonTextColorDefaultHighlighted, UIColor? cancelButtonTextColorDefaultDisabled)
		{
			searchBar.ShowsCancelButton = !string.IsNullOrEmpty(searchBar.Text);

			// We can't cache the cancel button reference because iOS drops it when it's not displayed
			// and creates a brand new one when necessary, so we have to look for it each time
			var cancelButton = searchBar.FindDescendantView<UIButton>();

			if (cancelButton == null)
				return;

			if (search.CancelButtonColor == Color.Default)
			{
				cancelButton.SetTitleColor(cancelButtonTextColorDefaultNormal, UIControlState.Normal);
				cancelButton.SetTitleColor(cancelButtonTextColorDefaultHighlighted, UIControlState.Highlighted);
				cancelButton.SetTitleColor(cancelButtonTextColorDefaultDisabled, UIControlState.Disabled);
			}
			else
			{
				cancelButton.SetTitleColor(search.CancelButtonColor.ToNative(), UIControlState.Normal);
				cancelButton.SetTitleColor(search.CancelButtonColor.ToNative(), UIControlState.Highlighted);
				cancelButton.SetTitleColor(search.CancelButtonColor.ToNative(), UIControlState.Disabled);	
			}
		}

		public static void UpdateText(this UISearchBar searchBar, ISearch search)
		{
			searchBar.SetText(search);
		}

		public static void UpdateTextColor(this UISearchBar searchBar, ISearch search)
		{
			UpdateTextColor(searchBar, search, null);
		}

		public static void UpdateTextColor(this UISearchBar searchBar, ISearch search, UIColor? defaultTextColor)
		{
			searchBar.UpdateTextColor(null, search, defaultTextColor);
		}

		public static void UpdateTextColor(this UISearchBar searchBar, UITextField? textField, ISearch search, UIColor? defaultTextColor)
		{
			if (searchBar == null)
				return;

			searchBar?.SetTextColor(textField, search, defaultTextColor);
		}

		public static void UpdateTextTransform(this UISearchBar searchBar, ISearch search)
		{
			searchBar.SetText(search);
		}

		public static void UpdateCharacterSpacing(this UISearchBar searchBar, ISearch search)
		{
			UpdateCharacterSpacing(searchBar, null, search);
		}

		public static void UpdateCharacterSpacing(this UISearchBar searchBar, UITextField? textField, ISearch search)
		{
			textField ??= searchBar.FindDescendantView<UITextField>();

			if (textField == null)
				return;

			textField.AttributedText = textField.AttributedText?.AddCharacterSpacing(search.Text, search.CharacterSpacing);
			textField.AttributedPlaceholder = textField.AttributedPlaceholder?.AddCharacterSpacing(search.Placeholder, search.CharacterSpacing);
		}

		public static void UpdatePlaceholder(this UISearchBar searchBar, ISearch search)
		{

		}

		public static void UpdatePlaceholderColor(this UISearchBar searchBar, ISearch search)
		{
		
		}

		public static void UpdateFontAttributes(this UISearchBar searchBar, ISearch search)
		{
			UpdateFontAttributes(searchBar, null, search);
		}

		public static void UpdateFontAttributes(this UISearchBar searchBar, UITextField? textField, ISearch search)
		{
			searchBar.SetFont(textField, search);
		}

		public static void UpdateFontFamily(this UISearchBar searchBar, ISearch search)
		{
			UpdateFontFamily(searchBar, null, search);
		}

		public static void UpdateFontFamily(this UISearchBar searchBar, UITextField? textField, ISearch search)
		{
			searchBar.SetFont(textField, search);
		}

		public static void UpdateFontSize(this UISearchBar searchBar, ISearch search)
		{
			UpdateFontSize(searchBar, null, search);
		}

		public static void UpdateFontSize(this UISearchBar searchBar, UITextField? textField, ISearch search)
		{
			searchBar.SetFont(textField, search);
		}

		public static void UpdateKeyboard(this UISearchBar searchBar, ISearch search)
		{

		}

		public static void UpdateIsSpellCheckEnabled(this UISearchBar searchBar, ISearch search)
		{

		}

		public static void UpdateHorizontalTextAlignment(this UISearchBar searchBar, ISearch search)
		{
			UpdateHorizontalTextAlignment(searchBar, null, search);
		}

		public static void UpdateHorizontalTextAlignment(this UISearchBar searchBar, UITextField? textField, ISearch search)
		{
			textField ??= searchBar.FindDescendantView<UITextField>();

			if (textField == null)
				return;

			// TODO: Pass the EffectiveFlowDirection.
			textField.TextAlignment = search.HorizontalTextAlignment.ToNativeTextAlignment(EffectiveFlowDirection.Explicit);
		}

		public static void UpdateVerticalTextAlignment(this UISearchBar searchBar, ISearch search)
		{
			UpdateVerticalTextAlignment(searchBar, null, search);
		}

		public static void UpdateVerticalTextAlignment(this UISearchBar searchBar, UITextField? textField, ISearch search)
		{
			textField ??= searchBar.FindDescendantView<UITextField>();

			if (textField == null)
				return;

			textField.VerticalAlignment = search.VerticalTextAlignment.ToNativeTextAlignment();
		}

		internal static void SetText(this UISearchBar searchBar, ISearch search)
		{
			searchBar.Text = search.UpdateTransformedText(search.Text, search.TextTransform);
		}

		internal static void SetTextColor(this UISearchBar searchBar, UITextField? textField, ISearch search, UIColor? defaultTextColor)
		{
			textField ??= searchBar.FindDescendantView<UITextField>();

			if (textField == null)
				return;

			defaultTextColor ??= textField.TextColor;
			var targetColor = search.Color;

			textField.TextColor = targetColor.IsDefault ? defaultTextColor : targetColor.ToNative();
		}

		internal static void SetFont(this UISearchBar searchBar, UITextField? textField, ISearch search)
		{
			textField ??= searchBar.FindDescendantView<UITextField>();

			if (textField == null)
				return;

			textField.Font = search.ToUIFont();
		}
	}
}
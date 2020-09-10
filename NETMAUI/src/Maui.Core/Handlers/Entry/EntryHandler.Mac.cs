using AppKit;
using System.Maui.Controls.Primitives;

namespace System.Maui.Platform
{
	public partial class EntryHandler : AbstractViewHandler<IEntry, NSTextField>
	{
		NSColor _defaultTextColor;

		protected override NSTextField CreateView()
		{
			NSTextField textField;

			if (VirtualView.IsPassword)
				textField = new NSSecureTextField();
			else
			{
				textField = new NativeEntry();
				(textField as NativeEntry).FocusChanged += TextFieldFocusChanged;
				(textField as NativeEntry).Completed += OnCompleted;
			}

			_defaultTextColor = ColorExtensions.LabelColor; 

			textField.Changed += OnChanged;
			textField.EditingBegan += OnEditingBegan;
			textField.EditingEnded += OnEditingEnded;

			return textField;
		}

        protected override void DisposeView(NSTextField nativeView)
        {
			_defaultTextColor = null;

			nativeView.EditingBegan -= OnEditingBegan;
			nativeView.Changed -= OnChanged;
			nativeView.EditingEnded -= OnEditingEnded;

            if (nativeView is NativeEntry nativeEntry)
            {
				nativeEntry.FocusChanged -= TextFieldFocusChanged;
				nativeEntry.Completed -= OnCompleted;
            }

            base.DisposeView(nativeView);
        }

		public static void MapPropertyText(IViewHandler Handler, ITextInput view)
		{
			(Handler as EntryHandler)?.UpdateText();
		}

		public static void MapPropertyColor(IViewHandler Handler, ITextInput entry)
		{
			(Handler as EntryHandler)?.UpdateTextColor();
		}

		public static void MapPropertyFont(IViewHandler Handler, IEntry entry)
		{

		}

		public static void MapPropertyTextTransform(IViewHandler Handler, IEntry entry)
		{

		}

		public static void MapPropertyCharacterSpacing(IViewHandler Handler, IEntry entry)
		{

		}

		public static void MapPropertyPlaceholder(IViewHandler Handler, ITextInput entry)
		{
			(Handler as EntryHandler)?.UpdatePlaceHolder();
		}

		public static void MapPropertyPlaceholderColor(IViewHandler Handler, ITextInput entry)
		{
			(Handler as EntryHandler)?.UpdatePlaceHolder();
		}

		public static void MapPropertyMaxLength(IViewHandler Handler, IEntry entry)
		{
			(Handler as EntryHandler)?.UpdateMaxLength();
		}

		public static void MapPropertyIsReadOnly(IViewHandler Handler, IEntry entry)
		{
			(Handler as EntryHandler)?.UpdateIsReadOnly();
		}

		public static void MapPropertyKeyboard(IViewHandler Handler, IEntry entry)
		{

		}

		public static void MapPropertyIsSpellCheckEnabled(IViewHandler Handler, IEntry entry)
		{

		}

		public static void MapPropertyHorizontalTextAlignment(IViewHandler Handler, IEntry entry)
		{
			(Handler as EntryHandler)?.UpdateAlignment();
		}

		public static void MapPropertyVerticalTextAlignment(IViewHandler Handler, IEntry entry)
		{

		}

		public static void MapPropertyFontSize(IViewHandler Handler, IEntry entry)
		{
			(Handler as EntryHandler)?.UpdateFont();
		}

		public static void MapPropertyFontAttributes(IViewHandler Handler, IEntry entry)
		{
			(Handler as EntryHandler)?.UpdateFont();
		}

		public static void MapPropertyIsPassword(IViewHandler Handler, IEntry entry)
		{

		}

		public static void MapPropertyReturnType(IViewHandler Handler, IEntry entry)
		{

		}

		public static void MapPropertyCursorPosition(IViewHandler Handler, IEntry entry)
		{

		}

		public static void MapPropertySelectionLength(IViewHandler Handler, IEntry entry)
		{

		}

		public static void MapPropertyIsTextPredictionEnabled(IViewHandler Handler, IEntry entry)
		{

		}

		public static void MapPropertyClearButtonVisibility(IViewHandler Handler, IEntry entry)
		{

		}

		public virtual void UpdateText()
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			var text = GetTextTransformText();

			if (string.IsNullOrEmpty(text))
				return;

			TypedNativeView.StringValue = text;
		}

		public virtual void UpdateTextColor()
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			var textColor = VirtualView.TextColor;

			if (textColor.IsDefault || !VirtualView.IsEnabled)
				TypedNativeView.TextColor = _defaultTextColor;
			else
				TypedNativeView.TextColor = textColor.ToNative();
		}

		public virtual void UpdatePlaceHolder()
        {
			if (TypedNativeView == null || VirtualView == null)
				return;

			var formatted = (FormattedString)VirtualView.Placeholder;

			if (formatted == null)
				return;

			var targetColor = VirtualView.PlaceholderColor;

			// Placeholder default color is 70% gray
			// https://developer.apple.com/library/prerelease/ios/documentation/UIKit/Reference/UITextField_Class/index.html#//apple_ref/occ/instp/UITextField/placeholder
			var color = VirtualView.IsEnabled && !targetColor.IsDefault ? targetColor : ColorExtensions.SeventyPercentGrey.ToColor();

			TypedNativeView.PlaceholderAttributedString = formatted.ToAttributed(VirtualView, color, VirtualView.HorizontalTextAlignment);
		}

		public virtual void UpdateMaxLength()
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			var currentControlText = TypedNativeView?.StringValue;

			if (currentControlText.Length > VirtualView?.MaxLength)
				TypedNativeView.StringValue = currentControlText.Substring(0, VirtualView.MaxLength);
		}

		public virtual void UpdateFont()
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			TypedNativeView.Font = VirtualView.ToNSFont();
		}

		public virtual void UpdateAlignment()
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			TypedNativeView.Alignment = VirtualView.HorizontalTextAlignment.ToNativeTextAlignment();
		}

		public virtual void UpdateIsReadOnly()
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			TypedNativeView.Editable = !VirtualView.IsReadOnly;

			if (VirtualView.IsReadOnly && TypedNativeView.Window?.FirstResponder == TypedNativeView.CurrentEditor)
				TypedNativeView.Window?.MakeFirstResponder(null);
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

		void TextFieldFocusChanged(object sender, BoolEventArgs e)
		{

		}

		void OnEditingBegan(object sender, EventArgs e)
		{

		}

		void OnChanged(object sender, EventArgs eventArgs)
		{

		}

		void OnEditingEnded(object sender, EventArgs e)
		{

		}

		void OnCompleted(object sender, EventArgs e)
		{

		}
	}
}
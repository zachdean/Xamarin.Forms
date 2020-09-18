using System.Collections.Generic;
using Android.Text;
using Android.Widget;

namespace Xamarin.Platform.Handlers
{
	public partial class EntryHandler : AbstractViewHandler<IEntry, EditText>
	{
		protected override EditText CreateView()
		{
			var editText = new EditText(Context);

			return editText;
		}

		public static void MapText(IViewHandler handler, IEntry entry)
		{
			(handler as EntryHandler)?.UpdateText();
		}

		public static void MapColor(IViewHandler handler, IEntry entry)
		{

		}

		public static void MapFont(IViewHandler handler, IEntry entry)
		{

		}

		public static void MapTextTransform(IViewHandler handler, IEntry entry)
		{
			(handler as EntryHandler)?.UpdateText();
		}

		public static void MapCharacterSpacing(IViewHandler handler, IEntry entry)
		{
			(handler as EntryHandler)?.UpdateCharacterSpacing();
		}

		public static void MapPlaceholder(IViewHandler handler, IEntry entry)
		{
			(handler as EntryHandler)?.UpdatePlaceholder();
		}

		public static void MapMaxLength(IViewHandler handler, IEntry entry)
		{
			(handler as EntryHandler)?.UpdateMaxLength();
		}

		public static void MapIsReadOnly(IViewHandler handler, IEntry entry)
		{
			(handler as EntryHandler)?.UpdateIsReadOnly();
		}

		public static void MapKeyboard(IViewHandler handler, IEntry entry)
		{

		}

		public static void MapIsSpellCheckEnabled(IViewHandler handler, IEntry entry)
		{

		}

		public static void MapPlaceholderColor(IViewHandler handler, IEntry entry)
		{

		}

		public static void MapHorizontalTextAlignment(IViewHandler handler, IEntry entry)
		{
			(handler as EntryHandler)?.UpdateHorizontalTextAlignment();
		}

		public static void MapVerticalTextAlignment(IViewHandler handler, IEntry entry)
		{
			(handler as EntryHandler)?.UpdateVerticalTextAlignment();
		}

		public static void MapFontSize(IViewHandler handler, IEntry entry)
		{

		}

		public static void MapFontAttributes(IViewHandler handler, IEntry entry)
		{

		}

		public static void MapIsPassword(IViewHandler handler, IEntry entry)
		{

		}

		public static void MapReturnType(IViewHandler handler, IEntry entry)
		{

		}

		public static void MapCursorPosition(IViewHandler handler, IEntry entry)
		{

		}

		public static void MapSelectionLength(IViewHandler handler, IEntry entry)
		{

		}

		public static void MapIsTextPredictionEnabled(IViewHandler handler, IEntry entry)
		{

		}

		public static void MapClearButtonVisibility(IViewHandler handler, IEntry entry)
		{

		}

		void UpdateText()
		{
			var text = VirtualView.UpdateTransformedText(VirtualView.Text, VirtualView.TextTransform);

			if (TypedNativeView.Text == text)
				return;

			TypedNativeView.Text = text;

			if (TypedNativeView.IsFocused)
			{
				TypedNativeView.SetSelection(text.Length);
				// TODO: Port KeyboardManager to Xamarin.Platform.
				//TypedNativeView.ShowKeyboard();
			}
		}

		void UpdatePlaceholder()
		{
			if (TypedNativeView.Hint == VirtualView.Placeholder)
				return;

			TypedNativeView.Hint = VirtualView.Placeholder;

			if (TypedNativeView.IsFocused)
			{
				// TODO: Port KeyboardManager to Xamarin.Platform.
				//TypedNativeView.ShowKeyboard();
			}
		}

		void UpdateCharacterSpacing()
		{
			if (NativeVersion.IsAtLeast(21))
			{
				TypedNativeView.LetterSpacing = VirtualView.CharacterSpacing.ToEm();
			}
		}

		void UpdateMaxLength()
		{
			var currentFilters = new List<IInputFilter>(TypedNativeView?.GetFilters() ?? new IInputFilter[0]);

			for (var i = 0; i < currentFilters.Count; i++)
			{
				if (currentFilters[i] is InputFilterLengthFilter)
				{
					currentFilters.RemoveAt(i);
					break;
				}
			}

			currentFilters.Add(new InputFilterLengthFilter(VirtualView.MaxLength));

			TypedNativeView?.SetFilters(currentFilters.ToArray());

			var currentControlText = TypedNativeView?.Text;

			if (currentControlText.Length > VirtualView.MaxLength)
				TypedNativeView.Text = currentControlText.Substring(0, VirtualView.MaxLength);
		}

		void UpdateHorizontalTextAlignment()
		{
			TypedNativeView.UpdateTextAlignment(VirtualView.HorizontalTextAlignment, VirtualView.VerticalTextAlignment);
		}

		void UpdateVerticalTextAlignment()
		{
			TypedNativeView.UpdateTextAlignment(VirtualView.HorizontalTextAlignment, VirtualView.VerticalTextAlignment);
		}

		void UpdateIsReadOnly()
		{
			bool isReadOnly = !VirtualView.IsReadOnly;
			TypedNativeView.FocusableInTouchMode = isReadOnly;
			TypedNativeView.Focusable = isReadOnly;
		}
	}
}
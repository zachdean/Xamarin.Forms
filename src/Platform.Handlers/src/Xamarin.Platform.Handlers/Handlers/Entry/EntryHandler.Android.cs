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

		}

		public static void MapCharacterSpacing(IViewHandler handler, IEntry entry)
		{

		}

		public static void MapPlaceholder(IViewHandler handler, IEntry entry)
		{

		}

		public static void MapMaxLength(IViewHandler handler, IEntry entry)
		{

		}

		public static void MapIsReadOnly(IViewHandler handler, IEntry entry)
		{

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

		}

		public static void MapVerticalTextAlignment(IViewHandler handler, IEntry entry)
		{

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
	}
}
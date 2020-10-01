namespace Xamarin.Platform.Handlers
{
	public partial class EntryHandler
	{
		public static PropertyMapper<IEntry, EntryHandler> EntryMapper = new PropertyMapper<IEntry, EntryHandler>(ViewHandler.ViewMapper)
		{
			[nameof(IText.Text)] = MapText,
			[nameof(IText.TextColor)] = MapColor,
			[nameof(IText.Font)] = MapFont,
			[nameof(IText.TextTransform)] = MapTextTransform,
			[nameof(IText.CharacterSpacing)] = MapCharacterSpacing,

			[nameof(ITextInput.Placeholder)] = MapPlaceholder,
			[nameof(ITextInput.PlaceholderColor)] = MapPlaceholderColor,
			[nameof(ITextInput.MaxLength)] = MapMaxLength,
			[nameof(ITextInput.IsReadOnly)] = MapIsReadOnly,
			[nameof(ITextInput.Keyboard)] = MapKeyboard,
			[nameof(ITextInput.IsSpellCheckEnabled)] = MapIsSpellCheckEnabled,

			[nameof(ITextAlignment.HorizontalTextAlignment)] = MapHorizontalTextAlignment,
			[nameof(ITextAlignment.VerticalTextAlignment)] = MapVerticalTextAlignment,

			[nameof(IFont.FontSize)] = MapFont,
			[nameof(IFont.FontAttributes)] = MapFont,

			[nameof(IEntry.IsPassword)] = MapIsPassword,
			[nameof(IEntry.ReturnType)] = MapReturnType,
			[nameof(IEntry.CursorPosition)] = MapCursorPosition,
			[nameof(IEntry.SelectionLength)] = MapSelectionLength,
			[nameof(IEntry.IsTextPredictionEnabled)] = MapIsTextPredictionEnabled,
			[nameof(IEntry.ClearButtonVisibility)] = MapClearButtonVisibility
		};

		public static void MapText(EntryHandler handler, IEntry entry)
		{
			ViewHandler.CheckParameters(handler, entry);
			handler.TypedNativeView?.UpdateText(entry);
		}

		public static void MapColor(EntryHandler handler, IEntry entry)
		{
			ViewHandler.CheckParameters(handler, entry);
			handler.TypedNativeView?.UpdateColor(entry);
		}

		public static void MapFont(EntryHandler handler, IEntry entry)
		{
			ViewHandler.CheckParameters(handler, entry);
			handler.TypedNativeView?.UpdateFont(entry);
		}

		public static void MapTextTransform(EntryHandler handler, IEntry entry)
		{
			ViewHandler.CheckParameters(handler, entry);
			handler.TypedNativeView?.UpdateTextTransform(entry);
		}

		public static void MapCharacterSpacing(EntryHandler handler, IEntry entry)
		{
			ViewHandler.CheckParameters(handler, entry);
			handler.TypedNativeView?.UpdateCharacterSpacing(entry);
		}

		public static void MapPlaceholder(EntryHandler handler, IEntry entry)
		{
			ViewHandler.CheckParameters(handler, entry);
			handler.TypedNativeView?.UpdatePlaceholder(entry);
		}

		public static void MapPlaceholderColor(EntryHandler handler, IEntry entry)
		{
			ViewHandler.CheckParameters(handler, entry);
			handler.TypedNativeView?.UpdatePlaceholderColor(entry);
		}

		public static void MapMaxLength(EntryHandler handler, IEntry entry)
		{
			ViewHandler.CheckParameters(handler, entry);
			handler.TypedNativeView?.UpdateMaxLength(entry);
		}

		public static void MapIsReadOnly(EntryHandler handler, IEntry entry)
		{
			ViewHandler.CheckParameters(handler, entry);
			handler.TypedNativeView?.UpdateIsReadOnly(entry);
		}

		public static void MapKeyboard(EntryHandler handler, IEntry entry)
		{
			ViewHandler.CheckParameters(handler, entry);
			handler.TypedNativeView?.UpdateKeyboard(entry);
		}

		public static void MapIsSpellCheckEnabled(EntryHandler handler, IEntry entry)
		{
			ViewHandler.CheckParameters(handler, entry);
			handler.TypedNativeView?.UpdateIsSpellCheckEnabled(entry);
		}

		public static void MapHorizontalTextAlignment(EntryHandler handler, IEntry entry)
		{
			ViewHandler.CheckParameters(handler, entry);
			handler.TypedNativeView?.UpdateHorizontalTextAlignment(entry);
		}

		public static void MapVerticalTextAlignment(EntryHandler handler, IEntry entry)
		{
			ViewHandler.CheckParameters(handler, entry);
			handler.TypedNativeView?.UpdateVerticalTextAlignment(entry);
		}

		public static void MapIsPassword(EntryHandler handler, IEntry entry)
		{
			ViewHandler.CheckParameters(handler, entry);
			handler.TypedNativeView?.UpdateIsPassword(entry);
		}

		public static void MapReturnType(EntryHandler handler, IEntry entry)
		{
			ViewHandler.CheckParameters(handler, entry);
			handler.TypedNativeView?.UpdateReturnType(entry);
		}

		public static void MapCursorPosition(EntryHandler handler, IEntry entry)
		{
			ViewHandler.CheckParameters(handler, entry);
			handler.TypedNativeView?.UpdateCursorPosition(entry);
		}

		public static void MapSelectionLength(EntryHandler handler, IEntry entry)
		{
			ViewHandler.CheckParameters(handler, entry);
			handler.TypedNativeView?.UpdateSelectionLength(entry);
		}

		public static void MapIsTextPredictionEnabled(EntryHandler handler, IEntry entry)
		{
			ViewHandler.CheckParameters(handler, entry);
			handler.TypedNativeView?.UpdateIsTextPredictionEnabled(entry);
		}

		public static void MapClearButtonVisibility(EntryHandler handler, IEntry entry)
		{
			ViewHandler.CheckParameters(handler, entry);
			handler.TypedNativeView?.UpdateClearButtonVisibility(entry);
		}

		public EntryHandler() : base(EntryMapper)
		{

		}

		public EntryHandler(PropertyMapper mapper) : base(mapper ?? EntryMapper)
		{

		}
	}
}

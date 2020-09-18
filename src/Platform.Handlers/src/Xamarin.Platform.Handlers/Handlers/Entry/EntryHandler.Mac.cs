using AppKit;

namespace Xamarin.Platform.Handlers
{
	public partial class EntryHandler : AbstractViewHandler<IEntry, NSTextField>
	{
		protected override NSTextField CreateView()
		{
			NSTextField textField = new NSTextField();

			return textField;
		}

		public static void MapText(IViewHandler handler, ITextInput view)
		{
		
		}

		public static void MapColor(IViewHandler handler, ITextInput entry)
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

		public static void MapPlaceholder(IViewHandler handler, ITextInput entry)
		{
	
		}

		public static void MapPlaceholderColor(IViewHandler handler, ITextInput entry)
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
	}
}
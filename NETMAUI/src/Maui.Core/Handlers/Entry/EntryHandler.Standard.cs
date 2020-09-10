
namespace System.Maui.Platform
{
	public partial class EntryHandler : AbstractViewHandler<IEntry, object>
	{
		protected override object CreateView() => throw new NotImplementedException();

		public static void MapPropertyText(IViewHandler Handler, IEntry entry) { }
		public static void MapPropertyColor(IViewHandler Handler, IEntry entry) { }
		public static void MapPropertyFont(IViewHandler Handler, IEntry entry) { }
		public static void MapPropertyTextTransform(IViewHandler Handler, IEntry entry) { }
		public static void MapPropertyCharacterSpacing(IViewHandler Handler, IEntry entry) { }
		public static void MapPropertyPlaceholder(IViewHandler Handler, IEntry entry) { }
		public static void MapPropertyPlaceholderColor(IViewHandler Handler, IEntry entry) { }
		public static void MapPropertyMaxLength(IViewHandler Handler, IEntry entry) { }
		public static void MapPropertyIsReadOnly(IViewHandler Handler, IEntry entry) { }
		public static void MapPropertyKeyboard(IViewHandler Handler, IEntry entry) { }
		public static void MapPropertyIsSpellCheckEnabled(IViewHandler Handler, IEntry entry) { }
		public static void MapPropertyHorizontalTextAlignment(IViewHandler Handler, IEntry entry) { }
		public static void MapPropertyVerticalTextAlignment(IViewHandler Handler, IEntry entry) { }
		public static void MapPropertyFontSize(IViewHandler Handler, IEntry entry) { }
		public static void MapPropertyFontAttributes(IViewHandler Handler, IEntry entry) { }
		public static void MapPropertyIsPassword(IViewHandler Handler, IEntry entry) { }
		public static void MapPropertyReturnType(IViewHandler Handler, IEntry entry) { }
		public static void MapPropertyCursorPosition(IViewHandler Handler, IEntry entry) { }
		public static void MapPropertySelectionLength(IViewHandler Handler, IEntry entry) { }
		public static void MapPropertyIsTextPredictionEnabled(IViewHandler Handler, IEntry entry) { }
		public static void MapPropertyClearButtonVisibility(IViewHandler Handler, IEntry entry) { }
	}
}

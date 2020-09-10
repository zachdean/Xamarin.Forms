namespace System.Maui.Platform
{
	public partial class LabelHandler : AbstractViewHandler<ILabel, object>
	{
		public static void MapPropertyText(IViewHandler Handler, IText view) { }
		public static void MapPropertyTextColor(IViewHandler Handler, IText view) { }
		public static void MapPropertyFont(IViewHandler Handler, IText view) { }
		public static void MapPropertyFontSize(IViewHandler Handler, IText view) { }
		public static void MapPropertyFontAttributes(IViewHandler Handler, IText view) { }
		public static void MapPropertyTextTransform(IViewHandler Handler, IText view) { }
		public static void MapPropertyHorizontalTextAlignment(IViewHandler Handler, IText view) { }
		public static void MapPropertyVerticalTextAlignment(IViewHandler Handler, IText view) { }
		public static void MapPropertyCharacterSpacing(IViewHandler Handler, IText view) { }
		public static void MapPropertyLineHeight(IViewHandler Handler, ILabel view) { }
		public static void MapPropertyTextDecorations(IViewHandler Handler, ILabel view) { }
		public static void MapPropertyLineBreakMode(IViewHandler Handler, ILabel view) { }
		public static void MapPropertyMaxLines(IViewHandler Handler, ILabel view) { }

		protected override object CreateView() => throw new NotImplementedException();
	}
}
namespace System.Maui.Platform
{
	public partial class ButtonHandler : AbstractViewHandler<IButton, object>
	{
		public static void MapPropertyText(IViewHandler Handler, IButton view) { }
		public static void MapPropertyTextColor(IViewHandler Handler, IButton view) { }
		public static void MapPropertyCornerRadius(IViewHandler Handler, IButton view) { }
		public static void MapPropertyBorderColor(IViewHandler Handler, IButton view) { }
		public static void MapPropertyBorderWidth(IViewHandler Handler, IButton view) { }
		public static void MapPropertyFont(IViewHandler Handler, IButton view) { }
		public static void MapPropertyFontSize(IViewHandler Handler, IButton view) { }
		public static void MapPropertyFontAttributes(IViewHandler Handler, IButton view) { }
		public static void MapPropertyCharacterSpacing(IViewHandler Handler, IButton view) { }

		protected override object CreateView() => throw new NotImplementedException();
	}
}
using System;

namespace Xamarin.Platform.Handlers
{
	public partial class SearchBarHandler : AbstractViewHandler<ISearch, object>
	{
		protected override object CreateNativeView() => throw new NotImplementedException();

		public static void MapSearchCommand(IViewHandler handler, ISearch search) { }
		public static void MapSearchCommandParameter(IViewHandler handler, ISearch search) { }
		public static void MapCancelButtonColor(IViewHandler handler, ISearch search) { }
		public static void MapText(IViewHandler handler, ISearch search) { }
		public static void MapTextColor(IViewHandler handler, ISearch search) { }
		public static void MapTextTransform(IViewHandler handler, ISearch search) { }
		public static void MapCharacterSpacing(IViewHandler handler, ISearch search) { }
		public static void MapPlaceholder(IViewHandler handler, ISearch search) { }
		public static void MapPlaceholderColor(IViewHandler handler, ISearch search) { }
		public static void MapFontAttributes(IViewHandler handler, ISearch search) { }
		public static void MapFontFamily(IViewHandler handler, ISearch search) { }
		public static void MapFontSize(IViewHandler handler, ISearch search) { }
		public static void MapKeyboard(IViewHandler handler, ISearch search) { }
		public static void MapIsSpellCheckEnabled(IViewHandler handler, ISearch search) { }
		public static void MapHorizontalTextAlignment(IViewHandler handler, ISearch search) { }
		public static void MapVerticalTextAlignment(IViewHandler handler, ISearch search) { }
	}
}
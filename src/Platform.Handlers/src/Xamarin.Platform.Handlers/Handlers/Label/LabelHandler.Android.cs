using System;
using Android.Widget;

namespace Xamarin.Platform.Handlers
{
	public partial class LabelHandler : AbstractViewHandler<ILabel, TextView>
	{
		protected override TextView CreateView() => throw new NotImplementedException();

		public static void MapPropertyText(IViewHandler handler, IText text) { }
		public static void MapPropertyTextColor(IViewHandler handler, IText text) { }
		public static void MapPropertyFont(IViewHandler handler, IText text) { }
		public static void MapPropertyFontSize(IViewHandler handler, IText text) { }
		public static void MapPropertyFontAttributes(IViewHandler handler, IText text) { }
		public static void MapPropertyTextTransform(IViewHandler handler, IText text) { }
		public static void MapPropertyHorizontalTextAlignment(IViewHandler handler, IText text) { }
		public static void MapPropertyVerticalTextAlignment(IViewHandler handler, IText text) { }
		public static void MapPropertyCharacterSpacing(IViewHandler handler, IText text) { }
		public static void MapPropertyLineHeight(IViewHandler handler, ILabel label) { }
		public static void MapPropertyTextDecorations(IViewHandler handler, ILabel label) { }
		public static void MapPropertyLineBreakMode(IViewHandler handler, ILabel label) { }
		public static void MapPropertyMaxLines(IViewHandler handler, ILabel label) { }
		public static void MapPropertyPadding(IViewHandler handler, ILabel label) { }
	}
}
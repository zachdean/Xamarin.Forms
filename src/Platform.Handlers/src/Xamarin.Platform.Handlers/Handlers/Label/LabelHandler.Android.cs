using Android.Widget;

namespace Xamarin.Platform.Handlers
{
	public partial class LabelHandler : AbstractViewHandler<ILabel, TextView>
	{
		protected override TextView CreateView()
		{
			var text = new TextView(Context);

			return text;
		}

		public static void MapPropertyText(IViewHandler handler, ILabel view)
		{
		
		}

		public static void MapPropertyTextColor(IViewHandler handler, ILabel view)
		{
		
		}

		public static void MapPropertyLineHeight(IViewHandler handler, ILabel view)
		{

		}

		public static void MapPropertyFont(IViewHandler handler, IText view)
		{
	
		}

		public static void MapPropertyFontSize(IViewHandler handler, IText view)
		{
	
		}

		public static void MapPropertyFontAttributes(IViewHandler handler, IText view)
		{

		}

		public static void MapPropertyTextTransform(IViewHandler handler, IText view)
		{

		}

		public static void MapPropertyHorizontalTextAlignment(IViewHandler handler, IText view)
		{
		
		}

		public static void MapPropertyVerticalTextAlignment(IViewHandler handler, IText view)
		{
			
		}

		public static void MapPropertyCharacterSpacing(IViewHandler handler, IText view)
		{
	
		}

		public static void MapPropertyTextDecorations(IViewHandler handler, ILabel view)
		{

		}

		public static void MapPropertyLineBreakMode(IViewHandler handler, ILabel view)
		{

		}

		public static void MapPropertyMaxLines(IViewHandler handler, ILabel view)
		{
	
		}

		public static void MapPropertyPadding(IViewHandler handler, ILabel view)
		{
	
		}
	}
}
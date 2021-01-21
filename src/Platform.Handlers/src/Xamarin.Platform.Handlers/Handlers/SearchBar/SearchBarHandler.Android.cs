using Android.Widget;

namespace Xamarin.Platform.Handlers
{
	public partial class SearchBarHandler : AbstractViewHandler<ISearch, SearchView>
	{
		protected override SearchView CreateNativeView()
		{
			return new SearchView(Context);
		}

		protected override void ConnectHandler(SearchView nativeView)
		{
			base.ConnectHandler(nativeView);
		}

		protected override void DisconnectHandler(SearchView nativeView)
		{
			base.DisconnectHandler(nativeView);
		}

		protected override void SetupDefaults(SearchView nativeView)
		{
			base.SetupDefaults(nativeView);
		}

		public static void MapSearchCommand(SearchBarHandler handler, ISearch search)
		{
			ViewHandler.CheckParameters(handler, search);

			handler.TypedNativeView?.UpdateSearchCommand(search);
		}

		public static void MapSearchCommandParameter(SearchBarHandler handler, ISearch search)
		{
			ViewHandler.CheckParameters(handler, search);

			handler.TypedNativeView?.UpdateSearchCommandParameter(search);
		}

		public static void MapCancelButtonColor(SearchBarHandler handler, ISearch search)
		{
			ViewHandler.CheckParameters(handler, search);

			handler.TypedNativeView?.UpdateCancelButtonColor(search);
		}

		public static void MapText(SearchBarHandler handler, ISearch search)
		{
			ViewHandler.CheckParameters(handler, search);

			handler.TypedNativeView?.UpdateText(search);
		}

		public static void MapTextColor(SearchBarHandler handler, ISearch search)
		{
			ViewHandler.CheckParameters(handler, search);

			handler.TypedNativeView?.UpdateTextColor(search);
		}

		public static void MapTextTransform(SearchBarHandler handler, ISearch search)
		{
			ViewHandler.CheckParameters(handler, search);

			handler.TypedNativeView?.UpdateTextTransform(search);
		}

		public static void MapCharacterSpacing(SearchBarHandler handler, ISearch search)
		{
			ViewHandler.CheckParameters(handler, search);

			handler.TypedNativeView?.UpdateCharacterSpacing(search);
		}

		public static void MapPlaceholder(SearchBarHandler handler, ISearch search)
		{
			ViewHandler.CheckParameters(handler, search);

			handler.TypedNativeView?.UpdatePlaceholder(search);
		}

		public static void MapPlaceholderColor(SearchBarHandler handler, ISearch search)
		{
			ViewHandler.CheckParameters(handler, search);

			handler.TypedNativeView?.UpdatePlaceholderColor(search);
		}

		public static void MapFontAttributes(SearchBarHandler handler, ISearch search)
		{
			ViewHandler.CheckParameters(handler, search);

			handler.TypedNativeView?.UpdateFontAttributes(search);
		}

		public static void MapFontFamily(SearchBarHandler handler, ISearch search)
		{
			ViewHandler.CheckParameters(handler, search);

			handler.TypedNativeView?.UpdateFontFamily(search);
		}

		public static void MapFontSize(SearchBarHandler handler, ISearch search)
		{
			ViewHandler.CheckParameters(handler, search);

			handler.TypedNativeView?.UpdateFontSize(search);
		}

		public static void MapHorizontalTextAlignment(SearchBarHandler handler, ISearch search)
		{
			ViewHandler.CheckParameters(handler, search);

			handler.TypedNativeView?.UpdateHorizontalTextAlignment(search);
		}

		public static void MapVerticalTextAlignment(SearchBarHandler handler, ISearch search)
		{
			ViewHandler.CheckParameters(handler, search);

			handler.TypedNativeView?.UpdateVerticalTextAlignment(search);
		}
	}
}
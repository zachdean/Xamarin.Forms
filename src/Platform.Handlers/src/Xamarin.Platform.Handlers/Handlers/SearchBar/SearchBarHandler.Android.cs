using System.Linq;
using Android.Widget;

namespace Xamarin.Platform.Handlers
{
	public partial class SearchBarHandler : AbstractViewHandler<ISearch, SearchView>
	{
		static EditText? EditText;

		QueryTextListener TextListener { get; } = new QueryTextListener();

		protected override SearchView CreateNativeView()
		{
			var searchView = new SearchView(Context);

			searchView.SetIconifiedByDefault(false);

			EditText = searchView.GetChildrenOfType<EditText>().FirstOrDefault();

			return searchView;
		}

		protected override void ConnectHandler(SearchView nativeView)
		{
			TextListener.Handler = this;
			nativeView.SetOnQueryTextListener(TextListener);
		}

		protected override void DisconnectHandler(SearchView nativeView)
		{
			TextListener.Handler = null;
			nativeView.SetOnQueryTextListener(null);
		}

		protected override void SetupDefaults(SearchView nativeView)
		{
			EditText ??= nativeView.GetChildrenOfType<EditText>().FirstOrDefault();
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

			handler.TypedNativeView?.UpdateCharacterSpacing(EditText, search);
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

			handler.TypedNativeView?.UpdateFontAttributes(EditText, search);
		}

		public static void MapFontFamily(SearchBarHandler handler, ISearch search)
		{
			ViewHandler.CheckParameters(handler, search);

			handler.TypedNativeView?.UpdateFontFamily(EditText, search);
		}

		public static void MapFontSize(SearchBarHandler handler, ISearch search)
		{
			ViewHandler.CheckParameters(handler, search);

			handler.TypedNativeView?.UpdateFontSize(EditText, search);
		}
				
		public static void MapMaxLength(SearchBarHandler handler, ISearch search)
		{
			ViewHandler.CheckParameters(handler, search);

			handler.TypedNativeView?.UpdateMaxLength(EditText, search);
		}

		public static void MapKeyboard(SearchBarHandler handler, ISearch search)
		{
			ViewHandler.CheckParameters(handler, search);

			handler.TypedNativeView?.UpdateKeyboard(search);
		}

		public static void MapIsSpellCheckEnabled(SearchBarHandler handler, ISearch search)
		{
			ViewHandler.CheckParameters(handler, search);

			handler.TypedNativeView?.UpdateIsSpellCheckEnabled(search);
		}

		public static void MapHorizontalTextAlignment(SearchBarHandler handler, ISearch search)
		{
			ViewHandler.CheckParameters(handler, search);

			handler.TypedNativeView?.UpdateHorizontalTextAlignment(EditText, search);
		}

		public static void MapVerticalTextAlignment(SearchBarHandler handler, ISearch search)
		{
			ViewHandler.CheckParameters(handler, search);

			handler.TypedNativeView?.UpdateVerticalTextAlignment(EditText, search);
		}

		public class QueryTextListener : Java.Lang.Object, SearchView.IOnQueryTextListener
		{
			public SearchBarHandler? Handler { get; set; }

			public bool OnQueryTextChange(string? newText)
			{
				return true;
			}

			public bool OnQueryTextSubmit(string? query)
			{
				return true;
			}
		}
	}
}
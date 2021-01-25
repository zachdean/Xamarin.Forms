using System.Linq;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;

namespace Xamarin.Platform.Handlers
{
	public partial class SearchBarHandler : AbstractViewHandler<ISearch, SearchView>
	{
		static float DefaultHeight => 42.0f;

		static TextColorSwitcher? TextColorSwitcher;
		static TextColorSwitcher? HintColorSwitcher;

		static EditText? EditText;

		QueryTextListener TextListener { get; } = new QueryTextListener();

		FocusChangeListener FocusListener { get; } = new FocusChangeListener();

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

			FocusListener.Handler = this;
			nativeView.SetOnQueryTextFocusChangeListener(FocusListener);
		}

		protected override void DisconnectHandler(SearchView nativeView)
		{
			TextListener.Handler = null;
			nativeView.SetOnQueryTextListener(null);

			FocusListener.Handler = null;
			nativeView.SetOnQueryTextFocusChangeListener(null);
		}

		protected override void SetupDefaults(SearchView nativeView)
		{
			EditText ??= nativeView.GetChildrenOfType<EditText>().FirstOrDefault();

			if (EditText != null)
			{
				TextColorSwitcher = new TextColorSwitcher(EditText.TextColors);
				HintColorSwitcher = new TextColorSwitcher(EditText.HintTextColors);
			}
		}

		public override Size GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			var size = base.GetDesiredSize(widthConstraint, heightConstraint);

			if (NativeVersion.IsAtLeast(24) && heightConstraint == 0 && size.Height == 0)
			{
				size = new Size(size.Width, Context?.ToPixels(DefaultHeight) ?? size.Height);
			}

			return size;
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

			handler.TypedNativeView?.UpdateTextColor(TextColorSwitcher, EditText, search);
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

			handler.TypedNativeView?.UpdatePlaceholderColor(HintColorSwitcher, EditText, search);
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

			handler.TypedNativeView?.UpdateKeyboard(EditText, search);
		}

		public static void MapIsSpellCheckEnabled(SearchBarHandler handler, ISearch search)
		{
			ViewHandler.CheckParameters(handler, search);

			handler.TypedNativeView?.UpdateIsSpellCheckEnabled(EditText, search);
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

		internal void ClearFocus(SearchView view)
		{
			view.ClearFocus();
		}

		public class QueryTextListener : Java.Lang.Object, SearchView.IOnQueryTextListener
		{
			public SearchBarHandler? Handler { get; set; }

			public bool OnQueryTextChange(string? newText)
			{
				TextTransformUtilites.SetPlainText(Handler?.VirtualView, newText ?? string.Empty);
				return true;
			}

			public bool OnQueryTextSubmit(string? query)
			{
				Handler?.VirtualView?.SearchButtonPressed();
				Handler?.TypedNativeView?.ClearFocus();
				return true;
			}
		}

		public class FocusChangeListener : Java.Lang.Object, View.IOnFocusChangeListener
		{
			public SearchBarHandler? Handler { get; set; }

			public void OnFocusChange(View? v, bool hasFocus)
			{
				// TODO: Port KeyboardManager
			}
		}
	}
}
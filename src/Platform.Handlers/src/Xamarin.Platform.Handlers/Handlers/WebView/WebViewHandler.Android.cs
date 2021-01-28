using Android.Webkit;
using Android.Widget;
using Xamarin.Forms;
using static Android.Views.ViewGroup;
using AWebView = Android.Webkit.WebView;

namespace Xamarin.Platform.Handlers
{
	public partial class WebViewHandler : AbstractViewHandler<IWebView, AWebView>, IWebViewDelegate2
	{
		public const string AssetBaseUrl = "file:///android_asset/";

		WebViewClient? _webViewClient;
		WebChromeClient? _webChromeClient;

		protected override AWebView CreateNativeView()
		{
			var aWebView = new AWebView(Context)
			{
#pragma warning disable 618 // This can probably be replaced with LinearLayout(LayoutParams.MatchParent, LayoutParams.MatchParent); just need to test that theory
				LayoutParameters = new AbsoluteLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent, 0, 0)
#pragma warning restore 618
			};

			if (aWebView.Settings != null)
			{
				aWebView.Settings.JavaScriptEnabled = true;
				aWebView.Settings.DomStorageEnabled = true;
			}

			_webViewClient = GetWebViewClient();
			aWebView.SetWebViewClient(_webViewClient);

			_webChromeClient = GetWebChromeClient();
			aWebView.SetWebChromeClient(_webChromeClient);

			return aWebView;
		}

		protected override void DisconnectHandler(AWebView nativeView)
		{
			nativeView.StopLoading();
			_webViewClient?.Dispose();
			_webChromeClient?.Dispose();
		}

		public static void MapSource(WebViewHandler handler, IWebView webView)
		{
			ViewHandler.CheckParameters(handler, webView);

			IWebViewDelegate2 webViewDelegate = handler;

			handler.TypedNativeView?.UpdateSource(webView, webViewDelegate);
		}

		public static void MapCanGoBack(WebViewHandler handler, IWebView webView)
		{
			ViewHandler.CheckParameters(handler, webView);

			handler.TypedNativeView?.UpdateCanGoBack(webView);
		}

		public static void MapCanGoForward(WebViewHandler handler, IWebView webView)
		{
			ViewHandler.CheckParameters(handler, webView);

			handler.TypedNativeView?.UpdateCanGoForward(webView);
		}

		public void LoadHtml(string? html, string? baseUrl)
		{
			TypedNativeView?.LoadDataWithBaseURL(baseUrl ?? AssetBaseUrl, html, "text/html", "UTF-8", null);
		}

		public void LoadUrl(string? url)
		{
			TypedNativeView?.LoadUrl(url);
		}

		protected virtual WebViewClient GetWebViewClient()
		{
			return new WebViewClient();
		}

		protected virtual WebChromeClient GetWebChromeClient()
		{
			return new WebChromeClient();
		}
	}
}
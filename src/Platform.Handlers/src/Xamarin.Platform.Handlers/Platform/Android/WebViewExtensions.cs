using Xamarin.Forms;
using AWebView = Android.Webkit.WebView;

namespace Xamarin.Platform
{
	public static class WebViewExtensions
	{
		public static void UpdateSource(this AWebView nativeWebView, IWebView webView)
		{
			nativeWebView.UpdateSource(webView, null);
		}

		public static void UpdateSource(this AWebView nativeWebView, IWebView webView, IWebViewDelegate2? webViewDelegate)
		{
			if (webViewDelegate != null)
				webView.Source.Load(webViewDelegate);

			nativeWebView.UpdateCanGoBackForward(webView);
		}

		public static void UpdateCanGoBack(this AWebView nativeWebView, IWebView webView)
		{
			webView.CanGoBack = nativeWebView.CanGoBack();
		}

		public static void UpdateCanGoForward(this AWebView nativeWebView, IWebView webView)
		{
			webView.CanGoForward = nativeWebView.CanGoForward();
		}

		internal static void UpdateCanGoBackForward(this AWebView nativeWebView, IWebView webView)
		{
			nativeWebView.UpdateCanGoBack(webView);
			nativeWebView.UpdateCanGoForward(webView);
		}
	}
}

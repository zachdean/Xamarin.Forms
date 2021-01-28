using WebKit;
using Xamarin.Forms;

namespace Xamarin.Platform
{
	public static class WebViewExtensions
	{
		public static void UpdateSource(this WKWebView nativeWebView, IWebView webView)
		{
			nativeWebView.UpdateSource(webView, null);
		}

		public static void UpdateSource(this WKWebView nativeWebView, IWebView webView, IWebViewDelegate2? webViewDelegate)
		{
			if (webViewDelegate != null)
				webView.Source.Load(webViewDelegate);

			nativeWebView.UpdateCanGoBackForward(webView);
		}

		public static void UpdateCanGoBack(this WKWebView nativeWebView, IWebView webView)
		{
			webView.CanGoBack = nativeWebView.CanGoBack;
		}

		public static void UpdateCanGoForward(this WKWebView nativeWebView, IWebView webView)
		{
			webView.CanGoForward = nativeWebView.CanGoForward;
		}

		internal static void UpdateCanGoBackForward(this WKWebView nativeWebView, IWebView webView)
		{
			nativeWebView.UpdateCanGoBack(webView);
			nativeWebView.UpdateCanGoForward(webView);
		}
	}
}
using CoreGraphics;
using WebKit;
using Xamarin.Forms;

namespace Xamarin.Platform.Handlers
{
	public partial class WebViewHandler : AbstractViewHandler<IWebView, WKWebView>, IWebViewDelegate2
	{
		protected override WKWebView CreateNativeView()
		{
			return new WKWebView(CGRect.Empty, CreateConfiguration());
		}

		public static void MapSource(WebViewHandler handler, IWebView webView)
		{
			ViewHandler.CheckParameters(handler, webView);

			handler.TypedNativeView?.UpdateSource(webView);
		}

		public void LoadHtml(string? html, string? baseUrl)
		{
			throw new System.NotImplementedException();
		}

		public void LoadUrl(string? url)
		{
			throw new System.NotImplementedException();
		}

		// https://developer.apple.com/forums/thread/99674
		// WKWebView and making sure cookies synchronize is really quirky
		// The main workaround I've found for ensuring that cookies synchronize 
		// is to share the Process Pool between all WkWebView instances.
		// It also has to be shared at the point you call init
		static WKWebViewConfiguration CreateConfiguration()
		{
			var config = new WKWebViewConfiguration();

			return config;
		}
	}
}
using Xamarin.Forms;
using AWebView = Android.Webkit.WebView;

namespace Xamarin.Platform.Handlers
{
	public partial class WebViewHandler : AbstractViewHandler<IWebView, AWebView>, IWebViewDelegate
	{
		protected override AWebView CreateNativeView()
		{
			var webView = new AWebView(Context);
			webView.Settings?.SetSupportMultipleWindows(true);
			return webView;
		}

		public void LoadHtml(string? html, string baseUrl)
		{
			throw new System.NotImplementedException();
		}

		public void LoadUrl(string? url)
		{
			throw new System.NotImplementedException();
		}
	}
}
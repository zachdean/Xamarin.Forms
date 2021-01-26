using WebKit;
using Xamarin.Forms;

namespace Xamarin.Platform.Handlers
{
	public partial class WebViewHandler : AbstractViewHandler<IWebView, WKWebView>, IWebViewDelegate
	{
		protected override WKWebView CreateNativeView()
		{
			return new WKWebView(CreateConfiguration());
		}

		public void LoadHtml(string? html, string baseUrl)
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

			if (_sharedPool == null)
			{
				_sharedPool = config.ProcessPool;
			}
			else
			{
				config.ProcessPool = _sharedPool;
			}

			return config;
		}


	}
}
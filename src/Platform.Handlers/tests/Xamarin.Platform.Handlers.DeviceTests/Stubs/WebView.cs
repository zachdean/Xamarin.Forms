using Xamarin.Forms;

namespace Xamarin.Platform.Handlers.DeviceTests.Stubs
{
	public class WebViewStub : StubBase, IWebView
	{
		public WebViewSource2 Source { get; set; }
		public bool CanGoBack { get; set; }
		public bool CanGoForward { get; set; }
	}
}
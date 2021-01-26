using Xamarin.Forms;

namespace Xamarin.Platform
{
	public interface IWebView : IView
	{
		WebViewSource Source { get; }
	}
}
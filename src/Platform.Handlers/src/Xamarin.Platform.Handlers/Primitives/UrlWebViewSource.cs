using System.ComponentModel;

namespace Xamarin.Forms
{
	public class UrlWebViewSource : WebViewSource
	{
		public string? Url { get; set; }

		[EditorBrowsable(EditorBrowsableState.Never)]
		public override void Load(IWebViewDelegate renderer)
		{
			renderer.LoadUrl(Url);
		}
	}
}
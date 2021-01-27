using System.ComponentModel;

namespace Xamarin.Forms
{
	public class UrlWebViewSource2 : WebViewSource2
	{
		public string? Url { get; set; }

		[EditorBrowsable(EditorBrowsableState.Never)]
		public override void Load(IWebViewDelegate2 renderer)
		{
			renderer.LoadUrl(Url);
		}
	}
}
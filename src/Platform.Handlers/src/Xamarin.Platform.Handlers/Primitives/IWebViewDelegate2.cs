namespace Xamarin.Forms
{
	public interface IWebViewDelegate2
	{
		void LoadHtml(string? html, string? baseUrl);
		void LoadUrl(string? url);
	}
}
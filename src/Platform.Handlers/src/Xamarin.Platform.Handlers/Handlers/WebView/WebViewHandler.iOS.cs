using System;
using System.IO;
using Foundation;
using WebKit;
using Xamarin.Forms;
using RectangleF = CoreGraphics.CGRect;

namespace Xamarin.Platform.Handlers
{
	public partial class WebViewHandler : AbstractViewHandler<IWebView, WKWebView>, IWebViewDelegate2
	{
		protected override WKWebView CreateNativeView()
		{
			return new WKWebView(RectangleF.Empty, new WKWebViewConfiguration());
		}

		public override Size GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			SetDesiredSize(widthConstraint, heightConstraint);

			return base.GetDesiredSize(widthConstraint, heightConstraint);
		}

		public static void MapSource(WebViewHandler handler, IWebView webView)
		{
			ViewHandler.CheckParameters(handler, webView);

			IWebViewDelegate2 webViewDelegate = handler;

			handler.TypedNativeView?.UpdateSource(webView, webViewDelegate);
		}

		public static void MapCanGoBack(WebViewHandler handler, IWebView webView)
		{
			ViewHandler.CheckParameters(handler, webView);

			handler.TypedNativeView?.UpdateCanGoBack(webView);
		}

		public static void MapCanGoForward(WebViewHandler handler, IWebView webView)
		{
			ViewHandler.CheckParameters(handler, webView);

			handler.TypedNativeView?.UpdateCanGoForward(webView);
		}

		public void LoadHtml(string? html, string? baseUrl)
		{
			if (html != null)
				TypedNativeView?.LoadHtmlString(html, baseUrl == null ? new NSUrl(NSBundle.MainBundle.BundlePath, true) : new NSUrl(baseUrl, true));
		}

		public void LoadUrl(string? url)
		{
			try
			{
				var uri = new Uri(url);
				var safeHostUri = new Uri($"{uri.Scheme}://{uri.Authority}", UriKind.Absolute);
				var safeRelativeUri = new Uri($"{uri.PathAndQuery}{uri.Fragment}", UriKind.Relative);
				NSUrlRequest request = new NSUrlRequest(new Uri(safeHostUri, safeRelativeUri));

				TypedNativeView?.LoadRequest(request);
			}
			catch (UriFormatException formatException)
			{
				// If we got a format exception trying to parse the URI, it might be because
				// someone is passing in a local bundled file page. If we can find a better way
				// to detect that scenario, we should use it; until then, we'll fall back to 
				// local file loading here and see if that works:
				if (!LoadFile(url))
				{
					Log.Warning(nameof(WebViewHandler), $"Unable to Load Url {url}: {formatException}");
				}
			}
			catch (Exception exc)
			{
				Log.Warning(nameof(WebViewHandler), $"Unable to Load Url {url}: {exc}");
			}
		}

		bool LoadFile(string? url)
		{
			try
			{
				var file = Path.GetFileNameWithoutExtension(url);
				var ext = Path.GetExtension(url);

				var nsUrl = NSBundle.MainBundle.GetUrlForResource(file, ext);

				if (nsUrl == null)
				{
					return false;
				}

				TypedNativeView?.LoadFileUrl(nsUrl, nsUrl);

				return true;
			}
			catch (Exception ex)
			{
				Log.Warning(nameof(WebViewHandler), $"Could not load {url} as local file: {ex}");
			}

			return false;
		}

		void SetDesiredSize(double width, double height)
		{
			if (TypedNativeView != null)
			{
				var x = TypedNativeView.Frame.X;
				var y = TypedNativeView.Frame.Y;

				TypedNativeView.Frame = new RectangleF(x, y, width, height);
			}
		}
	}
}
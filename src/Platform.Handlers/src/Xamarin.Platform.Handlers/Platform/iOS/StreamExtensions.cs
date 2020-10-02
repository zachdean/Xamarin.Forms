using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Platform
{
	public static class StreamExtensions
	{
		public static async Task<Stream?> GetStreamAsync(Uri uri, CancellationToken cancellationToken)
		{
			using (var client = GetHttpClient())
			{
				// Do not remove this await otherwise the client will dispose before
				// the stream even starts

				HttpResponseMessage response = await client.GetAsync(uri, cancellationToken).ConfigureAwait(false);
				if (!response.IsSuccessStatusCode)
				{
					System.Diagnostics.Debug.WriteLine("HTTP Request", $"Could not retrieve {uri}, status code {response.StatusCode}");
					return null;
				}

				var result = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

				return result;
			}
		}

		static HttpClient GetHttpClient()
		{
			var proxy = CoreFoundation.CFNetwork.GetSystemProxySettings();
			var handler = new HttpClientHandler();
			if (!string.IsNullOrEmpty(proxy.HTTPProxy))
			{
				handler.Proxy = CoreFoundation.CFNetwork.GetDefaultProxy();
				handler.UseProxy = true;
			}
			return new HttpClient(handler);
		}
	}
}

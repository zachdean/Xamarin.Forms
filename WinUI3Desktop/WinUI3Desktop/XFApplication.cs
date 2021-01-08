using System.IO;
using Microsoft.Extensions.Hosting;
using Microsoft.MobileBlazorBindings;
using Microsoft.Extensions.DependencyInjection;

namespace WinUI3Desktop
{
	public class XFApplication : Xamarin.Forms.Application
	{
		public XFApplication() : base()
		{
			var appName = "WinUI3Desktop"; // TODO: How to get this dynamically?
			var appRoot = Path.Combine(Windows.ApplicationModel.Package.Current.InstalledLocation.Path, appName);

			var hostBuilder = MobileBlazorBindingsHost2.CreateDefaultBuilder()
				.UseContentRoot(appRoot)
				.ConfigureServices((hostContext, services) =>
				{
					// Adds web-specific services such as NavigationManager
					services.AddBlazorHybrid();

					// Register app-specific services
					//services.AddSingleton<CounterState>();
				})
				.UseWebRoot("wwwroot");
			hostBuilder.UseStaticFiles();
			var host = hostBuilder.Build();

			MainPage = new Xamarin.Forms.ContentPage()
			{
				Content = new Xamarin.Forms.StackLayout()
				{
					Children =
						{
							new Xamarin.Forms.Label(){ Text = "Hello There"},
							new Microsoft.MobileBlazorBindings.WebView.Elements.BlazorWebView<BlazorDesktopApp.WebUI.MessageList>
							{
								HeightRequest = 500,
								WidthRequest = 500,
								Host = host,
							}
						}
				}
			};
		}
	}

	public static class MobileBlazorBindingsHost2
	{
		public static IHostBuilder CreateDefaultBuilder(string[] args = null)
		{
			// Inspired by Microsoft.Extensions.Hosting.Host, which can be seen here:
			// https://github.com/dotnet/runtime/blob/master/src/libraries/Microsoft.Extensions.Hosting/src/Host.cs
			// But slightly modified to work on all of Android, iOS, and UWP.

			var builder = new HostBuilder();

			builder.UseContentRoot(Directory.GetCurrentDirectory());
			builder.UseWebRoot("wwwroot");
			builder.UseDefaultServiceProvider((context, options) =>
			{
				var isDevelopment = context.HostingEnvironment.IsDevelopment();
				options.ValidateScopes = isDevelopment;
				options.ValidateOnBuild = isDevelopment;
			});

			return builder;
		}
	}
}

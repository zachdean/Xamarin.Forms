using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sample.Pages;
using Sample.Services;
using Sample.ViewModel;
using Xamarin.Platform.Core;
using Xamarin.Platform.Hosting;

namespace Sample
{
	public class MyApp : Xamarin.Platform.App
	{
		public void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
		{
			services.AddSingleton<ITextService, TextService>();
			services.AddTransient<MainPageViewModel>();
			services.AddTransient<MainPage>();
			services.AddTransient<IWindow, MainWindow>();
		}
	}
}
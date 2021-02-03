using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sample.Pages;
using Sample.Services;
using Sample.ViewModel;
using Xamarin.Platform;

namespace Sample
{
	public class MyApp : App
	{
		public void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
		{
			services.AddSingleton<ITextService, TextService>();
			services.AddTransient<MainPageViewModel>();
			services.AddTransient<MainPage>();
			services.AddTransient<IWindow, MainWindow>();
		}


		//Uncomment if you don't use DI
		//public override IEnumerable<IWindow> Windows => new IWindow[1] { Platform.GetWindow() };
	}
}
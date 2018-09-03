using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Xamarin.Forms;
using Xamarin.Forms.ControlGallery.WindowsUniversal;
using Xamarin.Forms.Controls;
using Xamarin.Forms.Platform.UWP;

[assembly: Dependency(typeof(WindowNavigation))]
namespace Xamarin.Forms.ControlGallery.WindowsUniversal
{
	public class WindowNavigation : IWindowNavigation
	{
		public async Task<Guid> OpenNewWindowAsync()
		{
			var currentViewId = ApplicationView.GetForCurrentView();
			var newAV = CoreApplication.CreateNewView();
			var _windowId = Guid.Empty;

			await newAV.Dispatcher.RunAsync(
							CoreDispatcherPriority.Normal,
							async () =>
							{
								var newWindow = Window.Current;
								var newAppView = ApplicationView.GetForCurrentView();
								Windows.UI.Xaml.Controls.Frame frameContent = new Windows.UI.Xaml.Controls.Frame();

								//Create new app to get WindowId
								var app = new Xamarin.Forms.Controls.App();
								_windowId = app.WindowId;

								newWindow.Content = frameContent;								
								newWindow.Activate();

								frameContent.Navigate(typeof(SecondPage), app);

								await ApplicationViewSwitcher.TryShowAsStandaloneAsync(
									newAppView.Id,
									ViewSizePreference.Default,
									currentViewId.Id,
									ViewSizePreference.Default);
							});

			return _windowId;
		}
	}
}

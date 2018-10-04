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
		[ThreadStatic]
		static EventHandler<Page> _updatePage;

		public void NavegateToAnotherPage(Page page)
		{
			_updatePage?.Invoke(null, page);
		}

		public async Task OpenNewWindowAsync()
		{
			var currentViewId = ApplicationView.GetForCurrentView();
			var newAV = CoreApplication.CreateNewView();

			await newAV.Dispatcher.RunAsync(
							CoreDispatcherPriority.Normal,
							async () =>
							{
								var newWindow = Window.Current;
								var newAppView = ApplicationView.GetForCurrentView();
								Windows.UI.Xaml.Controls.Frame frameContent = new Windows.UI.Xaml.Controls.Frame();
								newWindow.Content = frameContent;
								newWindow.Activate();

								frameContent.Navigate(typeof(SecondPage), new Controls.ControlGalleryPages.SecondPage());

								_updatePage += (sender, args) =>
								{
									frameContent.Navigate(typeof(SecondPage), args);
								};

								await ApplicationViewSwitcher.TryShowAsStandaloneAsync(
									newAppView.Id,
									ViewSizePreference.Default,
									currentViewId.Id,
									ViewSizePreference.Default);
							});
		}
	}
}

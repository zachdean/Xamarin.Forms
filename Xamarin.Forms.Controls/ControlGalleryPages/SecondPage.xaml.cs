using System;
using System.Threading.Tasks;
using Xamarin.Forms.Xaml;

namespace Xamarin.Forms.Controls.ControlGalleryPages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SecondPage : ContentPage
	{
		public SecondPage()
		{
			InitializeComponent();

			var labelRunsBackground = new Label() { Text = "This should start updating with the time in a few seconds" };
			stkTest.Children.Add(labelRunsBackground);

			Device.StartTimer(TimeSpan.FromSeconds(1), () =>
			{
				labelRunsBackground.Dispatcher.BeginInvokeOnMainThread(() => labelRunsBackground.Text = DateTime.Now.ToString("HH:mm:ss"));
				return true;
			});

			var threadpoolButton = new Button { Text = "Update Instructions from Thread Pool" };
			stkTest.Children.Add(threadpoolButton);


			threadpoolButton.Clicked += (o, a) => {
				Task.Run(() => {
					lblTest.Dispatcher.BeginInvokeOnMainThread(() => { lblTest.Text = "updated from thread pool 2"; });
				});
			};

		}

		private void Button_Clicked(object sender, EventArgs e)
		{
			DependencyService.Get<IWindowNavigation>().NavigateToAnotherPage(new ContentPage());
		}
	}
}
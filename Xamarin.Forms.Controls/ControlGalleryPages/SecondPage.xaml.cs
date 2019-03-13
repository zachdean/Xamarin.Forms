using System;
using Xamarin.Forms.Xaml;

namespace Xamarin.Forms.Controls.ControlGalleryPages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SecondPage : ContentPage
	{
		public SecondPage()
		{
			InitializeComponent();
		}

		private void Button_Clicked(object sender, EventArgs e)
		{
			DependencyService.Get<IWindowNavigation>().NavigateToAnotherPage(new ContentPage());
		}
	}
}
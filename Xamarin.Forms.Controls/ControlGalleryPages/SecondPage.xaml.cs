using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xamarin.Forms.Controls.ControlGalleryPages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SecondPage : MasterDetailPage
	{
		public SecondPage()
		{
			InitializeComponent();

			var layout = new StackLayout { BackgroundColor = Color.Red };
			layout.Children.Add(new Label { Text = "This is master Page" });
			var master = new ContentPage { Title = "Master", Content = layout, BackgroundColor = Color.SkyBlue };

			Master = master;
			Detail = CoreGallery.GetMainPage();

		}
	}
}
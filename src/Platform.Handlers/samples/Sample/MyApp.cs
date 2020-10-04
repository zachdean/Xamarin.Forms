using System.Reflection;
using Xamarin.Forms;
using Xamarin.Platform;
using Xamarin.Platform.Core;

namespace Sample
{
	public class MyApp : IApp
	{
		public MyApp()
		{
			Platform.Init();
		}

		public IView CreateView()
		{

			var image = new Xamarin.Forms.Image();
			//From URI with cache
			//image.Source = "https://via.placeholder.com/300.png/09f/fff";
			//Native
			image.Source = "fruits.jpg";
			//From Resource
			//image.Source = Xamarin.Forms.FileImageSource.FromResource("Sample.Images.xamarinlogo.png", typeof(MyApp).GetTypeInfo().Assembly);
			return image;


			//return new Button() { Text = "Hello I'm a button"};
		}
	}
}
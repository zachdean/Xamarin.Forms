using Xamarin.Platform;

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
			return new Entry { Text = "Text" };
		}
	}
}
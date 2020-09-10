namespace System.Maui.Controls
{
	public class App : IApp
	{
		static App()
		{
			Platform.Init();
		}

		public IView MainPage { get; set; }
	}
}
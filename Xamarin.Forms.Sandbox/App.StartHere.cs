namespace Xamarin.Forms.Sandbox
{
	public partial class App 
	{
		// This code is called from the App Constructor so just initialize the main page of the application here
		void InitializeMainPage()
		{
			MainPage = CreateStackLayoutPage(new View[] {
				new Button(){ Text = "test", VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.Center },
				new Button(){ Text = "test", HeightRequest = 20, HorizontalOptions = LayoutOptions.Center },
				new Editor() { Text = "text", HeightRequest = 200 } });

			MainPage.Visual = VisualMarker.Material;
		}
	}
}

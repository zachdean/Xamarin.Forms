using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Forms.Sandbox
{
	public partial class App 
	{
		// This code is called from the App Constructor so just initialize the main page of the application here
		void InitializeMainPage()
		{
			var button = new Button() { Text = "text" };
			Material.Button.SetStyle(button, Material.Style.Outline);
			MainPage = new ContentPage()
			{
				Content = CreateStackLayout(new[] { button }),
				Visual = VisualMarker.Material
			};
		}
	}
}

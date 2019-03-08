using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Xamarin.Forms.Sandbox
{
	public partial class App 
	{
		// This code is called from the App Constructor so just initialize the main page of the application here
		void InitializeMainPage()
		{
			//Func<View> create = null;
			//create = () =>
			//{
			//	Button button = null;
			//	button = new Button()
			//	{
			//		Text = "text"
			//	};

			//	Material.Button.SetStyle(button, Material.Style.Outline);
			//	return button;
			//};

			//MainPage = CreateListViewPage(create);
			//MainPage.Visual = VisualMarker.Material;

			MainPage = new MainPage();
		}
	}
}

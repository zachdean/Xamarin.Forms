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
			Func<View> create = null;
			create = ()=>
			{
				Button button = null;
				button = new Button()
				{
					Text = "text",
					Command = new Command(() =>
					{
						if(Material.Button.GetStyle(button) == Material.Style.Filled)
							Material.Button.SetStyle(button, Material.Style.Outline);
						else
							Material.Button.SetStyle(button, Material.Style.Filled);

					})
				};

				return button;
			};

			MainPage = CreateListViewPage(create);

			MainPage.Visual = VisualMarker.Material;
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUI3Desktop
{
	public class XFApplication : Xamarin.Forms.Application
	{
		public XFApplication() : base()
		{
			MainPage = new Xamarin.Forms.ContentPage()
			{
				Content = new Xamarin.Forms.StackLayout()
				{
					Children =
						{
							new Xamarin.Forms.Label(){ Text = "Hello There"},
							new Xamarin.Forms.WebView()
							{
								HeightRequest = 500,
								WidthRequest = 500,
								Source = "http://www.microsoft.com"
							}
						}
				}
			};
		}
	}
}

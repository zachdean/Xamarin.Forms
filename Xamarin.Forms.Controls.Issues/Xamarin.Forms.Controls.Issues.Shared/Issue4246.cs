using System;
using Xamarin.Forms.CustomAttributes;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Controls.Issues
{
	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Github, 4246, "XF 3.3 sets TabbedPage color based on NavigationPage.BarBackgroundColorProperty", PlatformAffected.Android | PlatformAffected.iOS)]
	public class Issue4246 : TestTabbedPage
	{
		protected override void Init()
		{
			Children.Add(new NavigationPage(new ContentPage())
			{
				Title = "1"
			});

			Children.Add(new NavigationPage(new ContentPage())
			{
				Title = "2"
			});

			Children.Add(new NavigationPage(new ContentPage())
			{
				Title = "3"
			});

			Children.Add(new NavigationPage(new ContentPage())
			{
				Title = "4"
			});

			SetValue(NavigationPage.BarTextColorProperty, Color.Black);
			SetValue(NavigationPage.BarBackgroundColorProperty, Color.Pink);
		}
	}
}
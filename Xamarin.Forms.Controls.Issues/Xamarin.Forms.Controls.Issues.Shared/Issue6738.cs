using Xamarin.Forms.CustomAttributes;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Controls;
using System;

#if UITEST
using Xamarin.Forms.Core.UITests;
using Xamarin.UITest;
using NUnit.Framework;
#endif

namespace Xamarin.Forms.Controls.Issues
{
#if UITEST
	[Category(UITestCategories.Shell)]
#endif
	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Github, 6738, "Flyout Navigation fails when coupled with tabs that have a stack", PlatformAffected.Android)]
	public class Issue6738 : TestShell
	{
		Tab tabOne = new Tab { Title = "One" };
		Tab tabTwo = new Tab { Title = "Two " };
		Tab flyoutContent = new Tab();
		Button pushPageButton = new Button { Text = "Tap to push new page to stack" };

		void OnPushTapped(object sender, EventArgs e)
		{
			Navigation.PushAsync(new ContentPage { Content = new Label { Text = "If this is the second time you have been to this page, the test has passed. Otherwise, go to tab two now." } });
		}

		protected override void Init()
		{
			pushPageButton.Clicked += OnPushTapped;
			tabOne.Items.Add(new ContentPage { Content = pushPageButton });
			tabTwo.Items.Add(new ContentPage { Content = new Label { Text = "If you've been here already, go to tab one now. Otherwise, go to Other Page in the flyout." } });
			flyoutContent.Items.Add(new ContentPage { Content = new Label { Text = "Go back to main page via the flyout" } });

			Items.Add(
					new FlyoutItem
					{
						Title = "Main",
						Items = { tabOne, tabTwo }
					}
			);
			Items.Add(new FlyoutItem {
				Title = "Other Page",
				Items = { flyoutContent }
			});
		}

#if UITEST
#if !(__ANDROID__ || __IOS__)
		[Ignore("Shell test is only supported on Android and iOS")]
#endif
		[Test]
		public void Issue6738Test()
		{
		}
#endif
	}
}
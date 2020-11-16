using Xamarin.Forms.CustomAttributes;
using Xamarin.Forms.Internals;

#if UITEST
using Xamarin.Forms.Core.UITests;
using Xamarin.UITest;
using NUnit.Framework;
#endif

namespace Xamarin.Forms.Controls.Issues
{
#if UITEST
	[Category(UITestCategories.ManualReview)]
#endif
	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Github, 8988, "App freezes on iPadOS 13.3 when in split view mode (multi-tasking) and change between pages", PlatformAffected.iOS)]
	public class Issue8988 : TestContentPage // or TestFlyoutPage, etc ...
	{
		SecondPage secondPage;

		protected override void Init()
		{
			secondPage = new SecondPage();
			var layout = new StackLayout();
			var label = new Label
			{
				Text = "Welcome to Xamarin Forms!",
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
			};
			var button = new Button
			{
				Text = "NextPage",
				Command = new Command(async () =>
				{
					await Navigation.PushModalAsync(secondPage, false);
				})

			};

			layout.Children.Add(label);
			layout.Children.Add(button);

			BackgroundColor = Color.YellowGreen;
			Content = layout;
		}

#if UITEST
		[Test]
		public void Issue8988Test()
		{
			RunningApp.WaitForElement("Issue1Label");
			// Delete this and all other UITEST sections if there is no way to automate the test. Otherwise, be sure to rename the test and update the Category attribute on the class. Note that you can add multiple categories.
			RunningApp.Screenshot("I am at Issue1");
			RunningApp.WaitForElement(q => q.Marked("Issue1Label"));
			RunningApp.Screenshot("I see the Label");
		}
#endif
	}

	class SecondPage : ContentPage
	{
		public SecondPage()
		{
			var layout = new StackLayout();
			var label = new Label
			{
				Text = "This is the Second Page!",
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
			};
			var button = new Button
			{
				Text = "Go Back to Main Page",
				Command = new Command(() => Navigation.PopModalAsync(false))
			};

			layout.Children.Add(label);
			layout.Children.Add(button);

			BackgroundColor = Color.Yellow;

			Content = layout;
		}
	}
}
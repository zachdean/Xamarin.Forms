using Xamarin.Forms.CustomAttributes;
using Xamarin.Forms.Internals;

#if UITEST
using Xamarin.UITest;
using NUnit.Framework;
using Xamarin.Forms.Core.UITests;
#endif

namespace Xamarin.Forms.Controls.Issues
{
#if UITEST
	[Category(UITestCategories.Navigation)]
#endif
	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Github, 11138,
		"[Bug] iOS Translucent not longer works with UIAppearance",
		PlatformAffected.iOS)]
	public class Issue11138 : TestContentPage
	{
		public Issue11138()
		{
		}

		protected override void Init()
		{
			Title = "Issue 11138";

			var layout = new StackLayout
			{
				BackgroundColor = Color.LightGray
			};

			var navigateButton = new Button
			{
				Text = "Navigate"
			};

			layout.Children.Add(navigateButton);

			Content = layout;

			navigateButton.Clicked += (sender, args) =>
			{
				Navigation.PushAsync(new Issue11138NavigationPage(new Issue11138()));
			};
		}
	}

	[Preserve(AllMembers = true)]
	public class Issue11138NavigationPage : NavigationPage
	{
		public Issue11138NavigationPage()
		{

		}

		public Issue11138NavigationPage(Page root) : base(root)
		{

		}
	}
}
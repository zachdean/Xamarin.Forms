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
	[Issue(IssueTracker.Github, 11703, "Android textAllCaps no longer works", PlatformAffected.Android)]
	public class Issue11703 : TestContentPage // or TestMasterDetailPage, etc ...
	{
		string buttonText = "Button text";
		string buttonId = "btnIssue11703";

		protected override void Init()
		{
			// Initialize ui here instead of ctor

			// create stack layout and add label explaining how to test by adding tag to styles.xml and running again and looking for camelcase text

			Content = new Button
			{
				AutomationId = buttonId,
				Text = buttonText
			};
		}
	}
}
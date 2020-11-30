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
	[Category(UITestCategories.CollectionView)]
#endif
	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Github, 8981, "[Bug] [iOS] CollectionView EmptyView causes the application to crash",
		PlatformAffected.macOS)]
	public partial class Issue8981 : TestContentPage
	{
		public Issue8981()
		{
#if APP
			InitializeComponent();
#endif
		}

		protected override void Init()
		{

		}
#if APP
		protected override async void OnAppearing()
		{
			base.OnAppearing();

			await TestLabel.ScaleTo(0, 1000);
			await TestLabel.ScaleTo(1, 1000);
		}
#endif
	}
}
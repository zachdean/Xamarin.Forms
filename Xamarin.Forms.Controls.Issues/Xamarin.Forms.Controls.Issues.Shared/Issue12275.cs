using System.Collections.ObjectModel;
using System.Threading.Tasks;
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
	[Issue(IssueTracker.Github, 12275, "[Regression] On iOS, CollectionView.EmptyView Does Not Appear in Xamarin.Forms v5.0.0.1487-pre1", PlatformAffected.iOS)]
	public class Issue12275 : TestContentPage
	{
		public Issue12275()
		{
			var layout = new StackLayout();

			var instructions = new Label
			{
				Padding = 12,
				BackgroundColor = Color.Black,
				TextColor = Color.White,
				Text = "If an image appears below (CollectionView EmptyView), the test has passed."
			};

			var collectionView = new CollectionView
			{
				EmptyView = new Image
				{
					Source = "https://img-prod-cms-rt-microsoft-com.akamaized.net/cms/api/am/imageFileData/RE2r0Th?ver=5b7d"
				}
			};

			layout.Children.Add(instructions);
			layout.Children.Add(collectionView);

			Content = layout;
		}

		protected override void Init()
		{
		
		}
	}
}

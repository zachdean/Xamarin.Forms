using Xamarin.Forms.CustomAttributes;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;

#if UITEST
using Xamarin.UITest;
using Xamarin.UITest.Queries;
using NUnit.Framework;
using Xamarin.Forms.Core.UITests;
#endif

namespace Xamarin.Forms.Controls.Issues
{
#if UITEST
	[Category(UITestCategories.CollectionView)]
#endif
#if APP
	[XamlCompilation(XamlCompilationOptions.Compile)]
#endif
	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Github, 8326, "[Bug] CollectionView: empty view doesn't show if collection view contains a header", PlatformAffected.Android)]
	public partial class Issue8326 : TestContentPage
	{
#if APP
		public Issue8326()
		{
			InitializeComponent();            
			collectionView.ItemsSource = new List<Issue8326Model> ();
		}
#endif

		protected override void Init()
		{
			Title = "Issue 8326";
		}
	}

	[Preserve(AllMembers = true)]
	public class Issue8326Model
	{
		public string Text { get; set; }
	}
}
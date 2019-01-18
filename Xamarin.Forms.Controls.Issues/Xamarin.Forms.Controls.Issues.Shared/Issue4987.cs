using System.Linq;
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
	[Issue(IssueTracker.Github, 4987, "Dismissing the Context Action Menu on iOS Prevents Tapping a ViewCel", PlatformAffected.iOS)]
	public class Issue14987 : TestContentPage // or TestMasterDetailPage, etc ...
	{
		string instructions = "Open context action, click delete, then tap same item to see if the item count increases";
		string success = "Success";
		string delete = "Delete";
		int count = 0;
		protected override void Init()
		{
			var label = new Label
			{
				Text = instructions
			};
			var labelCount = new Label
			{
				Text = $"Item Tap: {count}"
			};
			var listView = new ListView { ItemTemplate = new DataTemplate(typeof(ViewCellWithContextActions)), ItemsSource = Enumerable.Range(0, 3).Select(i => $"Item: {i}"), Header = new StackLayout { Children = { label, labelCount } } };
			listView.ItemTapped += (sender, args) =>
			{
				count++;
				labelCount.Text = $"Item Tap: {count}";
			};
			Content = listView;
		}


		[Preserve(AllMembers = true)]
		public class ViewCellWithContextActions : ViewCell
		{
			public ViewCellWithContextActions()
			{
				var label = new Label
				{
					HorizontalOptions = LayoutOptions.Center,
					VerticalOptions = LayoutOptions.Center
				};
				label.SetBinding(Label.TextProperty, ".");
				View = label;

				var delete = new MenuItem { Text = "Delete" };
				ContextActions.Add(delete);
			}
		}

#if UITEST
		[Test]
		public void Issue1Test() 
		{
			RunningApp.WaitForElement (q => q.Marked ("Item: 0"));
			RunningApp.Tap(q => q.Marked("Item: 0"));
			RunningApp.ActivateContextMenu("Item: 0");
			RunningApp.WaitForElement(q => q.Marked(delete));
			RunningApp.Tap(c => c.Marked(delete));
			RunningApp.WaitForElement(q => q.Marked("Item: 0"));
			RunningApp.Tap(q => q.Marked("Item: 0"));
			RunningApp.WaitForElement(q => q.Marked("Item Tap: 2"));
		}
#endif
	}
}
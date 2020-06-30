using System.Collections.ObjectModel;
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
	[Issue(IssueTracker.Github, 10942, "iOS - ListView and CollectionView - Editor - Focus is not cleared when Editor gets out of View", PlatformAffected.iOS)]
	public class Issue10942 : TestContentPage // or TestMasterDetailPage, etc ...
	{
		class EditorContentView : ContentView
		{
			public EditorContentView()
			{
				var editor = new Editor();
				editor.SetBinding(Editor.TextProperty, ".");
				Content = editor;
			}
		}

		class EditorViewCell : ViewCell
		{
			Editor editor;
			public EditorViewCell()
			{
				editor = new Editor();
				editor.SetBinding(Editor.TextProperty, ".");
				View = editor;
			}

			// Click on the first Cell/View and then scroll down
			// You notice that the OnDisappearing is not called for the cell where the editor is located
			protected override void OnDisappearing()
			{
				var text = (View as Editor)?.Text;
				editor.Unfocus();
				System.Diagnostics.Debug.WriteLine($"{text}");
				base.OnDisappearing(); // breakpoint here

			}
		}

		protected override void Init()
		{
			// comment one of the 2 lines out // use either listview or collectionview
			// Content = CreateCollectionView();
			Content = CreateListView();
		}

		ListView CreateListView()
		{
			var listView = new ListView();
			listView.ItemTemplate = new DataTemplate(typeof(EditorViewCell));
			listView.ItemsSource = GetSource();
			return listView;
		}

		CollectionView CreateCollectionView()
		{
			var collectionView = new CollectionView();
			collectionView.ItemTemplate = new DataTemplate(typeof(EditorContentView));
			collectionView.ItemsSource = GetSource();
			return collectionView;
		}

		ObservableCollection<int> GetSource() =>
			new ObservableCollection<int>(Enumerable.Range(0, 75));
	}
}
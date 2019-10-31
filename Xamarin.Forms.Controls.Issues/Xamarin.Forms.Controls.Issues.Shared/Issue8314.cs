using System.Collections.Generic;
using System.Diagnostics;
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
	[Issue(IssueTracker.Github, 8314, "Performance issue with ItemSizingStrategy on CollectionView", PlatformAffected.All)]
	public class Issue8314 : TestContentPage
	{
		Stopwatch _watch;
		Label _watchLabel;

		protected override void Init()
		{
			_watch = new Stopwatch();
			_watch.Start();

   			Title = "Issue 8314";

			var layout = new StackLayout();

			var instructions = new Label
			{
				BackgroundColor = Color.Black,
				TextColor = Color.White,
				Text = "The loading time must be below 1000 ms (100000 items). ItemSizingStrategy is MeasureFirstItem by default."
			};

			_watchLabel = new Label
			{
				FontSize = 14
			};

			var collectionView = new CollectionView
			{
				ItemTemplate = GetDataTemplate()
			};

			Debug.WriteLine(collectionView.ItemSizingStrategy);
   
			collectionView.ItemsLayout = new GridItemsLayout(ItemsLayoutOrientation.Vertical)
			{
				Span = 3
			};

			var items = new List<string>();

			for(int i = 0; i < 100000; i++)
			{
				items.Add($"Item {i + 1}");
			}

			collectionView.ItemsSource = items;
   
			layout.Children.Add(instructions);
			layout.Children.Add(_watchLabel);
			layout.Children.Add(collectionView);

			Content = layout;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

   			_watch.Stop();
			_watchLabel.Text = $"{_watch.ElapsedMilliseconds} ms";
		}

		DataTemplate GetDataTemplate()
		{
			var template = new DataTemplate(() =>
			{
				var scroll = new ScrollView();
				var stack = new StackLayout();

				var icon = new Image
				{
					Source = "calculator.png"
				};
				stack.Children.Add(icon);

				var cell = new Label();
				cell.SetBinding(Label.TextProperty, ".");
				cell.FontSize = 20;
				cell.BackgroundColor = Color.LightBlue;
				stack.Children.Add(cell);

				scroll.Content = stack;
				return scroll;
			});
			return template;
		}
	}
}
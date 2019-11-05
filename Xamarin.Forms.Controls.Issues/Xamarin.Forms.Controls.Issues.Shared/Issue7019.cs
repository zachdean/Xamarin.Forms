using System;
using Xamarin.Forms.CustomAttributes;

#if UITEST
using Xamarin.UITest;
using NUnit.Framework;
using Xamarin.Forms.Core.UITests;
#endif

namespace Xamarin.Forms.Controls.Issues
{
	[Issue(IssueTracker.Github, 7019, "Title goes empty with Fast Renderers in Android", PlatformAffected.Android)]
#if UITEST
	[Category(UITestCategories.TitleView)]
#endif
	public class Issue7019 : TestNavigationPage
	{
		int _i;

		protected override void Init()
		{
			PushAsync(GetPage());
		}

		ContentPage GetPage()
		{
			var page = new MyContentPage
			{
				BindingContext = new Issue7019ViewModel
				{
					Title = $"Foo {_i++}"
				}
			};

			var button = new Button
			{
				Text = "Next page"
			};

			button.Clicked += async (s, a) =>
			{
				await PushAsync(GetPage());
			};

			var stack = new StackLayout();
			stack.Children.Add(button);

			page.Content = stack;

			return page;
		}

		class Issue7019ViewModel
		{
			public string Title { get; set; }
		}

		class MyContentPage : ContentPage
		{
			public MyContentPage()
			{
				var titleView = new Grid();
				titleView.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
				titleView.ColumnDefinitions.Add(new ColumnDefinition());
				titleView.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

				var titleViewLabel = new Label
				{
					FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
					HorizontalTextAlignment = TextAlignment.Center,
					HorizontalOptions = LayoutOptions.CenterAndExpand,
					TextColor = Color.White,
					VerticalTextAlignment = TextAlignment.Center
				};

				titleViewLabel.SetBinding(Label.TextProperty, new Binding("Title"));

				titleView.Children.Add(titleViewLabel, 0, 0);
				Grid.SetColumnSpan(titleViewLabel, 3);

				NavigationPage.SetTitleView(this, titleView);
			}
		}
	}
}
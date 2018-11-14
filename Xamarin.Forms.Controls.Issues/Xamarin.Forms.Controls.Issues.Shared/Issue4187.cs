using System.Collections.Generic;
using Xamarin.Forms.CustomAttributes;
using Xamarin.Forms.Internals;

#if UITEST
using NUnit.Framework;
using System.Threading;
#endif

namespace Xamarin.Forms.Controls.Issues
{
	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Github, 4187, "Picker list shows up, when focus is set on other controls", PlatformAffected.Android)]
	public class Issue4187 : TestCarouselPage
	{
		protected override void Init()
		{
			var items = new List<Issue4187Model>
			{
				new Issue4187Model
				{
					Label = "Label 1",
					PickerSource = new List<string> {"Flower", "Harvest", "Propagation", "Vegetation"},
					Text = "Text 1"
				},
				new Issue4187Model
				{
					Label = "Label 2",
					PickerSource = new List<string> {"1", "2", "3", "4"},
					Text = "Text 2"
				}
			};
			var listView = new ListView
			{
				VerticalOptions = LayoutOptions.FillAndExpand,
				HasUnevenRows = true,
				ItemsSource = items,
				ItemTemplate = new DataTemplate(() =>
				{
					var label = new Label { Text = "Status" };
					label.SetBinding(Label.TextProperty, new Binding(nameof(Issue4187Model.Label)));
					var picker = new Picker();
					picker.SetBinding(Picker.ItemsSourceProperty, new Binding(nameof(Issue4187Model.PickerSource)));
					var entry = new Entry();
					entry.SetBinding(Entry.TextProperty, new Binding(nameof(Issue4187Model.Text)));

					return new ViewCell
					{
						View = new StackLayout
						{
							Children = {
								label,
								picker,
								new DatePicker(),
								new TimePicker(),
								entry
							}
						}
					};
				})
			};

			Children.Add(new ContentPage
			{
				Content = new StackLayout
				{
					Children = {
						GenerateNewPicker(),
						listView
					}
				}
			});

			Children.Add(new ContentPage
			{
				BackgroundColor = Color.Red,
				Content = new StackLayout
				{
					Children = { GenerateNewPicker() }
				}
			});

			Children.Add(new ContentPage
			{
				BackgroundColor = Color.Blue,
				Content = new StackLayout
				{
					Children = { GenerateNewPicker() }
				}
			});
		}

		Picker GenerateNewPicker()
		{
			var picker = new Picker();
			for (int i = 1; i < 10; i++)
				picker.Items.Add($"item {i}");
			return picker;
		}

		[Preserve(AllMembers = true)]
		class Issue4187Model
		{
			public string Label { get; set; }
			public List<string> PickerSource { get; set; }
			public string Text { get; set; }
		}

#if UITEST && __ANDROID__
		[Test]
		public void Issue4187Test()
		{
			RunningApp.WaitForElement("Text 1");
			Assert.AreEqual(7, RunningApp.Query(q => q.TextField().Class("PickerEditText")).Length);
			TapOnPicker(1);
			Assert.IsTrue(DialogIsOpened());
			RunningApp.Tap("Text 2");
			Assert.IsFalse(DialogIsOpened());
			TapOnPicker(3);
			Assert.IsTrue(DialogIsOpened());
			RunningApp.Tap("Text 1");
			Assert.IsFalse(DialogIsOpened());

			// Carousel - first page
			TapOnPicker(0);
			Assert.IsTrue(DialogIsOpened());

			// Red page
			RunningApp.SwipeRightToLeft();
			Assert.IsFalse(DialogIsOpened());
			TapOnPicker(0);
			Assert.IsTrue(DialogIsOpened());

			// Blue page
			RunningApp.SwipeRightToLeft();
			Assert.IsFalse(DialogIsOpened());
			TapOnPicker(0);
			Assert.IsTrue(DialogIsOpened());
		}

		void TapOnPicker(int index)
		{
			var picker = RunningApp.Query(q => q.TextField().Class("PickerEditText"))[index];
			var location = picker.Rect;
			RunningApp.TapCoordinates(location.X + 10, location.Y + location.Height / 2);
		}

		bool DialogIsOpened()
		{
			Thread.Sleep(1500);
			var frameLayouts = RunningApp.Query(q => q.Class("FrameLayout"));
			foreach (var layout in frameLayouts)
			{
				if (layout.Rect.X > 0 && layout.Rect.Y > 0 && layout.Description.Contains(@"id/content"))
				{
					// tap on close button
					RunningApp.Tap(q => q.Button().Id("button2"));
					Thread.Sleep(1500);
					return true;
				}
			}
			return false;
		}
#endif
	}
}
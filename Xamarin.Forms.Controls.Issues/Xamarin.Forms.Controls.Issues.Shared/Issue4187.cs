using System.Collections.Generic;
using Xamarin.Forms.CustomAttributes;
using Xamarin.Forms.Internals;

#if UITEST
using NUnit.Framework;
#endif

namespace Xamarin.Forms.Controls
{
	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Github, 4187, "Picker list shows up, when focus is set on other controls", PlatformAffected.Android)]
	public class Issue4187 : TestContentPage
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

			Content = new StackLayout()
			{
				Children = {
					listView
				}
			};
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
			void TapOnPicker(int index)
			{
				var picker = RunningApp.Query(q => q.TextField().Class("PickerEditText"))[index];
				var location = picker.Rect;
				RunningApp.TapCoordinates(location.X + 10, location.Y + location.Height / 2);
			}

			bool DialogIsOpened()
			{
				var frameLayouts = RunningApp.Query(q => q.Class("FrameLayout"));
				foreach (var layout in frameLayouts)
				{
					if (layout.Rect.X > 0 && layout.Rect.Y > 0 && layout.Description.Contains(@"id/content"))
					{
						// tap on close button
						RunningApp.Tap(q => q.Button().Id("button2"));
						return true;
					}
				}
				return false;
			}

			RunningApp.WaitForElement("Text 1");
			Assert.AreEqual(6, RunningApp.Query(q => q.TextField().Class("PickerEditText")).Length);
			TapOnPicker(0);
			Assert.IsTrue(DialogIsOpened());
			RunningApp.Tap("Text 2");
			Assert.IsFalse(DialogIsOpened());
			TapOnPicker(3);
			Assert.IsTrue(DialogIsOpened());
			RunningApp.Tap("Text 1");
			Assert.IsFalse(DialogIsOpened());
		}
#endif
	}
}
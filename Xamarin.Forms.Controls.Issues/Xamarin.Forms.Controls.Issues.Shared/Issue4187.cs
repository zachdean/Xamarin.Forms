using System;
using System.Collections.Generic;
using Xamarin.Forms.CustomAttributes;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Controls
{
	[Preserve (AllMembers=true)]
	[Issue (IssueTracker.Github, 4187, "Picker list shows up, when focus is set on other controls", PlatformAffected.Android)]
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
					var label = new Label() { Text = "Status" };
					label.SetBinding(Label.TextProperty, new Binding(nameof(Issue4187Model.Label)));
					var picker = new Picker();
					picker.SetBinding(Picker.ItemsSourceProperty, new Binding(nameof(Issue4187Model.PickerSource)));
					var entry = new Entry()
					{
						BackgroundColor = Color.Aqua
					};
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
	}
}
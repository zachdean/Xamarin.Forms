using System.Collections.Generic;
using System.Linq;
using System.Maui;
using System.Maui.Shapes;
using Xamarin.Forms;

namespace Sample
{
	public class MyApp : Application
	{
		Entry _entry;
		SearchBar _searchBar;
		Switch _iswitch;
		DatePicker _datePicker;
		Picker _picker;
		TimePicker _timePicker;
		Stepper _stepper;
		Button _button;
		Label _label;
		Label _otherLabel;

		public MyApp()
		{
			System.Maui.Registrar.Handlers.Register<DummyStackLayout, System.Maui.Platform.LayoutRenderer>();
			var layout = new DummyStackLayout() { BackgroundColor = Color.Bisque };

			foreach (var view in CreateTestViews().Concat(LotsOfLabels()))
			{
				layout.Children.Add(view);
			}

			MainPage = new ContentPage { Content = layout };

			_button.Clicked += (s, e) => {
				//#if __IOS__
				//					new UIKit.UIAlertView ("Hey!", "I was clicked", null, "Ok").Show ();
				//#else
				if (_label.Text.StartsWith("Lorem"))
				{
					System.Diagnostics.Debug.WriteLine($"Button clicked, decreasing length.");
					_label.Text = "At the top";
				}
				else
				{
					System.Diagnostics.Debug.WriteLine($"Button clicked, increasing length.");
					_label.Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.";
				}

				if (_otherLabel.Text.StartsWith("Lorem"))
				{
					_otherLabel.Text = "Hello Maui!";
				}
				else
				{
					_otherLabel.Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.";
				}
				//#endif	
			};

			_entry.TextChanged += (sender, args) => { System.Diagnostics.Debug.WriteLine($"Entry >>>>>> '{args.OldTextValue}' -> '{args.NewTextValue}'"); };

			_searchBar.SearchCommand = new Command(obj =>
			{
#if __IOS__
				new UIKit.UIAlertView("I m searching!", _searchBar.Text, null, "Ok").Show();
#endif
			});
			_searchBar.TextChanged += (sender, args) => { System.Diagnostics.Debug.WriteLine($"Search >>>>>> '{args.OldTextValue}' -> '{args.NewTextValue}'"); };
			_iswitch.Toggled += (s, e) =>
			{
#if __IOS__
				new UIKit.UIAlertView("I was toggled", e.Value.ToString(), null, "Ok").Show();
#endif
			};

			_entry.TextChanged += (sender, args) => { System.Diagnostics.Debug.WriteLine($"Entry >>>>>> '{args.OldTextValue}' -> '{args.NewTextValue}'"); };

			_datePicker.DateSelected += (sender, args) => { System.Diagnostics.Debug.WriteLine($"DatePicker >>>>>> '{args.OldDate}' -> '{args.NewDate}'"); };

			_picker.SelectedIndexChanged += (sender, args) => { System.Diagnostics.Debug.WriteLine($"Picker >>>>>> '{_picker.SelectedIndex}'"); };

			//_timePicker. += (sender, args) => { System.Diagnostics.Debug.WriteLine($"TimePicker >>>>>> '{args.OldTime}' -> '{args.NewTime}'"); };

			_stepper.ValueChanged += (sender, args) => { System.Diagnostics.Debug.WriteLine($"Stepper >>>>>> '{args.OldValue}' -> '{args.NewValue}'"); };
		}

		IEnumerable<IView> LotsOfLabels()
		{
			var count = 0;
			while (count < 0)
			{
				yield return new Label { Text = $"Label {count}", BackgroundColor = Color.LightPink };
				count += 1;
			}
		}

		DummyStackLayout CreateLabelAndEntry()
		{
			var label = new Label { Text = "Name:" };
			var entry = new Entry { Text = "Bob Loblaw" };

			var layout = new DummyStackLayout() { Orientation = Orientation.Horizontal };

			layout.Add(label);
			layout.Add(entry);

			layout.BackgroundColor = Color.LightGreen;

			return layout;
		}

		IView[] CreateTestViews() => new IView[] {
			(_label = new Label {
				Text = "At the top", BackgroundColor = Color.Red
			}),
			new CustomButton(){ Text = "Custom Button" },
			new ActivityIndicator
			{
				IsRunning = true,
				Color = Color.Red
			},
			(_otherLabel = new Label {
				Text = "Hello Maui!"
			}),
			(_entry = new Entry
			{
				Text = "Hello, Entry",
				PlaceholderColor = Color.SeaGreen,
				TextColor = Color.CornflowerBlue
			}),
			new Entry
			{
				Placeholder = "Enter some text here",
				TextColor = Color.Purple
			},

			CreateLabelAndEntry(),

			new Editor
			{
				Placeholder = "Enter some text here",
				PlaceholderColor = Color.Orange,
				TextColor = Color.Red
			},
			(_button = new Button {
				Text = "Click Me",
				BackgroundColor = Color.Green,
			}),
			new Label
			{
				Text = "Rounded Label!!!!",
				BackgroundColor = Color.Blue,
				TextColor = Color.White,
				ClipShape = new RoundedRectangle(6),
			},
			(_datePicker = new DatePicker
			{
				TextColor = Color.Brown,
				MinimumDate = new System.DateTime(1955, 11, 5),
				Date = new System.DateTime(1985, 10, 12),
				MaximumDate = new System.DateTime(2015, 10, 12)
			}),
			_picker = new Picker
			{
				Title = "Picker",
				TitleColor = Color.Red,
				TextColor = Color.Gray,
				ItemsSource = new List<string>
				{
					"Item 1",
					"Item 2",
					"Item 3"
				}
			},
			_timePicker = new TimePicker
			{
				TextColor = Color.Purple,
				Time = new System.TimeSpan(3, 45, 12),
				//ClockIdentifier = ClockIdentifiers.TwentyFourHour
			},
			new Slider
			{
				MinimumTrackColor = Color.MediumPurple,
				Minimum = 0,
				MaximumTrackColor = Color.Purple,
				Maximum = 100,
				Value = 50,
				ThumbColor = Color.DarkRed
			},
			(_searchBar = new SearchBar
			{
				Placeholder ="search here",
				PlaceholderColor = Color.Pink,
				BackgroundColor = Color.LightBlue,
				CancelButtonColor = Color.Red,
				TextColor = Color.Blue,
			}),
			new CheckBox()
			{
				ClipShape = new Circle(),
				BackgroundColor = Color.Purple
			},
			(_iswitch = new Switch{
				IsToggled = true,
				OnColor = Color.Red,
				ThumbColor = Color.RosyBrown
			}),
			new ProgressBar
			{
				Progress = 0.5,
				ProgressColor = Color.Green
			},
			_stepper = new Stepper
			{
				Minimum = 0,
				Maximum = 10,
				Value = 2,
				Increment = 0.5
			},
			//new WebView
			//{
			//	//Frame = new Rectangle(0, 0, 300, 300),
			//	Source = "https://xamarin.com"
			//},
		};
	}
}
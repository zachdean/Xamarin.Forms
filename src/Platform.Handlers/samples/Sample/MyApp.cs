using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Platform;
using Xamarin.Platform.Core;

namespace Sample
{
	public class MyApp : IApp
	{
		public MyApp()
		{
			Platform.Init();
		}

		public IView CreateView()
		{
			var verticalStack = new VerticalStackLayout() { Spacing = 5, BackgroundColor = Color.AntiqueWhite };
			var horizontalStack = new HorizontalStackLayout() { Spacing = 2 };

			var label = new Label { Text = "This top part is a Xamarin.Platform.VerticalStackLayout" };

			verticalStack.Add(label);

			var button = new Button() { Text = "A Button", Width = 200 };
			var button2 = new Button()
			{
				Color = Color.Green,
				Text = "Hello I'm a button",
				BackgroundColor = Color.Purple
			};

			horizontalStack.Add(button);
			horizontalStack.Add(button2);
			horizontalStack.Add(new Label { Text = "And these buttons are in a HorizontalStackLayout" });

			verticalStack.Add(horizontalStack);
			verticalStack.Add(CreatePicker());
			verticalStack.Add(new Slider());

			return verticalStack;
		}

		Picker CreatePicker()
		{
			var monkeyList = new List<string>
			{
				"Baboon",
				"Capuchin Monkey",
				"Blue Monkey",
				"Squirrel Monkey",
				"Golden Lion Tamarin",
				"Howler Monkey",
				"Japanese Macaque"
			};

			var picker = new Picker { Title = "Select a monkey", TitleColor = Color.Red, Color = Color.Green };
			picker.ItemsSource = monkeyList;

			return picker;
		}
	}
}
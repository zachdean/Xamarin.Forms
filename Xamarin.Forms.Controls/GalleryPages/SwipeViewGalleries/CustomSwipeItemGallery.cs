using System.Collections.Generic;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Controls.GalleryPages.SwipeViewGalleries
{
	[Preserve(AllMembers = true)]
	public class CustomSwipeItemGallery : ContentPage
	{
		public CustomSwipeItemGallery()
		{
			Title = "Custom SwipeItem Gallery";

			var swipeLayout = new StackLayout
			{
				Margin = new Thickness(12)
			};

			var swipeItemTextLabel = new Label
			{
				FontSize = 10,
				Text = "SwipeItem Text:"
			};

			swipeLayout.Children.Add(swipeItemTextLabel);

			var swipeItemTextEntry = new Entry
			{
				Text = "Delete",
				Placeholder = "SwipeItem Text"
			};

			swipeLayout.Children.Add(swipeItemTextEntry);

			var swipeItemTextColorLabel = new Label
			{
				FontSize = 10,
				Text = "Choose SwipeItem TextColor:"
			};

			swipeLayout.Children.Add(swipeItemTextColorLabel);

			var swipeItemTextColorPicker = new Picker();
			var colors = new List<string> { "#FFFFFF", "#FF0000", "#00FF00", "#0000FF", "#000000" };
			swipeItemTextColorPicker.ItemsSource = colors;
			swipeItemTextColorPicker.SelectedItem = colors[0];

			swipeLayout.Children.Add(swipeItemTextColorPicker);


			var swipeItemFontSizeLabel = new Label
			{
				FontSize = 10,
				Text = "Choose SwipeItem FontSize:"
			};

			swipeLayout.Children.Add(swipeItemFontSizeLabel);

			var swipeItemFontSizePicker = new Picker();
			var fonts = new List<int> { 10, 12, 14, 16, 18 };
			swipeItemFontSizePicker.ItemsSource = fonts;
			swipeItemFontSizePicker.SelectedItem = fonts[0];

			swipeLayout.Children.Add(swipeItemFontSizePicker);

			var deleteSwipeItem = new SwipeItem 
   			{
		 		BackgroundColor = Color.Red, 
				Text = swipeItemTextEntry.Text,
				TextColor = Color.FromHex(colors[swipeItemTextColorPicker.SelectedIndex]),
				FontSize = fonts[swipeItemFontSizePicker.SelectedIndex],
				Icon = "calculator.png" 
   			};

			deleteSwipeItem.Invoked += (sender, e) => { DisplayAlert("SwipeView", "Delete Invoked", "Ok"); };

			var leftSwipeItems = new SwipeItems
			{
				deleteSwipeItem
			};

			leftSwipeItems.Mode = SwipeMode.Execute;

			var swipeContent = new Grid
			{
				BackgroundColor = Color.Gray
			};

			var swipeLabel = new Label
			{
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				Text = "Swipe to Right"
			};

			swipeContent.Children.Add(swipeLabel);

			var swipeView = new SwipeView
			{
				HeightRequest = 60,
				WidthRequest = 300,
				LeftItems = leftSwipeItems,
				Content = swipeContent
			};

			swipeLayout.Children.Add(swipeView);

			swipeItemTextEntry.TextChanged += (sender, e) =>
			{
				deleteSwipeItem.Text = swipeItemTextEntry.Text;
			};

			swipeItemTextColorPicker.SelectedIndexChanged += (sender, e) =>
			{
				deleteSwipeItem.TextColor = Color.FromHex(colors[swipeItemTextColorPicker.SelectedIndex]);
			};

			swipeItemFontSizePicker.SelectedIndexChanged += (sender, e) =>
			{
				deleteSwipeItem.FontSize = fonts[swipeItemFontSizePicker.SelectedIndex];
			};

			Content = swipeLayout;
		}
	}
}


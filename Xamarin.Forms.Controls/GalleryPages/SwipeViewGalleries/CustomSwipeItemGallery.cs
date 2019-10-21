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

			var swipeItemBackgroundColorLabel = new Label
			{
				FontSize = 10,
				Text = "Choose SwipeItem BackgroundColor:"
			};

			swipeLayout.Children.Add(swipeItemBackgroundColorLabel);

			var swipeItemBackgroundColorPicker = new Picker();
			var colors = new List<string> { "#FFFFFF", "#FF0000", "#00FF00", "#0000FF", "#000000" };
			swipeItemBackgroundColorPicker.ItemsSource = colors;
			swipeItemBackgroundColorPicker.SelectedItem = colors[1];

			swipeLayout.Children.Add(swipeItemBackgroundColorPicker);

			var deleteSwipeItem = new SwipeItem
			{
				BackgroundColor = Color.FromHex(colors[swipeItemBackgroundColorPicker.SelectedIndex]),
				IconImageSource = "calculator.png",
				Text = swipeItemTextEntry.Text
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

			swipeItemBackgroundColorPicker.SelectedIndexChanged += (sender, e) =>
			{
				deleteSwipeItem.BackgroundColor = Color.FromHex(colors[swipeItemBackgroundColorPicker.SelectedIndex]);
			};
   
			Content = swipeLayout;
		}
	}
}
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Controls.GalleryPages.SwipeViewGalleries
{
	[Preserve(AllMembers = true)]
	public class SwipeThresholdGallery : ContentPage
	{
		public SwipeThresholdGallery()
		{
			Title = "SwipeThreshold Gallery";

			var swipeLayout = new StackLayout
			{
				Margin = new Thickness(12)
			};

			var swipeswipeThresholdLabel = new Label
			{
				FontSize = 10,
				Text = "SwipeThreshold:"
			};

			swipeLayout.Children.Add(swipeswipeThresholdLabel);

			var swipeThresholdSlider = new Slider
			{
				ThumbColor = Color.OrangeRed,
				BackgroundColor = Color.Blue,
				Maximum = 250,
				Minimum = 120
			};

			swipeLayout.Children.Add(swipeThresholdSlider);
   
			var deleteSwipeItem = new SwipeItem { BackgroundColor = Color.Red, Text = "Delete", Icon = "calculator.png" };
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
				SwipeThreshold = 250,
				LeftItems = leftSwipeItems,
				View = swipeContent
			};
			
			swipeLayout.Children.Add(swipeView);

			swipeThresholdSlider.Value = swipeView.SwipeThreshold;

			swipeThresholdSlider.ValueChanged += (sender, e) =>
			{
				swipeView.SwipeThreshold = swipeThresholdSlider.Value;
			};

			Content = swipeLayout;
		}
	}
}


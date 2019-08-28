using Xamarin.Forms.CustomAttributes;

namespace Xamarin.Forms.Controls
{
	internal class SwipeViewCoreGalleryPage : CoreGalleryPage<SwipeView>
	{
		protected override bool SupportsFocus
		{
			get { return false; }
		}

		protected override bool SupportsScroll 
		{
			get { return false; }
		}

		protected override void InitializeElement(SwipeView element)
		{
			element.HeightRequest = 60;
			element.BackgroundColor = Color.LightGray;
			element.Content = GetSwipeContent();
			element.LeftItems = GetSwipeItems();
		}

		protected override void Build(StackLayout stackLayout)
		{
			base.Build(stackLayout);

			var rightItemsContainer = new ValueViewContainer<SwipeView>(Test.SwipeView.RightItems, new SwipeView { RightItems = GetSwipeItems(), Content = GetSwipeContent(), HeightRequest = 60, BackgroundColor = Color.LightPink }, "RightItems", value => value.ToString());
			var topItemsContainer = new ValueViewContainer<SwipeView>(Test.SwipeView.TopItems, new SwipeView { TopItems = GetSwipeItems(), Content = GetSwipeContent(), HeightRequest = 60, BackgroundColor = Color.LightSkyBlue }, "TopItems", value => value.ToString());
			var bottomItemsContainer = new ValueViewContainer<SwipeView>(Test.SwipeView.BottomItems, new SwipeView { BottomItems = GetSwipeItems(), Content = GetSwipeContent(), HeightRequest = 60, BackgroundColor = Color.LightGray }, "BottomItems", value => value.ToString());

			Add(rightItemsContainer);
			Add(topItemsContainer);
			Add(bottomItemsContainer);
		}

		internal SwipeItems GetSwipeItems()
		{
			var swipeItems = new SwipeItems
			{
				new SwipeItem { BackgroundColor = Color.Green, Text = "Modify", Icon = "coffee.png"},
				new SwipeItem { BackgroundColor = Color.Red, Text = "Delete", Icon = "calculator.png" }
			};

			swipeItems.Mode = SwipeMode.Reveal;

			return swipeItems;
		}

		internal Grid GetSwipeContent()
		{
			var content = new Grid
			{
				BackgroundColor = Color.LightGoldenrodYellow
			};

			var info = new Label
			{
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				Text = "Swipe to Left (Reveal)"
			};

			content.Children.Add(info);

			return content;
		}
	}
}
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Controls.GalleryPages.SwipeViewGalleries
{
	[Preserve(AllMembers = true)]
	public class SwipeViewGallery : ContentPage
	{
		public SwipeViewGallery()
		{
			Content = new StackLayout
			{
				Children =
				{
					new Button { Text ="Enable CollectionView", AutomationId = "EnableCollectionView", Command = new Command(() => Device.SetFlags(new[] { ExperimentalFlags.CollectionViewExperimental })) },
					GalleryBuilder.NavButton("Basic SwipeView Gallery", () => new BasicSwipeGallery(), Navigation),
					GalleryBuilder.NavButton("BindableLayout Gallery", () => new SwipeBindableLayoutGallery(), Navigation),
					GalleryBuilder.NavButton("ListView (RecycleElement) Gallery", () => new SwipeListViewGallery(), Navigation),
					GalleryBuilder.NavButton("CollectionView Gallery", () => new SwipeCollectionViewGallery(), Navigation),
					GalleryBuilder.NavButton("SwipeBehaviorOnInvoked Gallery", () => new SwipeBehaviorOnInvokedGallery(), Navigation),
					GalleryBuilder.NavButton("Custom SwipeItem Gallery", () => new CustomSwipeItemGallery(), Navigation),
					GalleryBuilder.NavButton("SwipeItem Icon Gallery", () => new SwipeItemIconGallery(), Navigation),       
					GalleryBuilder.NavButton("SwipeTransitionMode Gallery", () => new SwipeTransitionModeGallery(), Navigation)
				}
			};
		}
	}
}
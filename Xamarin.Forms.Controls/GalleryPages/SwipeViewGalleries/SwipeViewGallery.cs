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
					GalleryBuilder.NavButton("Basic SwipeView Galleries", () => new BasicSwipeGallery(), Navigation),
					GalleryBuilder.NavButton("BindableLayout Galleries", () => new SwipeBindableLayoutGallery(), Navigation),
					GalleryBuilder.NavButton("ListView Galleries", () => new SwipeListViewGallery(), Navigation),
					GalleryBuilder.NavButton("CollectionView Galleries", () => new SwipeCollectionViewGallery(), Navigation)
				}
			};
		}
	}
}
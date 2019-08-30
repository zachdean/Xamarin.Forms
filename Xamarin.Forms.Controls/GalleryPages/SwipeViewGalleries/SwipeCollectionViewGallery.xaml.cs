using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Controls.GalleryPages.SwipeViewGalleries
{
	[Preserve(AllMembers = true)]
	public partial class SwipeCollectionViewGallery : ContentPage
	{
		public SwipeCollectionViewGallery()
		{
			InitializeComponent();
			BindingContext = new SwipeViewGalleryViewModel();
		}
	}
}

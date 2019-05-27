using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Xamarin.Forms.Controls
{
	public partial class BlogCollectionPage : BaseView
	{
		DisplayOrientation _orientation;
		BlogFeedViewModel _viewModel;
		BlogFeedViewModel ViewModel => _viewModel ?? (_viewModel = BindingContext as BlogFeedViewModel);
		public BlogCollectionPage()
		{
			InitializeComponent();

			_orientation = DeviceDisplay.MainDisplayInfo.Orientation;
			if (DeviceInfo.Idiom == DeviceIdiom.Phone)
			{
				DeviceDisplay.MainDisplayInfoChanged += DeviceDisplay_MainDisplayInfoChanged;
				SetSize();
			}
		}

		void DeviceDisplay_MainDisplayInfoChanged(object sender, DisplayInfoChangedEventArgs e)
		{
			if (_orientation != e.DisplayInfo.Orientation)
			{
				_orientation = e.DisplayInfo.Orientation;
				SetSize();
			}
		}

		void SetSize()
		{
			var gil = (GridItemsLayout)CollectionViewBlog.ItemsLayout;
			gil.Span = _orientation == DisplayOrientation.Portrait ? 1 : 2;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			OnPageVisible();
		}

		public void OnPageVisible()
		{
			if (ViewModel == null || !ViewModel.CanLoadMore || ViewModel.IsBusy || ViewModel.FeedItems.Count > 0)
				return;

			ViewModel.LoadItemsCommand.Execute(null);
		}
	}
}

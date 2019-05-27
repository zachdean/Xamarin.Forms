using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Linq;
using Xamarin.Forms;
using System.Windows.Input;
using System.Net.Http;

namespace Xamarin.Forms.Controls
{
	public partial class BlogPage : BaseView
	{
		BlogFeedViewModel viewModel;
		BlogFeedViewModel ViewModel => viewModel ?? (viewModel = BindingContext as BlogFeedViewModel);

		public BlogPage()
		{
			InitializeComponent();

			listView.ItemTapped += (sender, args) =>
			{
				if (listView.SelectedItem == null)
					return;
				this.Navigation.PushAsync(new BlogDetailsView(listView.SelectedItem as FeedItem));
				listView.SelectedItem = null;
			};
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


	public class BlogDetailsView : BaseView
	{
		public BlogDetailsView(FeedItem item)
		{
			BindingContext = item;
			var webView = new WebView
			{
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand
			};
			webView.Source = item.Link;
			Content = new StackLayout
			{
				Children = {  webView }
			};
			var share = new ToolbarItem
			{
				IconImageSource = "ic_share.png",
				Text = "Share",
				//Command = new Command(() => CrossShare.Current
				//  .Share("Be sure to read @shanselman's " + item.Title + " " + item.Link))
			};

			ToolbarItems.Add(share);
		}
	}
}

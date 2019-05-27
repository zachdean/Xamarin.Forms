using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Xamarin.Forms.Controls
{
	public partial class TwitterPage : BaseView
	{
		TwitterViewModel ViewModel => BindingContext as TwitterViewModel;

		public TwitterPage()
		{
			InitializeComponent();

			BindingContext = new TwitterViewModel();

			listView.ItemTapped += async (sender, args) =>
			{
				if (listView.SelectedItem == null)
					return;


				var tweet = listView.SelectedItem as Tweet;

				await Task.FromResult(true);
				//try to launch twitter or tweetbot app, else launch browser
				//var launch = DependencyService.Get<ILaunchTwitter>();
				//if (launch == null || !launch.OpenStatus(tweet.StatusID.ToString()))
				await Browser.OpenAsync("http://twitter.com/shanselman/status/" + tweet.StatusID);

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
			if (ViewModel == null || !ViewModel.CanLoadMore || ViewModel.IsBusy || ViewModel.Tweets.Count > 0)
				return;

			ViewModel.LoadTweetsCommand.Execute(null);
		}
	}
}

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Xamarin.Forms.Controls
{
	public class TwitterViewModel : HBaseViewModel
	{
		public ObservableCollection<Tweet> Tweets { get; }

		public TwitterViewModel()
		{
			Title = "Twitter";
			Icon = "slideout.png";
			Tweets = new ObservableCollection<Tweet>();
		}

		private Command loadTweetsCommand;

		public Command LoadTweetsCommand
		{
			get
			{
				return loadTweetsCommand ??
				  (loadTweetsCommand = new Command(async () =>
				  {
					  await ExecuteLoadTweetsCommand();
				  }, () =>
				  {
					  return !IsBusy;
				  }));
			}
		}

		public async Task ExecuteLoadTweetsCommand()
		{
			if (IsBusy)
				return;

			IsBusy = true;
			LoadTweetsCommand.ChangeCanExecute();
			var error = false;
			try
			{

				Tweets.Clear();
				//get tweets

			}
			catch
			{
				error = true;
			}

			if (error)
			{
				await Application.Current.MainPage?.DisplayAlert("Error", "Unable to load tweets.", "OK");
			}

			IsBusy = false;
			LoadTweetsCommand.ChangeCanExecute();
		}
	}
}

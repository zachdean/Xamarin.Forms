using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Controls.Helpers;
using Xamarin.Forms.Controls.Issues;

namespace Xamarin.Forms.Controls
{
	public partial class MyAbout : BaseView
	{
		public MyAbout()
		{
			InitializeComponent();
		}
	}

	public class AboutViewModel : HBaseViewModel
	{
		public List<SocialItem> SocialItems { get; }
		public AboutViewModel()
		{
			SocialItems = new List<SocialItem>
			{
				new SocialItem
				{
					Icon = IconConstants.TwitterCircle,
					Url = "https://www.twitter.com/shanselman"
				},
				new SocialItem
				{
					Icon = IconConstants.FacebookBox,
					Url = "https://www.facebook.com/shanselman"
				},
				new SocialItem
				{
					Icon = IconConstants.Instagram,
					Url = "https://www.instagram.com/shanselman"
				}
			};
		}
	}

	public class SocialItem
	{
		public SocialItem()
		{
			OpenUrlCommand = new Command(async () => await OpenSocialUrl());
		}

		public string Icon { get; set; }
		public string Url { get; set; }

		public ICommand OpenUrlCommand { get; }

		async Task OpenSocialUrl()
		{
			await Task.FromResult(true);
			//if (Url.Contains("twitter"))
			//{
			//	var launch = DependencyService.Get<ILaunchTwitter>();
			//	if (launch?.OpenUserName("shanselman") ?? false)
			//		return;
			//}
			//await Browser.OpenAsync(Url);
		}
	}
}

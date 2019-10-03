using System.Threading.Tasks;
using Xamarin.Forms.CustomAttributes;
using Xamarin.Forms.Internals;

#if UITEST
using Xamarin.Forms.Core.UITests;
using Xamarin.UITest;
using NUnit.Framework;
#endif

namespace Xamarin.Forms.Controls.Issues
{
#if UITEST
	[Category(UITestCategories.ManualReview)]
#endif
	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Github, 5386, "ToolbarItem IsEnabled doesnt read initial binding", PlatformAffected.Android)]
	public partial class Issue5386 : TestContentPage
	{
		public Issue5386()
		{
			InitializeComponent();
			Title = "Issue 5386";
			BindingContext = new Issue5386ViewModel();
		}

		protected override void Init()
		{

		}
	}

	[Preserve(AllMembers = true)]
	public class Issue5386ViewModel : BindableObject
	{
		public bool Enabled { get; set; }
		public string EnabledText { get; set; }
		public Command ChangeToggleCommand { get; set; }
		public Command ToolbarTappedCommand { get; set; }
		public Command ChangeToggleBackgroundThreadCommand { get; set; }

		public Issue5386ViewModel()
		{
			ChangeToggleCommand = new Command(ChangeToggle);
			ChangeToggleBackgroundThreadCommand = new Command(ChangeToggleBackgroundThread);
			ToolbarTappedCommand = new Command(ToolbarTapped);
			EnabledText = Enabled ? "Enabled" : "Disabled";
		}

		private async void ChangeToggleBackgroundThread(object obj)
		{
			await Task.Run(() =>
			{
				ChangeToggle();
			});
		}

		private async void ToolbarTapped(object obj)
		{
			await Application.Current.MainPage.DisplayAlert("ToolbarItem", "You tapped me and I obeyed", "OK");
		}

		private void ChangeToggle()
		{
			Enabled = !Enabled;
			OnPropertyChanged("Enabled");

			EnabledText = Enabled ? "Enabled" : "Disabled";
			OnPropertyChanged("EnabledText");
		}
	}
}
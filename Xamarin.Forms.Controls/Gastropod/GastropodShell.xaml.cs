using System.Windows.Input;
using Xamarin.Forms.Controls.Gastropod.Pages;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Controls.Gastropod
{
	[Preserve(AllMembers = true)]
	public partial class GastropodShell : Xamarin.Forms.Shell
    {
        public ICommand TakePhotoCommand => new Command(GoToCamera);
        public ICommand LoginCommand => new Command(GoToLogin);

        public GastropodShell()
        {
            InitializeComponent();

            BindingContext = this;

            RegisterRoutes();
        }

        private void RegisterRoutes()
        {
            Routing.RegisterRoute("login", typeof(LoginPage));
        }

        void GoToCamera()
        {
            Shell.Current.GoToAsync("//photo?payload=4.x", true);
            Shell.Current.FlyoutIsPresented = false;
        }

        void GoToLogin()
        {
            Shell.Current.Navigation.PushModalAsync(new LoginPage(), true);
            Shell.Current.FlyoutIsPresented = false;
        }
    }
}
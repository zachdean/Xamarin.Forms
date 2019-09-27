using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Controls.Gastropod.Pages
{
	[Preserve(AllMembers = true)]
	public partial class LoginPage : ContentPage
    {
        void Handle_Clicked(object sender, System.EventArgs e)
        {
            Shell.Current.Navigation.PopModalAsync();
        }

        public LoginPage()
        {
            InitializeComponent();
        }
    }
}

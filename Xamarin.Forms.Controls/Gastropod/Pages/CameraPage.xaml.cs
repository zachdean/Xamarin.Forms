using Xamarin.Forms.Controls.Gastropod.ViewModels;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Controls.Gastropod
{
	[Preserve(AllMembers = true)]
	public partial class CameraPage : ContentPage
    {
        public CameraPage()
        {
            InitializeComponent();
            BindingContext = new CameraViewModel();
        }
    }
}
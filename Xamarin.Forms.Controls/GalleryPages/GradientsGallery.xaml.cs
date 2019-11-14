namespace Xamarin.Forms.Controls.GalleryPages
{
	public partial class GradientsGallery : ContentPage
	{
		public GradientsGallery()
		{
			InitializeComponent();
			BackgroundPicker.SelectedIndex = 1;
		}

		private void OnBackgroundSelectedIndexChanged(object sender, System.EventArgs e)
		{
			Brush background = null;

			var selectedIndex = ((Picker)sender).SelectedIndex;

			switch (selectedIndex)
			{
				case 0:
					background = Resources["SolidColor"] as Brush;
					break;
				case 1:
					background = Resources["LinearGradient"] as Brush;
					break;
				case 2:
					background = Resources["RadialGradient"] as Brush;
					break;
			}

			if (background != null)
			{
				Button.Background = background;
				BoxView.Background = background;
				CollectionView.Background = background;
				DatePicker.Background = background;
				Editor.Background = background;
				Entry.Background = background;
				Frame.Background = background;
				Grid.Background = background;
				ImageButton.Background = background;
				ListView.Background = background;
				Picker.Background = background;
				SearchBar.Background = background;
				Slider.Background = background;
				Stepper.Background = background;
				TimePicker.Background = background;
			}
		}
	}
}
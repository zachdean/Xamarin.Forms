using System.Maui.Platform;
using System.Windows;

namespace Sample.WPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			var app = new MyApp();
			//Application.Current.MainWindow.Content = app.MainPage.ToNative();
		}
	}
}

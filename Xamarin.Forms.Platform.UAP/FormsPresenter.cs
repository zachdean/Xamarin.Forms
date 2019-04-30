using Windows.UI.Xaml;

namespace Xamarin.Forms.Platform.UWP
{
	internal class FormsPresenter : Windows.UI.Xaml.Controls.ContentPresenter
	{
		public FormsPresenter()
		{
			Loading += OnLoading;
			Loaded += OnLoaded;
			Unloaded += OnUnloaded;
			SizeChanged += (s, e) =>
			{
				if (ActualWidth > 0 && ActualHeight > 0 && DataContext != null)
				{
					var page = (Page)DataContext;
					((Page)page.RealParent).ContainerArea = new Rectangle(0, 0, ActualWidth, ActualHeight);
				}
			};
		}

		void OnLoading(FrameworkElement sender, object args)
		{
			if (!Application.Current.UseLegacyPageEvents)
				(DataContext as Page)?.SendAppearing();
		}

		void OnLoaded(object sender, RoutedEventArgs e)
		{
			(DataContext as Page)?.SendAppear(Application.Current.UseLegacyPageEvents);
		}

		void OnUnloaded(object sender, RoutedEventArgs e)
		{
			if (DataContext is Page page)
			{
				if (!Application.Current.UseLegacyPageEvents)
					page.SendDisappearing();

				page.SendDisappear(Application.Current.UseLegacyPageEvents);
			}
		}
	}
}

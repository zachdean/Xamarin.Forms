using System.ComponentModel;
using Windows.UI.Xaml.Media;

namespace Xamarin.Forms.Platform.UWP
{
	public class RefreshViewRenderer : ViewRenderer<RefreshView, RefreshViewControl>
	{
		public RefreshViewRenderer()
		{
			AutoPackage = false;
		}

		protected override void Dispose(bool disposing)
		{
			if (Control != null && disposing)
			{
				Control.Refresh -= OnRefresh;
				Control.Dispose();
			}

			base.Dispose(disposing);
		}

		protected override void OnElementChanged(ElementChangedEventArgs<RefreshView> e)
		{
			if (e.NewElement != null)
			{
				if (Control == null)
				{
					var refreshControl = new RefreshViewControl();
					refreshControl.Refresh += OnRefresh;
					SetNativeControl(refreshControl);
				}

				UpdateContent();
				UpdateIsEnabled();
				UpdateIsRefreshing();
				UpdateColors();
			}

			base.OnElementChanged(e);
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == ContentView.ContentProperty.PropertyName)
				UpdateContent();
			else if (e.PropertyName == VisualElement.IsEnabledProperty.PropertyName)
				UpdateIsEnabled();
			else if (e.PropertyName == RefreshView.IsRefreshingProperty.PropertyName)
				UpdateIsRefreshing();
			else if (e.PropertyName == RefreshView.RefreshColorProperty.PropertyName)
				UpdateColors();
		}

		protected override void UpdateBackgroundColor()
		{
			if (Element == null || Control == null)
				return;

			if (Element.BackgroundColor != Color.Default)
				Control.RefreshBackground = Element.BackgroundColor.ToBrush();
			else
				Control.RefreshBackground = Color.White.ToBrush();
		}

		private void UpdateContent()
		{
			if (Element.Content == null)
				return;

			IVisualElementRenderer renderer = Element.Content.GetOrCreateRenderer();
			Control.Content = renderer.ContainerElement;
		}

		private void UpdateIsEnabled()
		{
			Control.IsRefreshingEnabled = Element.IsEnabled;
		}

		private void UpdateIsRefreshing()
		{
			Control.IsRefreshing = Element.IsRefreshing;
		}

		private void UpdateColors()
		{
			Control.RefreshForeground = Element.RefreshColor != Color.Default 
				? Element.RefreshColor.ToBrush() 
				: (Brush)Windows.UI.Xaml.Application.Current.Resources["DefaultTextForegroundThemeBrush"];

			UpdateBackgroundColor();
		}

		private void OnRefresh(object sender, System.EventArgs e)
		{
			if (Element?.Command?.CanExecute(Element?.CommandParameter) ?? false)
			{
				Element.Command.Execute(Element?.CommandParameter);
			}
		}
	}
}
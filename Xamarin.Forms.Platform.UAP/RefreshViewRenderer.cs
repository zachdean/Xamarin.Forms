using System.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using Windows.Foundation;
using Windows.UI.Xaml.Media;

namespace Xamarin.Forms.Platform.UWP
{
	public class RefreshViewRenderer : ViewRenderer<RefreshView, RefreshContainer>
	{
		Deferral _refreshCompletionDeferral;

		public RefreshViewRenderer()
		{
			AutoPackage = false;
		}

		protected override void Dispose(bool disposing)
		{
			if (Control != null && disposing)
			{
				Control.RefreshRequested -= OnRefresh;

				if (_refreshCompletionDeferral != null)
				{
					_refreshCompletionDeferral.Complete();
					_refreshCompletionDeferral.Dispose();
					_refreshCompletionDeferral = null;
				}
			}

			base.Dispose(disposing);
		}

		protected override void OnElementChanged(ElementChangedEventArgs<RefreshView> e)
		{
			if (e.NewElement != null)
			{
				if (Control == null)
				{
					var refreshControl = new RefreshContainer
					{
						PullDirection = RefreshPullDirection.TopToBottom,
						Visualizer = new RefreshVisualizer()
					};

					refreshControl.RefreshRequested += OnRefresh;
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
				Control.Visualizer.Background = Element.BackgroundColor.ToBrush();
			else
				Control.Visualizer.Background = Color.White.ToBrush();
		}

		void UpdateContent()
		{
			if (Element.Content == null)
				return;

			IVisualElementRenderer renderer = Element.Content.GetOrCreateRenderer();
			Control.Content = renderer.ContainerElement;
		}

		void UpdateIsEnabled()
		{
			Control.IsEnabled = Element.IsEnabled;
		}

		void UpdateIsRefreshing()
		{
			if (!Element.IsRefreshing)
			{
				CompleteRefresh();
			}
			else if(_refreshCompletionDeferral == null)
			{
				Control.RequestRefresh();
			}
		}

		void UpdateColors()
		{
			Control.Visualizer.Foreground = Element.RefreshColor != Color.Default
				? Element.RefreshColor.ToBrush()
				: (Brush)Windows.UI.Xaml.Application.Current.Resources["DefaultTextForegroundThemeBrush"];

			UpdateBackgroundColor();
		}

		void CompleteRefresh()
		{
			if (_refreshCompletionDeferral != null)
			{
				_refreshCompletionDeferral.Complete();
				_refreshCompletionDeferral.Dispose();
				_refreshCompletionDeferral = null;
			}
		}

		void OnRefresh(object sender, RefreshRequestedEventArgs args)
		{
			_refreshCompletionDeferral = args.GetDeferral();

			if (Element?.Command?.CanExecute(Element?.CommandParameter) ?? false)
			{
				Element.Command.Execute(Element?.CommandParameter);
			}
		}
	}
}
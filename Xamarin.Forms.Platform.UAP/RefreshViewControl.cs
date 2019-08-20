using System;
using Windows.Devices.Input;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;
using WGrid = Windows.UI.Xaml.Controls.Grid;

namespace Xamarin.Forms.Platform.UWP
{
	public sealed class RefreshViewControl : ContentControl, IDisposable
	{
		private const double IndicatorHeight = 200d;
		private const double ElasticFactor = 0.5d;
		private const double TriggerOffset = 40.0d;

		private bool _triggerStart = false;
		private bool _triggered = false;
		private bool _canSlide = false;
		private bool _canTrigger = false;

		public RefreshViewControl()
		{
			DefaultStyleKey = typeof(RefreshViewControl);
		}

		public RelativePanel Root { get; private set; }
		public WGrid Indicator { get; private set; }
		public ScrollViewer Main { get; private set; }
		public CompositeTransform IndicatorTransform { get; private set; }
		public Storyboard IndicatorInAnimation { get; private set; }
		public Storyboard IndicatorOutAnimation { get; private set; }
		public Storyboard IndicatorOverlayInAnimation { get; private set; }
		public Storyboard IndicatorOverlayOutAnimation { get; private set; }
		public ProgressRing IndicatorRing { get; private set; }
		public Ellipse IndicatorOverlayBackground { get; private set; }
		public Storyboard RefreshStartAnimation { get; private set; }
		public Storyboard RefreshCompleteAnimation { get; private set; }

		public event EventHandler<EventArgs> Refresh;

		public Brush RefreshBackground
		{
			get { return (SolidColorBrush)GetValue(RefreshBackgroundProperty); }
			set { SetValue(RefreshBackgroundProperty, value); }
		}
		public static readonly DependencyProperty RefreshBackgroundProperty =
			DependencyProperty.Register("RefreshBackground", typeof(Brush), typeof(RefreshViewControl), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

		public Brush RefreshForeground
		{
			get { return (Brush)GetValue(RefreshForegroundProperty); }
			set { SetValue(RefreshForegroundProperty, value); }
		}

		public static readonly DependencyProperty RefreshForegroundProperty =
			DependencyProperty.Register("RefreshForeground", typeof(Brush), typeof(RefreshViewControl), new PropertyMetadata(new SolidColorBrush(Colors.White)));

		public ScrollBarVisibility VerticalScrollBarVisibility
		{
			get { return (ScrollBarVisibility)GetValue(VerticalScrollBarVisibilityProperty); }
			set { SetValue(VerticalScrollBarVisibilityProperty, value); }
		}

		public static readonly DependencyProperty VerticalScrollBarVisibilityProperty =
			DependencyProperty.Register("VerticalScrollBarVisibility", typeof(ScrollBarVisibility), typeof(RefreshViewControl), new PropertyMetadata(0));

		public bool IsRefreshingEnabled
		{
			get { return (bool)GetValue(IsRefreshingEnabledProperty); }
			set { SetValue(IsRefreshingEnabledProperty, value); }
		}

		public static readonly DependencyProperty IsRefreshingEnabledProperty =
			DependencyProperty.Register("IsRefreshingEnabled", typeof(bool), typeof(RefreshViewControl), new PropertyMetadata(true));

		public bool IsRefreshing
		{
			get { return (bool)GetValue(IsRefreshingProperty); }
			set { SetValue(IsRefreshingProperty, value); }
		}

		public static DependencyProperty IsRefreshingProperty { get; private set; } =
			DependencyProperty.Register("IsRefreshing", typeof(bool), typeof(RefreshViewControl), new PropertyMetadata(false, OnIsRefreshingChanged));

		private static void OnIsRefreshingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is RefreshViewControl refreshViewControl)
			{
				if ((bool)e.NewValue)
				{
					refreshViewControl.RefreshStart();
				}
				else
				{
					refreshViewControl.RefreshEnd();
				}
			}
		}

		public void Dispose()
		{
			if (Root != null)
			{
				Root.ManipulationDelta -= OnRootManipulationDelta;
				Root.Loaded -= OnRootLoaded;
				Root.PointerPressed -= OnRootPointerPressed;
				Root.PointerReleased -= OnRootPointerReleased;
				Root.PointerCanceled -= OnRootPointerReleased;
				Root.PointerCaptureLost -= OnRootPointerReleased;
				Root.PointerExited -= OnRootPointerReleased;
				Root = null;
			}

			if (Main != null)
			{
				Main.PointerWheelChanged -= OnMainPointerWheelChanged;
				Main = null;
			}

			if (Indicator != null)
			{
				Indicator = null;
			}
		}

		protected override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			Root = GetTemplateChild("Root") as RelativePanel;
			Indicator = GetTemplateChild("Indicator") as WGrid;
			Main = GetTemplateChild("Main") as ScrollViewer;
			IndicatorTransform = GetTemplateChild("IndicatorTransform") as CompositeTransform;
			IndicatorRing = GetTemplateChild("IndicatorRing") as ProgressRing;
			IndicatorOverlayBackground = GetTemplateChild("IndicatorOverlayBackground") as Ellipse;
			IndicatorInAnimation = Root.Resources["IndicatorIn"] as Storyboard;
			IndicatorOutAnimation = Root.Resources["IndicatorOut"] as Storyboard;
			IndicatorOverlayInAnimation = Root.Resources["IndicatorOverlayIn"] as Storyboard;
			IndicatorOverlayOutAnimation = Root.Resources["IndicatorOverlayOut"] as Storyboard;
			RefreshStartAnimation = Root.Resources["RefreshStart"] as Storyboard;
			RefreshCompleteAnimation = Root.Resources["RefreshComplete"] as Storyboard;

			var touch = new TouchCapabilities();

			if (touch.TouchPresent > 0)
			{
				Root.ManipulationDelta += OnRootManipulationDelta;
				Root.Loaded += OnRootLoaded;
				Root.PointerPressed += OnRootPointerPressed;
				Root.PointerReleased += OnRootPointerReleased;
				Root.PointerCanceled += OnRootPointerReleased;
				Root.PointerCaptureLost += OnRootPointerReleased;
				Root.PointerExited += OnRootPointerReleased;
				Main.VerticalScrollMode = ScrollMode.Disabled;
				Main.PointerWheelChanged += OnMainPointerWheelChanged;
			}
			else
			{
				Main.VerticalScrollMode = ScrollMode.Enabled;
				Indicator.Visibility = Visibility.Collapsed;
				Main.ManipulationMode = ManipulationModes.None;
			}
		}

		private void OnRootManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
		{
			if (_canSlide)
			{
				var delta = e.Delta.Translation.Y;
				if (Indicator.Height > 0 || (Main.VerticalOffset <= 0 && delta > 0 && _canTrigger))
				{
					double percent = Indicator.Height / IndicatorHeight;
					delta *= ElasticFactor * (1 - Math.Sqrt(1 - (1 - percent) * (1 - percent)));
					var height = Indicator.Height + delta;
					if (height < 0)
					{
						Indicator.Height = 0;
					}
					else
					{
						Indicator.Height = height;
						percent = Indicator.Height / TriggerOffset;
						if (!_triggered)
							IndicatorTransform.Rotation = 360 * percent;
						if (Indicator.Height > TriggerOffset && !_triggerStart)
						{
							_triggerStart = true;
							if (!_triggered)
								IndicatorInAnimation.Begin();
						}
						if (Indicator.Height < TriggerOffset && !_triggered)
						{
							_triggerStart = false;
							IndicatorOutAnimation.Begin();
						}
					}
				}
				else
				{
					Main.ChangeView(null, Main.VerticalOffset - delta, null);
				}
				e.Handled = true;
			}
		}

		private void OnRootLoaded(object sender, RoutedEventArgs e)
		{
			Indicator.Background = RefreshBackground;
			(IndicatorOverlayInAnimation.Children[0] as DoubleAnimation).To = TriggerOffset;
		}

		private void OnRootPointerPressed(object sender, PointerRoutedEventArgs e)
		{
			if (!IsRefreshingEnabled)
				return;

			_canSlide = true;
			_canTrigger = true;
		}

		private void OnRootPointerReleased(object sender, PointerRoutedEventArgs e)
		{
			if (Indicator.Height > 0)
			{
				_canSlide = false;
				if (_triggerStart)
				{
					IsRefreshing = true;
				}
				else
				{
					ResetScroll();
				}
			}
			else
			{
				_canTrigger = false;
			}
		}

		private void OnMainPointerWheelChanged(object sender, PointerRoutedEventArgs e)
		{
			var ptr = e.GetCurrentPoint(null);
			var ptrpr = ptr.Properties;
			var wheeldelta = ptrpr.MouseWheelDelta;
			var offset = Main.VerticalOffset - wheeldelta;
			Main.ChangeView(0, offset, 1);
		}

		private void RefreshStart()
		{
			if (!_triggered)
			{
				OnRefresh(this, new EventArgs());
				_triggered = true;
				RefreshStartAnimation.Begin();
			}
		}

		private void RefreshEnd()
		{
			ResetScroll();
			_triggered = false;
			IndicatorOutAnimation.Begin();
			IndicatorRing.IsActive = false;
			RefreshCompleteAnimation.Begin();
		}

		private void ResetScroll()
		{
			Main.ChangeView(0, 0, 1);
			IndicatorOverlayOutAnimation.Begin();
		}

		private void OnRefresh(object sender, EventArgs e)
		{
			Refresh?.Invoke(sender, e);
		}
	}
}
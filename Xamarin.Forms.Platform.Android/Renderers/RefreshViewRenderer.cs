using System;
using System.ComponentModel;
using Android.Content;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Views;
using Xamarin.Forms.Internals;
using AView = Android.Views.View;

namespace Xamarin.Forms.Platform.Android
{
	public class RefreshViewRenderer : SwipeRefreshLayout, IVisualElementRenderer, IEffectControlProvider, SwipeRefreshLayout.IOnRefreshListener
	{
		bool _init;
		bool _refreshing;
		IVisualElementRenderer _renderer;
		int? _defaultLabelFor;

		public event EventHandler<VisualElementChangedEventArgs> ElementChanged;
		public event EventHandler<PropertyChangedEventArgs> ElementPropertyChanged;

		public RefreshViewRenderer(Context context)
			: base(context)
		{

		}

		public VisualElementTracker Tracker { get; private set; }

		public ViewGroup ViewGroup => this;

		public AView View => this;

		public SwipeRefreshLayout SwipeRefreshLayout => View as SwipeRefreshLayout;

		public VisualElement Element { get; private set; }

		public RefreshView RefreshView => Element != null ? (RefreshView)Element : null;

		public override bool Refreshing
		{
			get
			{
				return _refreshing;
			}
			set
			{
				_refreshing = value;

				if (RefreshView != null && RefreshView.IsRefreshing != _refreshing)
					RefreshView.IsRefreshing = _refreshing;

				if (base.Refreshing == _refreshing)
					return;

				base.Refreshing = _refreshing;
			}
		}

		public override bool CanChildScrollUp() => CanScrollUp(_renderer.View);

		public void SetElement(VisualElement element)
		{
			var oldElement = Element;

			if (oldElement != null)
				oldElement.PropertyChanged -= HandlePropertyChanged;

			Element = element;

			if (Element != null)
			{
				UpdateContent();
				Element.PropertyChanged += HandlePropertyChanged;
			}

			if (!_init)
			{
				_init = true;
				Tracker = new VisualElementTracker(this);
				SetOnRefreshListener(this);
			}

			UpdateColors();
			UpdateIsRefreshing();
			UpdateIsEnabled();

			ElementChanged?.Invoke(this, new VisualElementChangedEventArgs(oldElement, Element));
			EffectUtilities.RegisterEffectControlProvider(this, oldElement, element);
		}

		void UpdateContent()
		{
			if (RefreshView.Content == null)
				return;

			if (_renderer != null)
			{
				_renderer.View.RemoveFromParent();
				_renderer.Dispose();
				_renderer = null;
			}

			if (RefreshView.Content != null)
			{
				_renderer = Platform.CreateRenderer(RefreshView.Content, Context);

				Platform.SetRenderer(RefreshView.Content, _renderer);

				if (_renderer.View.Parent != null)
					_renderer.View.RemoveFromParent();

				using (var layout = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent))
					SwipeRefreshLayout.AddView(_renderer.View, layout);
			}
		}

		void UpdateColors()
		{
			if (RefreshView == null)
				return;

			if (RefreshView.RefreshColor != Color.Default)
				SetColorSchemeColors(RefreshView.RefreshColor.ToAndroid());
			if (RefreshView.BackgroundColor != Color.Default)
				SetProgressBackgroundColorSchemeColor(RefreshView.BackgroundColor.ToAndroid());
		}

		void UpdateIsRefreshing() => Refreshing = RefreshView.IsRefreshing;

		void UpdateIsEnabled() => SwipeRefreshLayout.Enabled = RefreshView.IsEnabled;

		bool CanScrollUp(AView view)
		{
			if (!(view is ViewGroup viewGroup))
				return base.CanChildScrollUp();

			var sdk = (int)global::Android.OS.Build.VERSION.SdkInt;

			if (sdk >= 16)
			{
				if (viewGroup.IsScrollContainer)
				{
					return base.CanChildScrollUp();
				}
			}

			for (int i = 0; i < viewGroup.ChildCount; i++)
			{
				var child = viewGroup.GetChildAt(i);
				if (child is global::Android.Widget.AbsListView)
				{
					if (child is global::Android.Widget.AbsListView list)
					{
						if (list.FirstVisiblePosition == 0)
						{
							var subChild = list.GetChildAt(0);

							return subChild != null && subChild.Top != 0;
						}

						return true;
					}
				}
				else if (child is global::Android.Widget.ScrollView)
				{
					var scrollview = child as global::Android.Widget.ScrollView;
					return scrollview.ScrollY <= 0.0;
				}
				else if (child is global::Android.Webkit.WebView)
				{
					var webView = child as global::Android.Webkit.WebView;
					return webView.ScrollY > 0.0;
				}
				else if (child is SwipeRefreshLayout)
				{
					return CanScrollUp(child as ViewGroup);
				}
			}

			return false;
		}

		public void OnRefresh()
		{
			if (RefreshView?.Command?.CanExecute(RefreshView?.CommandParameter) ?? false)
			{
				RefreshView.Command.Execute(RefreshView?.CommandParameter);
			}
		}

		void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == ContentView.ContentProperty.PropertyName)
				UpdateContent();
			else if (e.PropertyName == VisualElement.IsEnabledProperty.PropertyName)
				UpdateIsEnabled();
			else if (e.PropertyName == RefreshView.IsRefreshingProperty.PropertyName)
				UpdateIsRefreshing();
			else if (e.IsOneOf(RefreshView.RefreshColorProperty, VisualElement.BackgroundColorProperty))
				UpdateColors();

			ElementPropertyChanged?.Invoke(sender, e);

		}

		public SizeRequest GetDesiredSize(int widthConstraint, int heightConstraint)
		{
			_renderer.View.Measure(widthConstraint, heightConstraint);

			return new SizeRequest(new Size(_renderer.View.MeasuredWidth, _renderer.View.MeasuredHeight), new Size(40, 40));
		}

		public void UpdateLayout() => Tracker?.UpdateLayout();

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			if (disposing)
			{
				if (Element != null)
				{
					Element.PropertyChanged -= HandlePropertyChanged;
				}

				if (_renderer != null)
					_renderer.View.RemoveFromParent();
			}

			_renderer?.Dispose();
			_renderer = null;

			Tracker?.Dispose();
			Tracker = null;

			_init = false;
		}

		public void SetLabelFor(int? id)
		{
			if (_defaultLabelFor == null)
			{
				_defaultLabelFor = ViewCompat.GetLabelFor(this);
			}

			ViewCompat.SetLabelFor(this, (int)(id ?? _defaultLabelFor));
		}

		void IEffectControlProvider.RegisterEffect(Effect effect)
		{
			if (effect is PlatformEffect platformEffect)
				OnRegisterEffect(platformEffect);
		}

		void OnRegisterEffect(PlatformEffect effect)
		{
			effect.SetContainer(this);
			effect.SetControl(this);
		}
	}
}
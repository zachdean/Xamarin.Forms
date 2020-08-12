using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.DualScreen
{
	public static class LayoutHingePaddingBehavior
	{
		public static readonly BindableProperty AttachBehaviorProperty =
			BindableProperty.CreateAttached(
				"AttachBehavior",
				typeof(bool),
				typeof(LayoutHingePaddingBehavior),
				false,
				propertyChanged: OnAttachBehaviorChanged);

		public static bool GetAttachBehavior(BindableObject view)
		{
			return (bool)view.GetValue(AttachBehaviorProperty);
		}

		public static void SetAttachBehavior(BindableObject view, bool value)
		{
			view.SetValue(AttachBehaviorProperty, value);
		}

		internal static readonly BindablePropertyKey DualScreenInfoPropertyKey =
			BindableProperty.CreateAttachedReadOnly(
				"DualScreenInfo",
				typeof(DualScreenInfo),
				typeof(LayoutHingePaddingBehavior),
				null,
				propertyChanged: OnDualScreenInfoChanged);

		public static readonly BindableProperty DualScreenInfoProperty = DualScreenInfoPropertyKey.BindableProperty;
		public static DualScreenInfo GetDualScreenInfoProperty(BindableObject view)
		{
			return (DualScreenInfo)view.GetValue(DualScreenInfoProperty);
		}

		static void OnAttachBehaviorChanged(BindableObject view, object oldValue, object newValue)
		{
			if (!(view is VisualElement ve))
			{
				return;
			}

			bool attachBehavior = (bool)newValue;
			if (attachBehavior)
			{
				view.SetValue(DualScreenInfoPropertyKey, new DualScreenInfo(ve));
			}
			else
			{
				view.SetValue(DualScreenInfoPropertyKey, null);
			}
		}

		static void OnDualScreenInfoPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{			
			if (e.PropertyName != nameof(DualScreenInfo.SpanningBounds) &&
				e.PropertyName != nameof(DualScreenInfo.HingeBounds))
				return;

			DualScreenInfo info = sender as DualScreenInfo;
			if (!(info.Element is Layout layout))
				return;

			if (layout.Parent == null)
				return;

			if (!layout.IsPlatformEnabled)
				return;

			if(info.HingeBounds == null || 
				info.HingeBounds == Rectangle.Zero || 
				info.SpanningBounds.Length < 2 ||
				info.SpanMode == TwoPaneViewMode.SinglePane)
			{
				layout.Padding = new Thickness(0, 0, 0, 0);
			}
			else
			{
				if(info.SpanMode == TwoPaneViewMode.Tall)
				{
					// place in bottom panel
					if(info.SpanningBounds[1].Height > info.SpanningBounds[0].Height)
					{
						var mh = info.HingeBounds.Height + info.SpanningBounds[0].Height;
						layout.Padding = new Thickness(0, mh, 0, 0);
					}
					// place in top panel
					else
					{
						var mh = info.SpanningBounds[1].Height;
						layout.Padding = new Thickness(0, 0, 0, mh);
					}
				}
				else
				{
					// place in right panel
					if (info.SpanningBounds[1].Width > info.SpanningBounds[0].Width)
					{
						var mh = info.HingeBounds.Width + info.SpanningBounds[0].Width;
						layout.Padding = new Thickness(0, 0, mh, 0);
					}
					// place in left panel
					else
					{
						var mh = info.SpanningBounds[1].Width;
						layout.Padding = new Thickness(0, mh, 0, 0);
					}
				}
			}
		}

		static void OnDualScreenInfoChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (oldValue is DualScreenInfo oldInfo)
				oldInfo.PropertyChanged -= OnDualScreenInfoPropertyChanged;

			if (newValue is DualScreenInfo newInfo)
				newInfo.PropertyChanged += OnDualScreenInfoPropertyChanged;
		}
	}
}

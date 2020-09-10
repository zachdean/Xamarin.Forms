using Android.Graphics.Drawables;
using Android.Widget;
using ASwitch = Android.Widget.Switch;

namespace System.Maui.Platform
{
	public partial class SwitchHandler : AbstractViewHandler<ISwitch, ASwitch>
	{
		Drawable _defaultTrackDrawable;
		bool _changedThumbColor;
		OnListener _onListener;

		protected override ASwitch CreateView()
		{
			_onListener = new OnListener(this);
			var aswitch = new ASwitch(Context);
			aswitch.SetOnCheckedChangeListener(_onListener);
			return aswitch;
		}

		protected override void SetupDefaults()
		{
			_defaultTrackDrawable = TypedNativeView.TrackDrawable;
			base.SetupDefaults();
		}

		protected override void DisposeView(ASwitch nativeView)
		{
			if (_onListener != null)
			{
				nativeView.SetOnCheckedChangeListener(null);
				_onListener = null;
			}
			base.DisposeView(nativeView);
		}

		public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			SizeRequest sizeConstraint = base.GetDesiredSize(widthConstraint, heightConstraint);

			if (sizeConstraint.Request.Width == 0)
			{
				int width = (int)widthConstraint;
				if (widthConstraint <= 0)
					width = (int)Context.GetThemeAttributeDp(global::Android.Resource.Attribute.SwitchMinWidth);

				sizeConstraint = new SizeRequest(new Size(width, sizeConstraint.Request.Height), new Size(width, sizeConstraint.Minimum.Height));
			}

			return sizeConstraint;
		}

		public static void MapPropertyIsToggled(IViewHandler Handler, ISwitch view)
		{
			(Handler as SwitchHandler)?.UpdateIsToggled();
		}

		public static void MapPropertyOnColor(IViewHandler Handler, ISwitch view)
		{
			(Handler as SwitchHandler)?.UpdateOnColor();
		}

		public static void MapPropertyThumbColor(IViewHandler Handler, ISwitch view)
		{
			(Handler as SwitchHandler)?.UpdateThumbColor();
		}

		public virtual void SetIsOn(bool isChecked) => VirtualView.IsToggled = isChecked;

		public virtual void UpdateIsToggled()
		{
			TypedNativeView.Checked = VirtualView.IsToggled;
		}

		public virtual void UpdateOnColor()
		{
			if (TypedNativeView.Checked)
			{
				var onColor = VirtualView.OnColor;

				if (onColor.IsDefault)
				{
					TypedNativeView.TrackDrawable = _defaultTrackDrawable;
				}
				else
				{
					TypedNativeView.TrackDrawable?.SetColorFilter(onColor.ToNative(), FilterMode.Multiply);
				}
			}
			else
			{
				TypedNativeView.TrackDrawable?.ClearColorFilter();
			}
		}

		public virtual void UpdateThumbColor()
		{
			var thumbColor = VirtualView.ThumbColor;

			if (!thumbColor.IsDefault)
			{
				TypedNativeView.ThumbDrawable.SetColorFilter(thumbColor, FilterMode.Multiply);
				_changedThumbColor = true;
			}
			else
			{
				if (_changedThumbColor)
				{
					TypedNativeView.ThumbDrawable?.ClearColorFilter();
					_changedThumbColor = false;
				}
			}

			TypedNativeView.ThumbDrawable.SetColorFilter(thumbColor, FilterMode.Multiply);
		}
	}

	class OnListener : Java.Lang.Object, CompoundButton.IOnCheckedChangeListener
	{
        readonly SwitchHandler _switchHandler;

		public OnListener(SwitchHandler switchHandler)
		{
			_switchHandler = switchHandler;
		}

		void CompoundButton.IOnCheckedChangeListener.OnCheckedChanged(CompoundButton buttonView, bool isChecked)
		{
			_switchHandler.UpdateOnColor();
		}
	}
}

using System.Drawing;
using UIKit;

namespace System.Maui.Platform
{
	public partial class SwitchHandler : AbstractViewHandler<ISwitch, UISwitch>
	{
		UIColor _defaultOnColor;
		UIColor _defaultThumbColor;

		protected override UISwitch CreateView()
		{
			var nativeView = new UISwitch(RectangleF.Empty);
			nativeView.ValueChanged += UISwitchValueChanged;
			return nativeView;
		}

		protected override void SetupDefaults()
		{
			_defaultOnColor = UISwitch.Appearance.OnTintColor;
			_defaultThumbColor = UISwitch.Appearance.ThumbTintColor;
			base.SetupDefaults();
		}

		protected override void DisposeView(UISwitch nativeView)
		{
			nativeView.ValueChanged -= UISwitchValueChanged;
			base.DisposeView(nativeView);
		}

		public static void MapPropertyIsToggled(IViewHandler Handler, ISwitch view)
		{
			(Handler as SwitchHandler)?.UpdateIsToggled();
		}

		public virtual void SetIsOn() =>
			VirtualView.IsToggled = TypedNativeView.On;

		public static void MapPropertyOnColor(IViewHandler Handler, ISwitch view)
		{
			(Handler as SwitchHandler)?.UpdateOnColor();
		}

		public static void MapPropertyThumbColor(IViewHandler Handler, ISwitch view)
		{
			(Handler as SwitchHandler)?.UpdateThumbColor();
		}

		public virtual void UpdateIsToggled()
        {
			TypedNativeView.SetState(VirtualView.IsToggled, true);
		}

		public virtual void UpdateOnColor()
		{
			var onColor = VirtualView.OnColor;
			TypedNativeView.OnTintColor = onColor.IsDefault ? _defaultOnColor : onColor.ToNative();
		}

		public virtual void UpdateThumbColor()
		{
			var thumbColor = VirtualView.ThumbColor;
			TypedNativeView.ThumbTintColor = thumbColor.IsDefault ? _defaultThumbColor : thumbColor.ToNative();
		}

		void UISwitchValueChanged(object sender, EventArgs e)
		{
			SetIsOn();
		}
	}
}

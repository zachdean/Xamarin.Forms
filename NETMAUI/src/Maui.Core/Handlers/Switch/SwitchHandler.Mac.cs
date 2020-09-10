using AppKit;

namespace System.Maui.Platform
{
	public partial class SwitchHandler : AbstractViewHandler<ISwitch, NSView>
	{
		protected override NSView CreateView()
		{
			if (NativeVersion.IsAtLeast(15))
			{
				var nativeSwitch = new NSSwitch();

				nativeSwitch.Activated += OnControlActivated;

				return nativeSwitch;
			}

			var nativeButton = new NSButton
			{
				AllowsMixedState = false,
				Title = string.Empty
			};

			nativeButton.SetButtonType(NSButtonType.Switch);

			nativeButton.Activated += OnControlActivated;

			return nativeButton;
		}

		protected override void DisposeView(NSView nativeView)
		{
			if (NativeVersion.IsAtLeast(15))
				((NSSwitch)TypedNativeView).Activated -= OnControlActivated;
			else
				((NSButton)TypedNativeView).Activated -= OnControlActivated;

			base.DisposeView(nativeView);
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

		public virtual void UpdateIsToggled()
        {
			if (NativeVersion.IsAtLeast(15))
				((NSSwitch)TypedNativeView).State = VirtualView.IsToggled ? 1 : 0;
			else
				((NSButton)TypedNativeView).State = VirtualView.IsToggled ? NSCellStateValue.On : NSCellStateValue.Off;
		}

		public virtual void UpdateOnColor()
		{
			// TODO:
		}

		public virtual void UpdateThumbColor()
		{
			// TODO:
		}

		public virtual void SetIsOn()
		{
			if (NativeVersion.IsAtLeast(15))
				VirtualView.IsToggled = ((NSSwitch)TypedNativeView).State == 1;
			else
				VirtualView.IsToggled = ((NSButton)TypedNativeView).State == NSCellStateValue.On;
		}

		void OnControlActivated(object sender, EventArgs e)
		{
			SetIsOn();
		}
	}
}
using System;
using AppKit;

namespace Xamarin.Platform.Handlers
{
	public partial class SwitchHandler : AbstractViewHandler<ISwitch, NSView>
	{
		protected override NSView CreateNativeView()
		{
			if (NativeVersion.IsAtLeast(15))
			{
				return new NSSwitch();
			}

			var nativeButton = new NSButton
			{
				AllowsMixedState = false,
				Title = string.Empty
			};

			nativeButton.SetButtonType(NSButtonType.Switch);

			return nativeButton;
		}

		protected override void ConnectHandler(NSView nativeView)
		{
			if (NativeVersion.IsAtLeast(15))
				((NSSwitch)nativeView).Activated += OnControlActivated;
			else
				((NSButton)nativeView).Activated += OnControlActivated;
		}

		protected override void DisconnectHandler(NSView nativeView)
		{
			if (NativeVersion.IsAtLeast(15))
				((NSSwitch)nativeView).Activated -= OnControlActivated;
			else
				((NSButton)nativeView).Activated -= OnControlActivated;
		}

		void OnControlActivated(object sender, EventArgs e)
		{
			if (VirtualView == null)
				return;

			if (NativeVersion.IsAtLeast(15))
			{
				if (TypedNativeView is NSSwitch nSSwitch)
					VirtualView.IsToggled = nSSwitch.State == 1;
			}
			else
			{
				if (TypedNativeView is NSButton nSButton)
					VirtualView.IsToggled = nSButton.State == NSCellStateValue.On;
			}
		}
	}
}
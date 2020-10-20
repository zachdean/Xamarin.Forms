using AppKit;

namespace Xamarin.Platform
{
	public static class SwitchExtensions
	{
		public static void UpdateIsToggled(this NSView nativeView, ISwitch view)
		{
			if (NativeVersion.IsAtLeast(15))
				((NSSwitch)nativeView).State = view.IsToggled ? 1 : 0;
			else
				((NSButton)nativeView).State = view.IsToggled ? NSCellStateValue.On : NSCellStateValue.Off;
		}

		public static void UpdateOnColor(this NSView nativeView, ISwitch view)
		{
			
		}

		public static void UpdateThumbColor(this NSView nativeView, ISwitch view)
		{
			
		}
	}
}
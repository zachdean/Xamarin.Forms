using AppKit;
using Foundation;

namespace System.Maui.Controls.Primitives
{
	class NativeEntry : NSTextField
	{
		public EventHandler<BoolEventArgs> FocusChanged;

		public EventHandler Completed;

		public override void DidEndEditing(NSNotification notification)
		{
			if (CurrentEditor != Window.FirstResponder)
				FocusChanged?.Invoke(this, new BoolEventArgs(false));

			base.DidEndEditing(notification);
		}

		public override void KeyUp(NSEvent theEvent)
		{
			base.KeyUp(theEvent);

			if (theEvent.KeyCode == (ushort)NSKey.Return)
				Completed?.Invoke(this, EventArgs.Empty);
		}
	}

	internal class BoolEventArgs : EventArgs
	{
		public BoolEventArgs(bool value)
		{
			Value = value;
		}
		public bool Value
		{
			get;
			private set;
		}
	}
}
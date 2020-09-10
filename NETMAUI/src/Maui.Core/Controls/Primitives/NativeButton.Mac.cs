using AppKit;
using CoreGraphics;
using Foundation;

namespace System.Maui.Controls.Primitives
{
	public class NativeButton : NSButton
	{
		class NativeButtonCell : NSButtonCell
		{
			public override CGRect DrawTitle(NSAttributedString title, CGRect frame, NSView controlView)
			{
				if (controlView is NativeButton button)
				{
					var paddedFrame = new CGRect(frame.X + button._leftPadding,
						frame.Y + button._topPadding,
						frame.Width - button._leftPadding - button._rightPadding,
						frame.Height - button._topPadding - button._bottomPadding);
					return base.DrawTitle(title, paddedFrame, controlView);
				}
				return base.DrawTitle(title, frame, controlView);
			}
		}

		public NativeButton()
		{
			Cell = new NativeButtonCell();
		}

		public event Action Pressed;

		public event Action Released;

		public override void MouseDown(NSEvent theEvent)
		{
			Pressed?.Invoke();

			base.MouseDown(theEvent);

			Released?.Invoke();
		}

		nfloat _leftPadding;
		nfloat _topPadding;
		nfloat _rightPadding;
		nfloat _bottomPadding;

		internal void UpdatePadding(Thickness padding)
		{
			_leftPadding = (nfloat)padding.Left;
			_topPadding = (nfloat)padding.Top;
			_rightPadding = (nfloat)padding.Right;
			_bottomPadding = (nfloat)padding.Bottom;

			InvalidateIntrinsicContentSize();
		}

		public override CGSize IntrinsicContentSize
		{
			get
			{
				var baseSize = base.IntrinsicContentSize;
				return new CGSize(baseSize.Width + _leftPadding + _rightPadding, baseSize.Height + _topPadding + _bottomPadding);
			}
		}
	}
}
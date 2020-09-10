using Android.Widget;
using Android.Content;
using Android.Views;

namespace System.Maui.Controls.Primitives
{
	public class NativeSlider : SeekBar
	{
		bool _isTouching = false;

		public NativeSlider(Context context) : base(context)
		{
			// This should work, but it doesn't.
			DuplicateParentStateEnabled = false;
		}

		public override bool OnTouchEvent(MotionEvent e)
		{
			switch (e.Action)
			{
				case MotionEventActions.Down:
					_isTouching = true;
					break;
				case MotionEventActions.Up:
					Pressed = false;
					break;
			}
				
			return base.OnTouchEvent(e);
		}

		public override bool Pressed
		{
			get
			{
				return base.Pressed;
			}
			set
			{
				if (_isTouching)
				{
					base.Pressed = value;
					_isTouching = value;
				}
			}
		}
	}
}
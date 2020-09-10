using Android.Content;
using Android.Views;
using Android.Widget;

namespace System.Maui.Controls.Primitives
{
	internal class ConditionalFocusLayout : LinearLayout, global::Android.Views.View.IOnTouchListener
	{
		public ConditionalFocusLayout(System.IntPtr p, global::Android.Runtime.JniHandleOwnership o) : base(p, o)
		{
			// Added default constructor to prevent crash when accessing selected row in ListViewAdapter.Dispose
		}

		public ConditionalFocusLayout(Context context) : base(context)
		{
			SetOnTouchListener(this);
		}

		public bool OnTouch(global::Android.Views.View v, MotionEvent e)
		{
			bool allowFocus = v is EditText;
			DescendantFocusability = allowFocus ? DescendantFocusability.AfterDescendants : DescendantFocusability.BlockDescendants;
			return false;
		}
	}
}
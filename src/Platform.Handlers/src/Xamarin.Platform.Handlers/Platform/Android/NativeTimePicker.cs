using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using static Android.Views.View;

namespace Xamarin.Platform
{
	public class NativeTimePicker : EditText, IOnClickListener
	{
		public NativeTimePicker(Context? context) : base(context)
		{
			Initialize();
		}

		public NativeTimePicker(Context? context, IAttributeSet attrs) : base(context, attrs)
		{
			Initialize();
		}

		public NativeTimePicker(Context? context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
			Initialize();
		}

		protected NativeTimePicker(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{
		}

		private void Initialize()
		{
			Focusable = true;
			SetOnClickListener(this);
		}

		public Action? ShowPicker { get; set; }
		public Action? HidePicker { get; set; }

		public void OnClick(View? v)
		{
			ShowPicker?.Invoke();
		}
	}
}
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.Text;
using Java.Lang;
using System.Collections.Generic;

namespace Xamarin.Forms.Platform.Android
{
	public class PickerEditText : EditText
	{
		HashSet<Keycode> availableKeys = new HashSet<Keycode>(new[] {
			Keycode.Tab, Keycode.Forward, Keycode.Back, Keycode.DpadDown, Keycode.DpadLeft, Keycode.DpadRight, Keycode.DpadUp
		});

		readonly IPickerRenderer owner;

		public PickerEditText(Context context, IPickerRenderer pickerRenderer) : base(context)
		{
			Focusable = true;
			Clickable = true;
			InputType = InputTypes.Null;
			KeyPress += OnKeyPress;
			owner = pickerRenderer;
			SetOnClickListener(PickerListener.Instance);
		}

		private void OnKeyPress(object sender, KeyEventArgs e)
		{
			if (availableKeys.Contains(e.KeyCode))
			{
				e.Handled = false;
				return;
			}
			e.Handled = true;
			CallOnClick();
		}

		public override bool OnTouchEvent(MotionEvent e)
		{
			if (e.Action == MotionEventActions.Up && !IsFocused)
				RequestFocus();
			return base.OnTouchEvent(e); // raises the OnClick event if focus is already received
		}

		class PickerListener : Object, IOnClickListener
		{
			#region Statics
			public static readonly PickerListener Instance = new PickerListener();
			#endregion

			public void OnClick(global::Android.Views.View v)
			{
				(v as PickerEditText)?.owner.OnClick();
			}
		}
	}
}
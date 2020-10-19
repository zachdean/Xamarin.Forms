using System;
using UIKit;

namespace Xamarin.Platform
{
	public static class TimePickerExtensions
	{
		public static void UpdateFormat(this NativeTimePicker nativeTimePicker, ITimePicker timePicker)
		{
			nativeTimePicker.UpdateTime(timePicker, null);
			nativeTimePicker.UpdateCharacterSpacing(timePicker);
		}

		public static void UpdateFormat(this NativeTimePicker nativeTimePicker, ITimePicker timePicker, UIDatePicker? picker)
		{
			nativeTimePicker.UpdateTime(timePicker, picker);
			nativeTimePicker.UpdateCharacterSpacing(timePicker);
		}

		public static void UpdateTime(this NativeTimePicker nativeTimePicker, ITimePicker timePicker)
		{
			nativeTimePicker.UpdateTime(timePicker, null);
			nativeTimePicker.UpdateCharacterSpacing(timePicker);
		}

		public static void UpdateTime(this NativeTimePicker nativeTimePicker, ITimePicker timePicker, UIDatePicker? picker)
		{
			if (picker != null)
				picker.Date = new DateTime(1, 1, 1).Add(timePicker.Time).ToNSDate();

			nativeTimePicker.Text = DateTime.Today.Add(timePicker.Time).ToString(timePicker.Format);

			nativeTimePicker.UpdateCharacterSpacing(timePicker);
		}

		public static void UpdateTextColor(this NativeTimePicker nativeTimePicker, ITimePicker timePicker)
		{
			nativeTimePicker.UpdateTextColor(timePicker, null);
		}

		public static void UpdateTextColor(this NativeTimePicker nativeTimePicker, ITimePicker timePicker, UIColor? defaultTextColor)
		{
			var textColor = timePicker.Color;

			if (defaultTextColor != null && (textColor.IsDefault || !timePicker.IsEnabled))
				nativeTimePicker.TextColor = defaultTextColor;
			else
				nativeTimePicker.TextColor = textColor.ToNative();

			// HACK: This forces the color to update; there's probably a more elegant way to make this happen
			nativeTimePicker.Text = nativeTimePicker.Text;
		}

		public static void UpdateFont(this NativeTimePicker nativeTimePicker, ITimePicker timePicker)
		{
			nativeTimePicker.Font = timePicker.ToUIFont();
		}

		public static void UpdateCharacterSpacing(this NativeTimePicker nativeTimePicker, ITimePicker timePicker)
		{
			var textAttr = nativeTimePicker.AttributedText?.AddCharacterSpacing(nativeTimePicker.Text ?? string.Empty, timePicker.CharacterSpacing);

			if (textAttr != null)
				nativeTimePicker.AttributedText = textAttr;
		}
	}
}
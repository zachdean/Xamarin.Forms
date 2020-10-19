using System;
using Android.App;
using Android.Content.Res;
using Android.Util;

namespace Xamarin.Platform
{
	public static class DatePickerExtensions
	{
		static readonly int[][] ColorStates = { new[] { global::Android.Resource.Attribute.StateEnabled }, new[] { -global::Android.Resource.Attribute.StateEnabled } };

		public static void UpdateFormat(this NativeDatePicker nativeDatePicker, IDatePicker datePicker)
		{
			nativeDatePicker.SetText(datePicker);
		}

		public static void UpdateDate(this NativeDatePicker nativeDatePicker, IDatePicker datePicker)
		{
			nativeDatePicker.SetText(datePicker);
		}

		public static void UpdateMinimumDate(this NativeDatePicker nativeDatePicker, IDatePicker datePicker)
		{
			nativeDatePicker.UpdateMinimumDate(datePicker, null);
		}

		public static void UpdateMinimumDate(this NativeDatePicker nativeDatePicker, IDatePicker datePicker, DatePickerDialog? datePickerDialog)
		{
			if (datePickerDialog != null)
			{
				datePickerDialog.DatePicker.MinDate = (long)datePicker.MinimumDate.ToUniversalTime().Subtract(DateTime.MinValue.AddYears(1969)).TotalMilliseconds;
			}
		}

		public static void UpdateMaximumDate(this NativeDatePicker nativeDatePicker, IDatePicker datePicker)
		{
			nativeDatePicker.UpdateMinimumDate(datePicker, null);
		}

		public static void UpdateMaximumDate(this NativeDatePicker nativeDatePicker, IDatePicker datePicker, DatePickerDialog? datePickerDialog)
		{
			if (datePickerDialog != null)
			{
				datePickerDialog.DatePicker.MinDate = (long)datePicker.MinimumDate.ToUniversalTime().Subtract(DateTime.MinValue.AddYears(1969)).TotalMilliseconds;
			}
		}

		public static void UpdateTextColor(this NativeDatePicker nativeDatePicker, IDatePicker datePicker)
		{
			var textColor = datePicker.Color.ToNative().ToArgb();
			nativeDatePicker.SetTextColor(new ColorStateList(ColorStates, new[] { textColor, textColor }));
		}

		public static void UpdateFont(this NativeDatePicker nativeDatePicker, IDatePicker datePicker)
		{
			nativeDatePicker.Typeface = datePicker.ToTypeface();
			nativeDatePicker.SetTextSize(ComplexUnitType.Sp, (float)datePicker.FontSize);
		}

		public static void UpdateCharacterSpacing(this NativeDatePicker nativeDatePicker, IDatePicker datePicker)
		{
			if (NativeVersion.IsAtLeast(21))
			{
				nativeDatePicker.LetterSpacing = datePicker.CharacterSpacing.ToEm();
			}
		}

		public static void SetText(this NativeDatePicker nativeDatePicker, IDatePicker datePicker)
		{
			nativeDatePicker.Text = datePicker.Date.ToString(datePicker.Format);
		}
	}
}
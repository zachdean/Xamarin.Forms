using System;
using Android.Content.Res;
using Android.Text.Format;
using Android.Util;

namespace Xamarin.Platform
{
	public static class TimePickerExtensions
	{
		static readonly int[][] ColorStates = { new[] { global::Android.Resource.Attribute.StateEnabled }, new[] { -global::Android.Resource.Attribute.StateEnabled } };

		public static void UpdateFormat(this NativeTimePicker nativeTimePicker, ITimePicker timePicker)
		{
			SetTime(nativeTimePicker, timePicker);
		}

		public static void UpdateTime(this NativeTimePicker nativeTimePicker, ITimePicker timePicker)
		{
			SetTime(nativeTimePicker, timePicker);
		}

		public static void UpdateTextColor(this NativeTimePicker nativeTimePicker, ITimePicker timePicker)
		{
			var textColor = timePicker.Color.ToNative().ToArgb();
			nativeTimePicker.SetTextColor(new ColorStateList(ColorStates, new[] { textColor, textColor }));
		}

		public static void UpdateFont(this NativeTimePicker nativeTimePicker, ITimePicker timePicker)
		{
			nativeTimePicker.Typeface = timePicker.ToTypeface();
			nativeTimePicker.SetTextSize(ComplexUnitType.Sp, (float)timePicker.FontSize);
		}

		public static void UpdateCharacterSpacing(this NativeTimePicker nativeTimePicker, ITimePicker timePicker)
		{
			if (NativeVersion.IsAtLeast(21))
			{
				nativeTimePicker.LetterSpacing = timePicker.CharacterSpacing.ToEm();
			}
		}

		public static bool Is24HourView(this NativeTimePicker nativeTimePicker, ITimePicker? timePicker)
		{
			return timePicker != null && (DateFormat.Is24HourFormat(nativeTimePicker.Context) && timePicker.Format == "t" || timePicker.Format == "HH:mm");
		}

		internal static void SetTime(this NativeTimePicker nativeTimePicker, ITimePicker timePicker)
		{
			var timeFormat = nativeTimePicker.Is24HourView(timePicker) ? "HH:mm" : timePicker.Format;
			nativeTimePicker.Text = DateTime.Today.Add(timePicker.Time).ToString(timeFormat);
		}
	}
}
using UIKit;

namespace Xamarin.Platform
{
	public static class DatePickerExtensions
	{
		public static void UpdateFormat(this NativeDatePicker nativeDatePicker, IDatePicker datePicker)
		{
			nativeDatePicker.UpdateDate(datePicker, null);
		}

		public static void UpdateFormat(this NativeDatePicker nativeDatePicker, IDatePicker datePicker, UIDatePicker? picker)
		{
			nativeDatePicker.UpdateDate(datePicker, picker);
		}

		public static void UpdateDate(this NativeDatePicker nativeDatePicker, IDatePicker datePicker)
		{
			nativeDatePicker.UpdateDate(datePicker, null);
		}

		public static void UpdateDate(this NativeDatePicker nativeDatePicker, IDatePicker datePicker, UIDatePicker? picker)
		{
			if (picker != null && picker.Date.ToDateTime().Date != datePicker.Date.Date)
				picker.SetDate(datePicker.Date.ToNSDate(), false);

			nativeDatePicker.Text = datePicker.Date.ToString(datePicker.Format);

			nativeDatePicker.UpdateCharacterSpacing(datePicker);
		}

		public static void UpdateMinimumDate(this NativeDatePicker nativeDatePicker, IDatePicker datePicker)
		{
			nativeDatePicker.UpdateMinimumDate(datePicker, null);
		}

		public static void UpdateMinimumDate(this NativeDatePicker nativeDatePicker, IDatePicker datePicker, UIDatePicker? picker)
		{
			if (picker != null)
			{
				picker.MinimumDate = datePicker.MinimumDate.ToNSDate();
			}
		}

		public static void UpdateMaximumDate(this NativeDatePicker nativeDatePicker, IDatePicker datePicker)
		{
			nativeDatePicker.UpdateMaximumDate(datePicker, null);
		}

		public static void UpdateMaximumDate(this NativeDatePicker nativeDatePicker, IDatePicker datePicker, UIDatePicker? picker)
		{
			if (picker != null)
			{
				picker.MaximumDate = datePicker.MaximumDate.ToNSDate();
			}
		}

		public static void UpdateTextColor(this NativeDatePicker nativeDatePicker, IDatePicker datePicker)
		{
			nativeDatePicker.UpdateTextColor(datePicker);
		}

		public static void UpdateTextColor(this NativeDatePicker nativeDatePicker, IDatePicker datePicker, UIColor? defaultTextColor)
		{
			var textColor = datePicker.Color;

			if (defaultTextColor != null && (textColor.IsDefault || !datePicker.IsEnabled))
				nativeDatePicker.TextColor = defaultTextColor;
			else
				nativeDatePicker.TextColor = textColor.ToNative();

			// HACK This forces the color to update; there's probably a more elegant way to make this happen
			nativeDatePicker.Text = nativeDatePicker.Text;
		}

		public static void UpdateFont(this NativeDatePicker nativeDatePicker, IDatePicker datePicker)
		{
			nativeDatePicker.Font = datePicker.ToUIFont();
		}

		public static void UpdateCharacterSpacing(this NativeDatePicker nativeDatePicker, IDatePicker datePicker)
		{
			var textAttr = nativeDatePicker.AttributedText?.AddCharacterSpacing(nativeDatePicker.Text ?? string.Empty, datePicker.CharacterSpacing);

			if (textAttr != null)
				nativeDatePicker.AttributedText = textAttr;
		}
	}
}
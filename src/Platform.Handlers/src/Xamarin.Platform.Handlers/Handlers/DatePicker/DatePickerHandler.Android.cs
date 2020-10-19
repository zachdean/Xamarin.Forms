using Android.App;

namespace Xamarin.Platform.Handlers
{
	public partial class DatePickerHandler : AbstractViewHandler<IDatePicker, NativeDatePicker>
	{
		NativeDatePicker? _timePicker;
		static AlertDialog? Dialog;

		protected override NativeDatePicker CreateView()
		{
			_timePicker = new NativeDatePicker(Context)
			{
				ShowPicker = ShowPickerDialog,
				HidePicker = HidePickerDialog
			};

			return _timePicker;
		}

		public override void TearDown()
		{
			if (Dialog != null)
			{
				Dialog.Hide();
				Dialog.Dispose();
				Dialog = null;
			}

			base.TearDown();
		}

		protected virtual DatePickerDialog CreateDatePickerDialog(int year, int month, int day)
		{
			var dialog = new DatePickerDialog(Context!, (o, e) =>
			{
				if (VirtualView != null)
					VirtualView.Date = e.Date;
			}, year, month, day);

			return dialog;
		}

		public static void MapFormat(DatePickerHandler handler, IDatePicker datePicker)
		{
			ViewHandler.CheckParameters(handler, datePicker);
			handler.TypedNativeView?.UpdateFormat(datePicker);
		}

		public static void MapDate(DatePickerHandler handler, IDatePicker datePicker)
		{
			ViewHandler.CheckParameters(handler, datePicker);
			handler.TypedNativeView?.UpdateDate(datePicker);
		}

		public static void MapMinimumDate(DatePickerHandler handler, IDatePicker datePicker)
		{
			ViewHandler.CheckParameters(handler, datePicker);
			handler.TypedNativeView?.UpdateMinimumDate(datePicker, Dialog as DatePickerDialog);
		}

		public static void MapMaximumDate(DatePickerHandler handler, IDatePicker datePicker)
		{
			ViewHandler.CheckParameters(handler, datePicker);
			handler.TypedNativeView?.UpdateMaximumDate(datePicker, Dialog as DatePickerDialog);
		}

		public static void MapColor(DatePickerHandler handler, IDatePicker datePicker)
		{
			ViewHandler.CheckParameters(handler, datePicker);
			handler.TypedNativeView?.UpdateTextColor(datePicker);
		}

		public static void MapFont(DatePickerHandler handler, IDatePicker datePicker)
		{
			ViewHandler.CheckParameters(handler, datePicker);
			handler.TypedNativeView?.UpdateFont(datePicker);
		}

		public static void MapCharacterSpacing(DatePickerHandler handler, IDatePicker datePicker)
		{
			ViewHandler.CheckParameters(handler, datePicker);
			handler.TypedNativeView?.UpdateCharacterSpacing(datePicker);
		}

		void ShowPickerDialog()
		{
			if (VirtualView == null)
				return;

			var date = VirtualView.Date;
			ShowPickerDialog(date.Year, date.Month, date.Day);
		}

		void ShowPickerDialog(int year, int month, int day)
		{
			Dialog = CreateDatePickerDialog(year, month, day);
			Dialog.Show();
		}

		void HidePickerDialog()
		{
			Dialog?.Hide();
		}
	}
}
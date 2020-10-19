using System;
using Android.App;

namespace Xamarin.Platform.Handlers
{
	public partial class TimePickerHandler : AbstractViewHandler<ITimePicker, NativeTimePicker>
	{
		NativeTimePicker? _timePicker;
		AlertDialog? _dialog;

		protected override NativeTimePicker CreateView()
		{
			_timePicker = new NativeTimePicker(Context)
			{
				ShowPicker = ShowPickerDialog,
				HidePicker = HidePickerDialog
			};

			return _timePicker;
		}

		public override void TearDown()
		{
			if (_dialog != null)
			{
				_dialog.Hide();
				_dialog = null;
			}

			base.TearDown();
		}

		protected virtual TimePickerDialog CreateTimePickerDialog(int hour, int minute)
		{
			void onTimeSetCallback(object obj, TimePickerDialog.TimeSetEventArgs args)
			{
				if (VirtualView == null || TypedNativeView == null)
					return;

				VirtualView.Time = new TimeSpan(args.HourOfDay, args.Minute, 0);
			}

			var dialog = new TimePickerDialog(Context!, onTimeSetCallback, hour, minute, _timePicker != null && _timePicker.Is24HourView(VirtualView));

			return dialog;
		}

		public static void MapFormat(TimePickerHandler handler, ITimePicker timePicker)
		{
			ViewHandler.CheckParameters(handler, timePicker);
			handler.TypedNativeView?.UpdateFormat(timePicker);
		}

		public static void MapTime(TimePickerHandler handler, ITimePicker timePicker)
		{
			ViewHandler.CheckParameters(handler, timePicker);
			handler.TypedNativeView?.UpdateTime(timePicker);
		}

		public static void MapColor(TimePickerHandler handler, ITimePicker timePicker)
		{
			ViewHandler.CheckParameters(handler, timePicker);
			handler.TypedNativeView?.UpdateTextColor(timePicker);
		}

		public static void MapFont(TimePickerHandler handler, ITimePicker timePicker)
		{
			ViewHandler.CheckParameters(handler, timePicker);
			handler.TypedNativeView?.UpdateFont(timePicker);
		}

		public static void MapCharacterSpacing(TimePickerHandler handler, ITimePicker timePicker)
		{
			ViewHandler.CheckParameters(handler, timePicker);
			handler.TypedNativeView?.UpdateCharacterSpacing(timePicker);
		}

		void ShowPickerDialog()
		{
			if (VirtualView == null)
				return;

			var time = VirtualView.Time;
			ShowPickerDialog(time.Hours, time.Minutes);
		}

		// This overload is here so we can pass in the current values from the dialog 
		// on an orientation change (so that orientation changes don't cause the user's date selection progress
		// to be lost). Not useful until we have orientation changed events.
		void ShowPickerDialog(int hour, int minute)
		{
			_dialog = CreateTimePickerDialog(hour, minute);
			_dialog.Show();
		}

		void HidePickerDialog()
		{
			_dialog?.Hide();
		}
	}
}
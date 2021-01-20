using System;

namespace Xamarin.Platform.Handlers
{
	public partial class TimePickerHandler : AbstractViewHandler<ITimePicker, object>
	{
		protected override object CreateNativeView() => throw new NotImplementedException();

		public static void MapFormat(TimePickerHandler handler, ITimePicker timePicker) { }
		public static void MapTime(TimePickerHandler handler, ITimePicker timePicker) { }
		public static void MapColor(TimePickerHandler handler, ITimePicker timePicker) { }
		public static void MapFont(TimePickerHandler handler, ITimePicker timePicker) { }
		public static void MapCharacterSpacing(TimePickerHandler handler, ITimePicker timePicker) { }
	}
}
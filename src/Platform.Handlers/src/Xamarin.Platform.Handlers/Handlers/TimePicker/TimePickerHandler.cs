namespace Xamarin.Platform.Handlers
{
	public partial class TimePickerHandler
	{
		public static PropertyMapper<ITimePicker, TimePickerHandler> TimePickerMapper = new PropertyMapper<ITimePicker, TimePickerHandler>(ViewHandler.ViewMapper)
		{
			[nameof(ITimePicker.Format)] = MapFormat,
			[nameof(ITimePicker.Time)] = MapTime,
			[nameof(ITimePicker.Color)] = MapColor,
			[nameof(ITimePicker.Font)] = MapFont,
			[nameof(ITimePicker.FontAttributes)] = MapFont,
			[nameof(ITimePicker.FontFamily)] = MapFont,
			[nameof(ITimePicker.FontSize)] = MapFont,
			[nameof(ITimePicker.CharacterSpacing)] = MapCharacterSpacing
		};
				
		public TimePickerHandler() : base(TimePickerMapper)
		{

		}

		public TimePickerHandler(PropertyMapper mapper) : base(mapper)
		{

		}
	}
}
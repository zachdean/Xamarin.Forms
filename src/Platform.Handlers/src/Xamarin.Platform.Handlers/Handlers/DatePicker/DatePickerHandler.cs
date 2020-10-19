namespace Xamarin.Platform.Handlers
{
	public partial class DatePickerHandler
	{
		public static PropertyMapper<IDatePicker, DatePickerHandler> DatePickerMapper = new PropertyMapper<IDatePicker, DatePickerHandler>(ViewHandler.ViewMapper)
		{
			[nameof(IDatePicker.Format)] = MapFormat,
			[nameof(IDatePicker.Date)] = MapDate,
			[nameof(IDatePicker.MinimumDate)] = MapMinimumDate,
			[nameof(IDatePicker.MaximumDate)] = MapMaximumDate,
			[nameof(IDatePicker.Color)] = MapColor,
			[nameof(IDatePicker.Font)] = MapFont,
			[nameof(IDatePicker.FontAttributes)] = MapFont,
			[nameof(IDatePicker.FontFamily)] = MapFont,
			[nameof(IDatePicker.FontSize)] = MapFont,
			[nameof(IDatePicker.CharacterSpacing)] = MapCharacterSpacing
		};

		public DatePickerHandler() : base(DatePickerMapper)
		{

		}

		public DatePickerHandler(PropertyMapper mapper) : base(mapper)
		{

		}
	}
}
namespace System.Maui.Platform
{
	public partial class ButtonHandler 
	{
		public static PropertyMapper<IButton> ButtonMapper = new PropertyMapper<IButton>(ViewHandler.ViewMapper)
		{
			[nameof(IButton.Text)] = MapPropertyText,
			[nameof(IButton.TextColor)] = MapPropertyTextColor,
			[nameof(IButton.CornerRadius)] = MapPropertyCornerRadius,
			[nameof(IButton.BorderColor)] = MapPropertyBorderColor,
			[nameof(IButton.BorderWidth)] = MapPropertyBorderWidth,
			[nameof(IButton.Font)] = MapPropertyFont,
			[nameof(IButton.FontSize)] = MapPropertyFontSize,
			[nameof(IButton.FontAttributes)] = MapPropertyFontAttributes,
			[nameof(IButton.CharacterSpacing)] = MapPropertyCharacterSpacing
		};

		public ButtonHandler() : base(ButtonMapper)
		{

		}
		public ButtonHandler(PropertyMapper mapper) : base(mapper ?? ButtonMapper)
		{

		}
	}
}
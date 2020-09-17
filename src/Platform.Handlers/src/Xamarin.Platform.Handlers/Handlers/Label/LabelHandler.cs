namespace Xamarin.Platform.Handlers
{
	public partial class LabelHandler
	{
		public static PropertyMapper<ILabel> LabelMapper = new PropertyMapper<ILabel>(ViewHandler.ViewMapper)
		{
			[nameof(ILabel.Text)] = MapPropertyText,
			[nameof(ILabel.TextColor)] = MapPropertyTextColor,
			[nameof(ILabel.LineHeight)] = MapPropertyLineHeight,
			[nameof(ILabel.Font)] = MapPropertyFont,
			[nameof(ILabel.FontSize)] = MapPropertyFontSize,
			[nameof(ILabel.FontAttributes)] = MapPropertyFontAttributes,
			[nameof(ILabel.TextTransform)] = MapPropertyTextTransform,
			[nameof(ILabel.HorizontalTextAlignment)] = MapPropertyHorizontalTextAlignment,
			[nameof(ILabel.VerticalTextAlignment)] = MapPropertyVerticalTextAlignment,
			[nameof(ILabel.CharacterSpacing)] = MapPropertyCharacterSpacing,
			[nameof(ILabel.TextDecorations)] = MapPropertyTextDecorations,
			[nameof(ILabel.LineBreakMode)] = MapPropertyLineBreakMode,
			[nameof(ILabel.MaxLines)] = MapPropertyMaxLines,
			[nameof(IPadding.Padding)] = MapPropertyPadding
		};

		public LabelHandler() : base(LabelMapper)
		{

		}

		public LabelHandler(PropertyMapper mapper) : base(mapper ?? LabelMapper)
		{

		}
	}
}
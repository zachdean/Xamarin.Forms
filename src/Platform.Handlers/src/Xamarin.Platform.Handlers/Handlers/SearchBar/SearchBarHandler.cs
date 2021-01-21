namespace Xamarin.Platform.Handlers
{
	public partial class SearchBarHandler
	{
		public static PropertyMapper<ISearch, SearchBarHandler> SearchBarMapper = new PropertyMapper<ISearch, SearchBarHandler>(ViewHandler.ViewMapper)
		{
			[nameof(ISearch.SearchCommand)] = MapSearchCommand,
			[nameof(ISearch.SearchCommandParameter)] = MapSearchCommandParameter,
			[nameof(ISearch.CancelButtonColor)] = MapCancelButtonColor,
			[nameof(IText.Text)] = MapText,
			[nameof(IText.Color)] = MapTextColor,
			[nameof(IText.TextTransform)] = MapTextTransform,
			[nameof(IText.CharacterSpacing)] = MapCharacterSpacing,
			[nameof(IPlaceholder.Placeholder)] = MapPlaceholder,
			[nameof(IPlaceholder.PlaceholderColor)] = MapPlaceholderColor,
			[nameof(IFont.FontAttributes)] = MapFontAttributes,
			[nameof(IFont.FontFamily)] = MapFontFamily,
			[nameof(IFont.FontSize)] = MapFontSize,
			[nameof(ITextAlignment.HorizontalTextAlignment)] = MapHorizontalTextAlignment,
			[nameof(ITextAlignment.VerticalTextAlignment)] = MapVerticalTextAlignment
		};

		public SearchBarHandler() : base(SearchBarMapper)
		{

		}

		public SearchBarHandler(PropertyMapper mapper) : base(mapper ?? SearchBarMapper)
		{

		}
	}
}
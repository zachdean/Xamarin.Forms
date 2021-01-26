using Xamarin.Platform;

namespace Xamarin.Forms
{
	public partial class SearchBar : ISearch
	{
		Color IText.Color => TextColor;

		public string UpdateTransformedText(string source, TextTransform textTransform)
		{
			return UpdateFormsText(source, TextTransform);
		}

		void ISearch.SearchButtonPressed()
		{
			(this as ISearchBarController).OnSearchButtonPressed();
		}
	}
}
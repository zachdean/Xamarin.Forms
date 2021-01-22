using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Platform;

namespace Sample
{
	public class SearchBar : Xamarin.Forms.View, ISearch
	{
		public ICommand SearchCommand { get; set; }

		public object SearchCommandParameter { get; set; }

		public Color CancelButtonColor { get; set; }

		public string Text { get; set; }

		public Color Color { get; set; }

		public Font Font { get; set; }

		public TextTransform TextTransform { get; set; }

		public double CharacterSpacing { get; set; }

		public string Placeholder { get; set; }

		public Color PlaceholderColor { get; set; }

		public FontAttributes FontAttributes { get; set; }

		public string FontFamily { get; set; }

		public double FontSize { get; set; }

		public TextAlignment HorizontalTextAlignment { get; set; }

		public TextAlignment VerticalTextAlignment { get; set; }

		public Keyboard Keyboard { get; set; }

		public bool IsSpellCheckEnabled { get; set; }

		public int MaxLength { get; set; }

		public bool IsReadOnly { get; set; }

		string IText.UpdateTransformedText(string source, TextTransform textTransform)
			=> TextTransformUtilites.GetTransformedText(source, textTransform);
	}
}
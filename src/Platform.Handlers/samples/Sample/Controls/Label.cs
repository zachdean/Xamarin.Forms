using Xamarin.Forms;
using Xamarin.Platform;

namespace Sample
{
	public class Label : View, ILabel
	{
		public double LineHeight { get; set; }

		public FormattedString FormattedText { get; set; }

		public string Text { get; set; }

		public TextType TextType { get; set; } = TextType.Text;

		public Color TextColor { get; set; }

		public int MaxLines { get; set; }

		public TextDecorations TextDecorations { get; set; }

		public LineBreakMode LineBreakMode { get; set; }

		public string FontFamily { get; set; }

		public Font Font { get; set; }

		public double FontSize { get; set; }

		public FontAttributes FontAttributes { get; set; }

		public TextTransform TextTransform { get; set; }

		public TextAlignment HorizontalTextAlignment { get; set; }

		public TextAlignment VerticalTextAlignment { get; set; }

		public double CharacterSpacing { get; set; }

		public Thickness Padding { get; set; }

		object ILabel.FormattedText { get; }
	}
}
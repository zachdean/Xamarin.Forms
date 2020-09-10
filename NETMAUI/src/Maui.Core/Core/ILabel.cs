namespace System.Maui
{
	public interface ILabel : IText
	{
		FormattedString FormattedText { get; }

		TextType TextType { get; }

		double LineHeight { get; }

		int MaxLines { get; }

		TextDecorations TextDecorations { get; }

		LineBreakMode LineBreakMode { get; }
	}
}
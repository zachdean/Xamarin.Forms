using Xamarin.Forms;

namespace Xamarin.Platform
{
	public interface ILabel : IText, IPadding
	{
		object FormattedText { get; }

		TextType TextType { get; }

		double LineHeight { get; }

		int MaxLines { get; }

		TextDecorations TextDecorations { get; }

		LineBreakMode LineBreakMode { get; }
	}
}
using Xamarin.Forms;

namespace Xamarin.Platform
{
	public interface ITextInput : IText, IPlaceholder
	{
		Keyboard Keyboard { get; }
		bool IsSpellCheckEnabled { get; }
		int MaxLength { get; }
		bool IsReadOnly { get; }
	}
}
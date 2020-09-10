namespace System.Maui
{
	public interface ITextInput : IText
	{
		Keyboard Keyboard { get; }
		bool IsSpellCheckEnabled { get; }
		int MaxLength { get; } 
		string Placeholder { get; }
		Color PlaceholderColor { get; }
		bool IsReadOnly { get; }
		new string Text { get; set; }
		string IText.Text => Text;
	}
}
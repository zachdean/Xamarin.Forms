using System.ComponentModel;

namespace System.Maui
{
	[Flags]
	[TypeConverter(typeof(Xaml.TextDecorationConverter))]
	public enum TextDecorations
	{
		None = 0,
		Underline = 1 << 0,
		Strikethrough = 1 << 1,
	}
}
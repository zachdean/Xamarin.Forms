using System.ComponentModel;

namespace System.Maui
{
	[Flags]
	[TypeConverter(typeof(Xaml.FontAttributesConverter))]
	public enum FontAttributes
	{
		None = 0,
		Bold = 1 << 0,
		Italic = 1 << 1
	}
}
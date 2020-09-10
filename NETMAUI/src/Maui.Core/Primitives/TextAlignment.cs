using System.ComponentModel;

namespace System.Maui
{
	[TypeConverter(typeof(Xaml.TextAlignmentConverter))]
	public enum TextAlignment
	{
		Start,
		Center,
		End
	}
}

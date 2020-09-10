using AppKit;

namespace System.Maui.Platform
{
	internal static class AlignmentExtensions
	{
		internal static NSTextAlignment ToNativeTextAlignment(this TextAlignment alignment)
		{
			switch (alignment)
			{
				case TextAlignment.Center:
					return NSTextAlignment.Center;
				case TextAlignment.End:
					return NSTextAlignment.Right;
				case TextAlignment.Start:
				default:
					return NSTextAlignment.Left;
			}
		}
	}
}
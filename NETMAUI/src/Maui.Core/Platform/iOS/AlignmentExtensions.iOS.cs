using UIKit;

namespace System.Maui.Platform
{
	internal static class AlignmentExtensions
	{
		internal static UITextAlignment ToNativeTextAlignment(this TextAlignment alignment)
		{
			switch (alignment)
			{
				case TextAlignment.Center:
					return UITextAlignment.Center;
				case TextAlignment.End:
					return UITextAlignment.Right;
				case TextAlignment.Start:
				default:
					return UITextAlignment.Left;
			}
		}

		internal static UIControlContentVerticalAlignment ToNativeTextVerticalAlignment(this TextAlignment alignment)
		{
			switch (alignment)
			{
				case TextAlignment.Center:
					return UIControlContentVerticalAlignment.Center;
				case TextAlignment.End:
					return UIControlContentVerticalAlignment.Bottom;
				case TextAlignment.Start:
					return UIControlContentVerticalAlignment.Top;
				default:
					return UIControlContentVerticalAlignment.Top;
			}
		}
	}
}
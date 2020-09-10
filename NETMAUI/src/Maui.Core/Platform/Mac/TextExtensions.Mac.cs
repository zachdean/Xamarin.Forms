using AppKit;
using Foundation;

namespace System.Maui.Platform
{
	public static class TextExtensions
	{
        internal static NSMutableAttributedString AddCharacterSpacing(this NSAttributedString attributedString, string text, double characterSpacing)
        {
            NSMutableAttributedString mutableAttributedString;

            if (attributedString == null || attributedString.Length == 0)
                mutableAttributedString = text == null ? new NSMutableAttributedString() : new NSMutableAttributedString(text);
            else
                mutableAttributedString = new NSMutableAttributedString(attributedString);

            AddKerningAdjustment(mutableAttributedString, text, characterSpacing);

            return mutableAttributedString;
        }

        internal static void AddKerningAdjustment(NSMutableAttributedString mutableAttributedString, string text, double characterSpacing)
        {
            if (!string.IsNullOrEmpty(text))
            {
                mutableAttributedString.AddAttribute
                (
                    NSStringAttributeKey.KerningAdjustment,
                    NSObject.FromObject(characterSpacing), new NSRange(0, text.Length - 1)
                );
            }
        }
    }
}
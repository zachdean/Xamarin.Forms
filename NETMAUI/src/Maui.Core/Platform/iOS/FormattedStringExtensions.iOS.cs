using Foundation;
using UIKit;

namespace System.Maui.Platform
{
	public static class FormattedStringExtensions
	{
		public static NSAttributedString ToAttributed(this Span span, Font defaultFont, Color defaultForegroundColor)
		{
			if (span == null)
				return null;

#pragma warning disable 0618 // Retaining legacy call to obsolete code
			var font = span.Font != Font.Default ? span.Font : defaultFont;
#pragma warning restore 0618
			var fgcolor = span.TextColor;

			if (fgcolor.IsDefault)
				fgcolor = defaultForegroundColor;

			if (fgcolor.IsDefault)
				fgcolor = ColorExtensions.LabelColor.ToColor();

			return new NSAttributedString(span.Text, font == Font.Default ? null : font.ToUIFont(), fgcolor.ToNative(),
				span.BackgroundColor.ToNative(), kerning: (float)span.CharacterSpacing);
		}

		public static NSAttributedString ToAttributed(this FormattedString formattedString, Font defaultFont,
			Color defaultForegroundColor)
		{
			if (formattedString == null)
				return null;
			var attributed = new NSMutableAttributedString();
			for (int i = 0; i < formattedString.Spans.Count; i++)
			{
				Span span = formattedString.Spans[i];
				if (span.Text == null)
					continue;

				attributed.Append(span.ToAttributed(defaultFont, defaultForegroundColor));
			}

			return attributed;
		}

		internal static NSAttributedString ToAttributed(this Span span, IView owner, Color defaultForegroundColor, TextAlignment textAlignment, double lineHeight = -1.0)
		{
			if (span == null)
				return null;

			var text = span.Text;
			if (text == null)
				return null;

			NSMutableParagraphStyle style = new NSMutableParagraphStyle();
			lineHeight = span.LineHeight >= 0 ? span.LineHeight : lineHeight;
			if (lineHeight >= 0)
			{
				style.LineHeightMultiple = new nfloat(lineHeight);
			}

			switch (textAlignment)
			{
				case TextAlignment.Start:
					style.Alignment = UITextAlignment.Left;
					break;
				case TextAlignment.Center:
					style.Alignment = UITextAlignment.Center;
					break;
				case TextAlignment.End:
					style.Alignment = UITextAlignment.Right;
					break;
				default:
					style.Alignment = UITextAlignment.Left;
					break;
			}

			UIFont targetFont;

			if (span.IsDefault())
				targetFont = owner.ToUIFont();
			else
				targetFont = span.ToUIFont();

			var fgcolor = span.TextColor;

			if (fgcolor.IsDefault)
				fgcolor = defaultForegroundColor;

			if (fgcolor.IsDefault)
				fgcolor = ColorExtensions.LabelColor.ToColor();

			UIColor spanFgColor;
			UIColor spanBgColor;
			spanFgColor = fgcolor.ToNative();
			spanBgColor = span.BackgroundColor.ToNative();

			bool hasUnderline = false;
			bool hasStrikethrough = false;

			if (span.TextDecorations != TextDecorations.None)
			{
				var textDecorations = span.TextDecorations;
				hasUnderline = (textDecorations & TextDecorations.Underline) != 0;
				hasStrikethrough = (textDecorations & TextDecorations.Strikethrough) != 0;
			}

			var attrString = new NSAttributedString(text, targetFont, spanFgColor, spanBgColor,
				underlineStyle: hasUnderline ? NSUnderlineStyle.Single : NSUnderlineStyle.None,
				strikethroughStyle: hasStrikethrough ? NSUnderlineStyle.Single : NSUnderlineStyle.None, paragraphStyle: style, kerning: (float)span.CharacterSpacing);

			return attrString;
		}

		internal static NSAttributedString ToAttributed(this FormattedString formattedString, IView owner,
			Color defaultForegroundColor, TextAlignment textAlignment = TextAlignment.Start, double lineHeight = -1.0)
		{
			if (formattedString == null)
				return null;

			var attributed = new NSMutableAttributedString();

			for (int i = 0; i < formattedString.Spans.Count; i++)
			{
				Span span = formattedString.Spans[i];

				var attributedString = span.ToAttributed(owner, defaultForegroundColor, textAlignment, lineHeight);

				if (attributedString == null)
					continue;

				attributed.Append(attributedString);
			}

			return attributed;
		}
	}
}
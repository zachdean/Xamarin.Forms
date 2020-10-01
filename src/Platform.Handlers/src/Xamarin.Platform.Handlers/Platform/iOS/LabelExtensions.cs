using System;
using Foundation;
using UIKit;
using Xamarin.Forms;
using RectangleF = CoreGraphics.CGRect;
using SizeF = CoreGraphics.CGSize;

namespace Xamarin.Platform
{
	public static class LabelExtensions
	{
		public static void UpdateText(this UILabel nativeLabel, ILabel label)
		{
			SetText(nativeLabel, label);
		}

		public static void UpdateTextColor(this UILabel nativeLabel, ILabel label)
		{
			var textColor = label.TextColor;

			if (textColor.IsDefault && label.TextType == TextType.Html)
			{
				// If no explicit text color has been specified and we're displaying HTML, 
				// let the HTML determine the colors
				return;
			}

			// Default value of color documented to be black in iOS docs
			nativeLabel.TextColor = textColor.ToNative(ColorExtensions.LabelColor);

		}

		public static void UpdateFont(this UILabel nativeLabel, ILabel label)
		{
			nativeLabel.Font = label.ToUIFont();
		}

		public static void UpdateCharacterSpacing(this UILabel nativeLabel, ILabel label)
		{
			if (label.TextType != TextType.Text)
				return;

			var textAttr = nativeLabel.AttributedText?.AddCharacterSpacing(label.Text, label.CharacterSpacing);

			if (textAttr != null)
				nativeLabel.AttributedText = textAttr;
		}

		public static void UpdateLineHeight(this UILabel nativeLabel, ILabel label)
		{
			SetText(nativeLabel, label);
		}

		public static void UpdateHorizontalTextAlignment(this UILabel nativeLabel, ILabel label)
		{
			nativeLabel.TextAlignment = label.HorizontalTextAlignment.ToNativeTextAlignment(EffectiveFlowDirection.Explicit);
		}

		public static void UpdateVerticalTextAlignment(this UILabel nativeLabel, ILabel label)
		{
			SizeF fitSize;
			nfloat labelHeight;

			switch (label.VerticalTextAlignment)
			{
				case TextAlignment.Start:
					fitSize = nativeLabel.SizeThatFits(label.Frame.Size.ToSizeF());
					labelHeight = (nfloat)Math.Min(nativeLabel.Bounds.Height, fitSize.Height);
					nativeLabel.Frame = new RectangleF(0, 0, (nfloat)label.Frame.Width, labelHeight);
					break;
				case TextAlignment.Center:
					nativeLabel.Frame = new RectangleF(0, 0, (nfloat)label.Frame.Width, (nfloat)label.Frame.Height);
					break;
				case TextAlignment.End:
					fitSize = nativeLabel.SizeThatFits(label.Frame.Size.ToSizeF());
					labelHeight = (nfloat)Math.Min(nativeLabel.Bounds.Height, fitSize.Height);
					nfloat yOffset = 0;
					yOffset = (nfloat)(label.Frame.Height - labelHeight);
					nativeLabel.Frame = new RectangleF(0, yOffset, (nfloat)label.Frame.Width, labelHeight);

					break;
			}
		}

		public static void UpdateTextDecorations(this UILabel nativeLabel, ILabel label)
		{
			if (label?.TextType != TextType.Text)
				return;

			if (!(nativeLabel.AttributedText?.Length > 0))
				return;

			var textDecorations = label.TextDecorations;

			var newAttributedText = new NSMutableAttributedString(nativeLabel.AttributedText);
			var strikeThroughStyleKey = UIStringAttributeKey.StrikethroughStyle;
			var underlineStyleKey = UIStringAttributeKey.UnderlineStyle;

			var range = new NSRange(0, newAttributedText.Length);

			if ((textDecorations & TextDecorations.Strikethrough) == 0)
				newAttributedText.RemoveAttribute(strikeThroughStyleKey, range);
			else
				newAttributedText.AddAttribute(strikeThroughStyleKey, NSNumber.FromInt32((int)NSUnderlineStyle.Single), range);

			if ((textDecorations & TextDecorations.Underline) == 0)
				newAttributedText.RemoveAttribute(underlineStyleKey, range);
			else
				newAttributedText.AddAttribute(underlineStyleKey, NSNumber.FromInt32((int)NSUnderlineStyle.Single), range);

			nativeLabel.AttributedText = newAttributedText;
		}

		public static void UpdateLineBreakMode(this UILabel nativeLabel, ILabel label)
		{
			switch (label.LineBreakMode)
			{
				case LineBreakMode.NoWrap:
					nativeLabel.LineBreakMode = UILineBreakMode.Clip;
					break;
				case LineBreakMode.WordWrap:
					nativeLabel.LineBreakMode = UILineBreakMode.WordWrap;
					break;
				case LineBreakMode.CharacterWrap:
					nativeLabel.LineBreakMode = UILineBreakMode.CharacterWrap;
					break;
				case LineBreakMode.HeadTruncation:
					nativeLabel.LineBreakMode = UILineBreakMode.HeadTruncation;
					break;
				case LineBreakMode.MiddleTruncation:
					nativeLabel.LineBreakMode = UILineBreakMode.MiddleTruncation;
					break;
				case LineBreakMode.TailTruncation:
					nativeLabel.LineBreakMode = UILineBreakMode.TailTruncation;
					break;
			}
		}

		public static void UpdateMaxLines(this UILabel nativeLabel, ILabel label)
		{
			if (label.MaxLines >= 0)
				nativeLabel.Lines = label.MaxLines;
			else
			{
				switch (label.LineBreakMode)
				{
					case LineBreakMode.WordWrap:
					case LineBreakMode.CharacterWrap:
						nativeLabel.Lines = 0;
						break;
					case LineBreakMode.NoWrap:
					case LineBreakMode.HeadTruncation:
					case LineBreakMode.MiddleTruncation:
					case LineBreakMode.TailTruncation:
						nativeLabel.Lines = 1;
						break;
				}
			}
		}

		public static void UpdatePadding(this UILabel nativeLabel, ILabel label)
		{
			if (!(nativeLabel is NativeLabel customNativeLabel))
			{
				Console.WriteLine("On iOS, a Label created with no padding will ignore padding changes");
				return;
			}

			customNativeLabel.TextInsets = new UIEdgeInsets(
				(float)label.Padding.Top,
				(float)label.Padding.Left,
				(float)label.Padding.Bottom,
				(float)label.Padding.Right);
		}

		internal static void SetText(this UILabel nativeLabel, ILabel label)
		{
			switch (label.TextType)
			{
				case TextType.Html:
					UpdateTextHtml(nativeLabel, label);
					break;
				default:
					UpdateTextPlainText(nativeLabel, label);
					break;
			}
		}

		internal static void UpdateTextHtml(this UILabel nativeLabel, ILabel label)
		{
			string text = label.Text ?? string.Empty;

			var attr = GetNSAttributedStringDocumentAttributes();

			NSError? nsError = null;

			nativeLabel.AttributedText = new NSAttributedString(text, attr, ref nsError);

			// Setting AttributedText will reset style-related properties, so we'll need to update them again
			UpdateTextColor(nativeLabel, label);
			UpdateFont(nativeLabel, label);
		}

		internal static void UpdateTextPlainText(this UILabel nativeLabel, ILabel label)
		{
			var text = label.UpdateTransformedText(label.Text, label.TextTransform);

			nativeLabel.Text = text;
		}

		static NSAttributedStringDocumentAttributes GetNSAttributedStringDocumentAttributes()
		{
			return new NSAttributedStringDocumentAttributes
			{
				DocumentType = NSDocumentType.HTML,
				StringEncoding = NSStringEncoding.UTF8
			};
		}
	}
}
using System.Maui.Platform;
using Foundation;

#if __MOBILE__
using UIKit;
using NativeLabel = UIKit.UILabel;
#else
using AppKit;
using NativeLabel = AppKit.NSTextField;
#endif

namespace System.Maui.Platform
{
	public partial class LabelHandler : AbstractViewHandler<ILabel, NativeLabel>
	{
		static Color? DefaultTextColor;

		SizeRequest _perfectSize;
		bool _perfectSizeValid;
		FormattedString _formatted;

		bool IsTextFormatted => _formatted != null;

		protected override NativeLabel CreateView()
		{
			var label = new NativeLabel();

#if !__MOBILE__
			label.Editable = false;
			label.Bezeled = false;
			label.DrawsBackground = false;
#endif

			if (DefaultTextColor == null)
				DefaultTextColor = label.TextColor.ToColor();

			return label;
		}

		protected override void SetupDefaults()
		{
			base.SetupDefaults();
		}

		public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			if (!_perfectSizeValid)
			{
				_perfectSize = base.GetDesiredSize(double.PositiveInfinity, double.PositiveInfinity);
				_perfectSize.Minimum = new Size(Math.Min(10, _perfectSize.Request.Width), _perfectSize.Request.Height);
				_perfectSizeValid = true;
			}

			var widthFits = widthConstraint >= _perfectSize.Request.Width;
			var heightFits = heightConstraint >= _perfectSize.Request.Height;

			if (widthFits && heightFits)
				return _perfectSize;

			var result = base.GetDesiredSize(widthConstraint, heightConstraint);
			var tinyWidth = Math.Min(10, result.Request.Width);
			result.Minimum = new Size(tinyWidth, result.Request.Height);

			if (widthFits || VirtualView.LineBreakMode == LineBreakMode.NoWrap)
				return result;

			bool containerIsNotInfinitelyWide = !double.IsInfinity(widthConstraint);

			if (containerIsNotInfinitelyWide)
			{
				bool textCouldHaveWrapped = VirtualView.LineBreakMode == LineBreakMode.WordWrap || VirtualView.LineBreakMode == LineBreakMode.CharacterWrap;
				bool textExceedsContainer = result.Request.Width > widthConstraint;

				if (textExceedsContainer || textCouldHaveWrapped)
				{
					var expandedWidth = Math.Max(tinyWidth, widthConstraint);
					result.Request = new Size(expandedWidth, result.Request.Height);
				}
			}

			return result;
		}

		public static void MapPropertyText(IViewHandler Handler, ILabel view)
		{
			if (!(Handler is LabelHandler labelHandler))
				return;

			labelHandler.UpdateText();
			labelHandler.UpdateTextDecorations();
			labelHandler.UpdateCharacterSpacing();
		}

		public static void MapPropertyTextColor(IViewHandler Handler, ILabel view)
		{
			(Handler as LabelHandler)?.UpdateTextColor();
		}

		public static void MapPropertyLineHeight(IViewHandler Handler, ILabel view)
		{
			(Handler as LabelHandler)?.UpdateText();
		}

		public static void MapPropertyFont(IViewHandler Handler, IText view)
		{
			(Handler as LabelHandler)?.UpdateFont();
		}

		public static void MapPropertyFontSize(IViewHandler Handler, IText view)
		{
			(Handler as LabelHandler)?.UpdateFont();
		}

		public static void MapPropertyFontAttributes(IViewHandler Handler, IText view)
		{
			(Handler as LabelHandler)?.UpdateFont();
		}

		public static void MapPropertyTextTransform(IViewHandler Handler, IText view)
		{
			(Handler as LabelHandler)?.UpdateText();
		}

		public static void MapPropertyHorizontalTextAlignment(IViewHandler Handler, IText view)
		{
			if (!(Handler.NativeView is NativeLabel nativeLabel))
				return;
#if __MOBILE__
			nativeLabel.TextAlignment = view.HorizontalTextAlignment.ToNativeTextAlignment();
#else
			nativeLabel.Alignment = view.HorizontalTextAlignment.ToNativeTextAlignment();
#endif
		}

		public static void MapPropertyVerticalTextAlignment(IViewHandler Handler, IText view)
		{
			// TODO:
		}

		public static void MapPropertyCharacterSpacing(IViewHandler Handler, IText view)
		{
			(Handler as LabelHandler)?.UpdateCharacterSpacing();
		}

		public static void MapPropertyTextDecorations(IViewHandler Handler, ILabel view)
		{
			(Handler as LabelHandler)?.UpdateTextDecorations();
		}

		public static void MapPropertyLineBreakMode(IViewHandler Handler, ILabel view)
		{
			if (!(Handler.NativeView is NativeLabel nativeLabel))
				return;

#if __MOBILE__
			switch (view.LineBreakMode)
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
#else
			switch (view.LineBreakMode)
			{
				case LineBreakMode.NoWrap:
					nativeLabel.LineBreakMode = NSLineBreakMode.Clipping;
					break;
				case LineBreakMode.WordWrap:
					nativeLabel.LineBreakMode = NSLineBreakMode.ByWordWrapping;
					break;
				case LineBreakMode.CharacterWrap:
					nativeLabel.LineBreakMode = NSLineBreakMode.CharWrapping;
					break;
				case LineBreakMode.HeadTruncation:
					nativeLabel.LineBreakMode = NSLineBreakMode.TruncatingHead;
					break;
				case LineBreakMode.MiddleTruncation:
					nativeLabel.LineBreakMode = NSLineBreakMode.TruncatingMiddle;
					break;
				case LineBreakMode.TailTruncation:
					nativeLabel.LineBreakMode = NSLineBreakMode.TruncatingTail;
					break;
			}
#endif
		}

		public static void MapPropertyMaxLines(IViewHandler Handler, ILabel view)
		{
			if (!(Handler.NativeView is NativeLabel nativeLabel))
				return;

			if (view.MaxLines >= 0)
			{
#if __MOBILE__
				nativeLabel.Lines = view.MaxLines;
#else
				nativeLabel.MaximumNumberOfLines = view.MaxLines;
#endif
			}
			else
			{
#if __MOBILE__
				switch (view.LineBreakMode)
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
#else
				switch (view.LineBreakMode)
				{
					case LineBreakMode.WordWrap:
					case LineBreakMode.CharacterWrap:
						nativeLabel.MaximumNumberOfLines = 0;
						break;
					case LineBreakMode.NoWrap:
					case LineBreakMode.HeadTruncation:
					case LineBreakMode.MiddleTruncation:
					case LineBreakMode.TailTruncation:
						nativeLabel.MaximumNumberOfLines = 1;
						break;
				}
#endif
			}
		}

		public virtual void UpdateText()
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			switch (VirtualView.TextType)
			{
				case TextType.Html:
					UpdateTextHtml();
					break;

				default:
					UpdateTextPlainText();
					break;
			}
		}

		public virtual void UpdateTextColor()
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			if (IsTextFormatted)
			{
				UpdateFormattedText();
				return;
			}

			var textColor = VirtualView.TextColor;

			if (textColor.IsDefault && VirtualView.TextType == TextType.Html)
			{
				// If no explicit text color has been specified and we're displaying HTML, 
				// let the HTML determine the colors
				return;
			}

			// Default value of color documented to be black in iOS docs
#if __MOBILE__
			TypedNativeView.TextColor = textColor.ToNative(ColorExtensions.LabelColor);
#else
			var alignment = VirtualView.HorizontalTextAlignment.ToNativeTextAlignment();
			var textWithColor = new NSAttributedString(VirtualView.Text ?? string.Empty, font: VirtualView.ToNSFont(), foregroundColor: textColor.ToNative(ColorExtensions.Black), paragraphStyle: new NSMutableParagraphStyle() { Alignment = alignment });
			textWithColor = textWithColor.AddCharacterSpacing(VirtualView.Text ?? string.Empty, VirtualView.CharacterSpacing);
			TypedNativeView.AttributedStringValue = textWithColor;
#endif
		}

		public virtual void UpdateTextDecorations()
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			if (VirtualView?.TextType != TextType.Text)
				return;

#if __MOBILE__
			if (!(TypedNativeView.AttributedText?.Length > 0))
				return;
#else
			if (!(TypedNativeView.AttributedStringValue?.Length > 0))
				return;
#endif

			var textDecorations = VirtualView.TextDecorations;
#if __MOBILE__
			var newAttributedText = new NSMutableAttributedString(TypedNativeView.AttributedText);
			var strikeThroughStyleKey = UIStringAttributeKey.StrikethroughStyle;
			var underlineStyleKey = UIStringAttributeKey.UnderlineStyle;

#else
			var newAttributedText = new NSMutableAttributedString(TypedNativeView.AttributedStringValue);
			var strikeThroughStyleKey = NSStringAttributeKey.StrikethroughStyle;
			var underlineStyleKey = NSStringAttributeKey.UnderlineStyle;
#endif
			var range = new NSRange(0, newAttributedText.Length);

			if ((textDecorations & TextDecorations.Strikethrough) == 0)
				newAttributedText.RemoveAttribute(strikeThroughStyleKey, range);
			else
				newAttributedText.AddAttribute(strikeThroughStyleKey, NSNumber.FromInt32((int)NSUnderlineStyle.Single), range);

			if ((textDecorations & TextDecorations.Underline) == 0)
				newAttributedText.RemoveAttribute(underlineStyleKey, range);
			else
				newAttributedText.AddAttribute(underlineStyleKey, NSNumber.FromInt32((int)NSUnderlineStyle.Single), range);

#if __MOBILE__
			TypedNativeView.AttributedText = newAttributedText;
#else
			TypedNativeView.AttributedStringValue = newAttributedText;
#endif
			_perfectSizeValid = false;
		}

		public virtual void UpdateCharacterSpacing()
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			if (VirtualView.TextType != TextType.Text)
				return;
#if __MOBILE__
			var textAttr = TypedNativeView.AttributedText.AddCharacterSpacing(VirtualView.Text, VirtualView.CharacterSpacing);

			if (textAttr != null)
				TypedNativeView.AttributedText = textAttr;
#else
			var textAttr = TypedNativeView.AttributedStringValue.AddCharacterSpacing(VirtualView.Text, VirtualView.CharacterSpacing);

			if (textAttr != null)
				TypedNativeView.AttributedStringValue = textAttr;
#endif

			_perfectSizeValid = false;
		}

		public virtual void UpdateFont()
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			if (IsTextFormatted)
			{
				UpdateFormattedText();
				return;
			}

			if (VirtualView.TextType == TextType.Html && FontIsDefault(VirtualView))
			{
				// If no explicit font properties have been specified and we're display HTML,
				// let the HTML determine the typeface
				return;
			}

#if __MOBILE__
			TypedNativeView.Font = VirtualView.ToUIFont();
#else
			TypedNativeView.Font = VirtualView.ToNSFont();
#endif
		}

		protected virtual NSAttributedStringDocumentAttributes GetNSAttributedStringDocumentAttributes()
		{
			return new NSAttributedStringDocumentAttributes
			{
				DocumentType = NSDocumentType.HTML,
				StringEncoding = NSStringEncoding.UTF8
			};
		}

		void UpdateTextPlainText()
		{
			var text = GetTextTransformText();

			_formatted = VirtualView.FormattedText;

			if (_formatted == null && VirtualView.LineHeight >= 0)
				_formatted = text;

			if (IsTextFormatted)
				UpdateFormattedText();
			else
			{
#if __MOBILE__
				TypedNativeView.Text = text;
#else
				TypedNativeView.StringValue = text ?? string.Empty;
#endif
			}
		}

		void UpdateFormattedText()
		{
#if __MOBILE__
			TypedNativeView.AttributedText = _formatted.ToAttributed(VirtualView, VirtualView.TextColor, VirtualView.HorizontalTextAlignment, VirtualView.LineHeight);
#else
			TypedNativeView.AttributedStringValue = _formatted.ToAttributed(VirtualView, VirtualView.TextColor, VirtualView.HorizontalTextAlignment, VirtualView.LineHeight);
#endif
			_perfectSizeValid = false;
		}

		void UpdateTextHtml()
		{
			string text = VirtualView.Text ?? string.Empty;

			var attr = GetNSAttributedStringDocumentAttributes();
#if __MOBILE__

			NSError nsError = null;

			TypedNativeView.AttributedText = new NSAttributedString(text, attr, ref nsError);
#else
			var htmlData = new NSMutableData();
			htmlData.SetData(text);

			TypedNativeView.AttributedStringValue = new NSAttributedString(htmlData, attr, out _);
#endif
			_perfectSizeValid = false;

			// Setting AttributedText will reset style-related properties, so we'll need to update them again
			UpdateTextColor();
			UpdateFont();
		}

		string GetTextTransformText()
		{
			if (TypedNativeView == null || VirtualView == null)
				return string.Empty;

			string text = VirtualView.TextTransform switch
			{
				TextTransform.Lowercase => VirtualView.Text.ToLowerInvariant(),
				TextTransform.Uppercase => VirtualView.Text.ToUpperInvariant(),
				_ => VirtualView.Text,
			};

			return text;
		}

		static bool FontIsDefault(ILabel label)
		{
			if (label.FontAttributes != FontAttributes.None)
			{
				return false;
			}

			if (!string.IsNullOrEmpty(label.FontFamily))
			{
				return false;
			}

			return true;
		}
	}
}
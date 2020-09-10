using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Text;
using Android.Widget;
using Android.Util;
using static Android.Views.View;

#if __ANDROID_29__
using AndroidX.Core.View;
#else
using Android.Support.V4.View;
#endif

namespace System.Maui.Platform
{
	public partial class LabelHandler : AbstractViewHandler<ILabel, TextView>
	{
		ColorStateList _labelTextColorDefault;
		Color _lastUpdateColor = Color.Default;
		float _lineSpacingExtraDefault = -1.0f;
		float _lineSpacingMultiplierDefault = -1.0f;

		int _lastConstraintHeight;
		int _lastConstraintWidth;
		SizeRequest? _lastSizeRequest;

		Typeface _lastTypeface;
		float _lastTextSize = -1f;

		SpannableString _spannableString;
		bool _wasFormatted;

		protected override TextView CreateView()
		{
			var text = new TextView(Context);
			_labelTextColorDefault = text.TextColors;
			return text;
		}

		protected override void DisposeView(TextView nativeView)
		{
			_spannableString?.Dispose();

			base.DisposeView(nativeView);
		}

		public static void MapPropertyText(IViewHandler Handler, ILabel view)
		{
			(Handler as LabelHandler)?.UpdateText();
		}

		public static void MapPropertyTextColor(IViewHandler Handler, ILabel view)
		{
			(Handler as LabelHandler)?.UpdateTextColor();
		}

		public static void MapPropertyLineHeight(IViewHandler Handler, ILabel view)
		{
			var nativeLabel = Handler.NativeView as TextView;
			var labelHandler = Handler as LabelHandler;

			if (labelHandler._lineSpacingExtraDefault < 0)
				labelHandler._lineSpacingExtraDefault = nativeLabel.LineSpacingExtra;

			if (labelHandler._lineSpacingMultiplierDefault < 0)
				labelHandler._lineSpacingMultiplierDefault = nativeLabel.LineSpacingMultiplier;

			if (view.LineHeight == -1)
				nativeLabel.SetLineSpacing(labelHandler._lineSpacingExtraDefault, labelHandler._lineSpacingMultiplierDefault);
			else if (nativeLabel.LineHeight >= 0)
				nativeLabel.SetLineSpacing(0, nativeLabel.LineHeight);

			labelHandler._lastSizeRequest = null;
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
			if (!(Handler.NativeView is TextView nativeLabel))
				return;

			nativeLabel.Text = view.TextTransform.GetTextTransformText(view.Text);
		}

		public static void MapPropertyHorizontalTextAlignment(IViewHandler Handler, IText view)
		{
			(Handler as LabelHandler)?.UpdateTextAlignment();
		}

		public static void MapPropertyVerticalTextAlignment(IViewHandler Handler, IText view)
		{
			(Handler as LabelHandler)?.UpdateTextAlignment();
		}

		public static void MapPropertyCharacterSpacing(IViewHandler Handler, IText view)
		{
			if (NativeVersion.IsAtLeast(21))
			{
				var nativeLabel = Handler.NativeView as TextView;
				nativeLabel.LetterSpacing = view.CharacterSpacing.ToEm();
			}
		}

		public static void MapPropertyTextDecorations(IViewHandler Handler, ILabel view)
		{
			if (!(Handler.NativeView is TextView nativeLabel))
				return;

			var textDecorations = view.TextDecorations;

			if ((textDecorations & TextDecorations.Strikethrough) == 0)
				nativeLabel.PaintFlags &= ~PaintFlags.StrikeThruText;
			else
				nativeLabel.PaintFlags |= PaintFlags.StrikeThruText;

			if ((textDecorations & TextDecorations.Underline) == 0)
				nativeLabel.PaintFlags &= ~PaintFlags.UnderlineText;
			else
				nativeLabel.PaintFlags |= PaintFlags.UnderlineText;
		}

		public static void MapPropertyLineBreakMode(IViewHandler Handler, ILabel view)
		{
			if (!(Handler.NativeView is TextView nativeLabel))
				return;

			nativeLabel.SetLineBreakMode(view);

			(Handler as LabelHandler)._lastSizeRequest = null;
		}

		public static void MapPropertyMaxLines(IViewHandler Handler, ILabel view)
		{
			if (!(Handler.NativeView is TextView nativeLabel))
				return;

			nativeLabel.SetMaxLines(view);
		}

		public override SizeRequest GetDesiredSize(double wConstraint, double hConstraint)
		{
			int widthConstraint = wConstraint == double.MaxValue ? int.MaxValue : (int)wConstraint;
			int heightConstraint = hConstraint == double.MaxValue ? int.MaxValue : (int)hConstraint;

			if (_lastSizeRequest.HasValue)
			{
				// If we are measuring the same thing, no need to waste the time
				bool canRecycleLast = widthConstraint == _lastConstraintWidth && heightConstraint == _lastConstraintHeight;

				if (!canRecycleLast)
				{
					// If the last time we measured the returned size was all around smaller than the passed constraint
					// and the constraint is bigger than the last size request, we can assume the newly measured size request
					// will not change either.
					int lastConstraintWidthSize = MeasureSpec.GetSize(_lastConstraintWidth);
					int lastConstraintHeightSize = MeasureSpec.GetSize(_lastConstraintHeight);

					int currentConstraintWidthSize = MeasureSpec.GetSize(widthConstraint);
					int currentConstraintHeightSize = MeasureSpec.GetSize(heightConstraint);

					bool lastWasSmallerThanConstraints = _lastSizeRequest.Value.Request.Width < lastConstraintWidthSize && _lastSizeRequest.Value.Request.Height < lastConstraintHeightSize;

					bool currentConstraintsBiggerThanLastRequest = currentConstraintWidthSize >= _lastSizeRequest.Value.Request.Width && currentConstraintHeightSize >= _lastSizeRequest.Value.Request.Height;

					canRecycleLast = lastWasSmallerThanConstraints && currentConstraintsBiggerThanLastRequest;
				}

				if (canRecycleLast)
					return _lastSizeRequest.Value;
			}

			// We need to clear the Hint or else it will interfere with the sizing of the Label
			var hint = TypedNativeView.Hint;
			bool setHint = TypedNativeView.LayoutParameters != null;

			if (!string.IsNullOrEmpty(hint) && setHint)
				TypedNativeView.Hint = string.Empty;

			TypedNativeView.Measure(widthConstraint, heightConstraint);
			var result = new SizeRequest(new Size(TypedNativeView.MeasuredWidth, TypedNativeView.MeasuredHeight), new Size());

			// Set Hint back after sizing
			if (setHint)
				TypedNativeView.Hint = hint;

			result.Minimum = new Size(Math.Min(Context.ToPixels(10), result.Request.Width), result.Request.Height);

			// If the measure of the view has changed then trigger a request for layout
			// If the measure hasn't changed then force a layout of the label
			var measureIsChanged = !_lastSizeRequest.HasValue ||
				_lastSizeRequest.HasValue && (_lastSizeRequest.Value.Request.Height != TypedNativeView.MeasuredHeight || _lastSizeRequest.Value.Request.Width != TypedNativeView.MeasuredWidth);

			if (measureIsChanged)
				TypedNativeView.RequestLayout();
			else
				TypedNativeView.ForceLayout();

			_lastConstraintWidth = widthConstraint;
			_lastConstraintHeight = heightConstraint;
			_lastSizeRequest = result;

			return result;
		}

		public virtual void UpdateText()
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			if (VirtualView.FormattedText != null)
			{
				var formattedText = VirtualView.FormattedText ?? VirtualView.Text;
#pragma warning disable 618 // We will need to update this when .Font goes away
				TypedNativeView.TextFormatted = _spannableString = formattedText.ToAttributed(VirtualView.Font, VirtualView.TextColor, TypedNativeView);
#pragma warning restore 618
				_wasFormatted = true;
			}
			else
			{
				if (_wasFormatted)
				{
					TypedNativeView.SetTextColor(_labelTextColorDefault);
					_lastUpdateColor = Color.Default;
				}

				switch (VirtualView.TextType)
				{

					case TextType.Html:
						if (NativeVersion.IsAtLeast(24))
							TypedNativeView.SetText(Html.FromHtml(VirtualView.Text ?? string.Empty, FromHtmlOptions.ModeCompact), TextView.BufferType.Spannable);
						else
#pragma warning disable CS0618 // Type or member is obsolete
							TypedNativeView.SetText(Html.FromHtml(VirtualView.Text ?? string.Empty), TextView.BufferType.Spannable);
#pragma warning restore CS0618 // Type or member is obsolete
						break;
					default:
						TypedNativeView.Text = VirtualView.TextTransform.GetTextTransformText(VirtualView.Text);
						break;
				}

				UpdateTextColor();
				UpdateFont();

				_wasFormatted = false;
			}

			_lastSizeRequest = null;
		}

		public virtual void UpdateTextColor()
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			Color textColor = VirtualView.TextColor;

			if (textColor == _lastUpdateColor)
				return;

			_lastUpdateColor = textColor;

			if (textColor.IsDefault)
				TypedNativeView.SetTextColor(_labelTextColorDefault);
			else
				TypedNativeView.SetTextColor(textColor.ToNative());
		}

		public virtual void UpdateFont()
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			double fontSize = VirtualView.FontSize;
			FontAttributes fontAttributes = VirtualView.FontAttributes;
			string fontFamily = VirtualView.FontFamily;

			Font font;

			if (fontFamily != null)
				font = Font.OfSize(fontFamily, fontSize).WithAttributes(fontAttributes);
			else
				font = Font.SystemFontOfSize(fontSize, fontAttributes);

			Typeface newTypeface = font.ToTypeface();

			if (newTypeface != _lastTypeface)
			{
				TypedNativeView.Typeface = newTypeface;
				_lastTypeface = newTypeface;
			}

			float newTextSize = font.ToScaledPixel();

			if (newTextSize != _lastTextSize)
			{
				TypedNativeView.SetTextSize(ComplexUnitType.Sp, newTextSize);
				_lastTextSize = newTextSize;
			}
		}

		public virtual void UpdateTextAlignment()
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			TypedNativeView.Gravity = VirtualView.HorizontalTextAlignment.ToHorizontalGravityFlags() | VirtualView.VerticalTextAlignment.ToVerticalGravityFlags();

			_lastSizeRequest = null;
		}
	}
}
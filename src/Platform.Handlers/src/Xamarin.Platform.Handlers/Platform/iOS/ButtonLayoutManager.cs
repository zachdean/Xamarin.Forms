using System;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;

namespace Xamarin.Platform
{
	// TODO: The entire layout system. iOS buttons were not designed for
	//       anything but image left, text right, single line layouts.
	public class ButtonLayoutManager : IDisposable
	{
		// This looks like it should be a const under iOS Classic,
		// but that doesn't work under iOS 
		// ReSharper disable once BuiltInTypeReferenceStyle
		// Under iOS Classic Resharper wants to suggest this use the built-in type ref
		// but under iOS that suggestion won't work
		readonly nfloat _minimumButtonHeight = 44; // Apple docs

		readonly UIButton _nativeView;
		readonly IButton? _virtualView;

		readonly bool _preserveInitialPadding;
		readonly bool _spacingAdjustsPadding;
		readonly bool _borderAdjustsPadding;
		readonly bool _collapseHorizontalPadding;

		UIEdgeInsets? _defaultImageInsets;
		UIEdgeInsets? _defaultTitleInsets;
		UIEdgeInsets? _defaultContentInsets;

		UIEdgeInsets _paddingAdjustments = new UIEdgeInsets();

		public ButtonLayoutManager(UIButton nativeView,
			IButton? virtualView,
			bool preserveInitialPadding = false,
			bool spacingAdjustsPadding = true,
			bool borderAdjustsPadding = false,
			bool collapseHorizontalPadding = false)
		{
			_nativeView = nativeView ?? throw new ArgumentNullException(nameof(nativeView));
			_virtualView = virtualView ?? throw new ArgumentNullException(nameof(virtualView));

			_preserveInitialPadding = preserveInitialPadding;
			_spacingAdjustsPadding = spacingAdjustsPadding;
			_borderAdjustsPadding = borderAdjustsPadding;
			_collapseHorizontalPadding = collapseHorizontalPadding;
		}

		public void Dispose()
		{
			if (_nativeView != null)
				_nativeView.Dispose();
		}

		public CGSize SizeThatFits(CGSize size, CGSize measured)
		{
			if (_nativeView == null || _virtualView == null)
				return measured;

			var control = _nativeView;

			if (control == null)
				return measured;

			if (measured.Height < _minimumButtonHeight)
				measured.Height = _minimumButtonHeight;

			var titleHeight = control.TitleLabel.Frame.Height;

			if (titleHeight > measured.Height)
				measured.Height = titleHeight;

			return measured;
		}

		public void Update()
		{
			UpdatePadding();
			UpdateText();
			UpdateEdgeInsets();
			UpdateLineBreakMode();
		}

		public void UpdateLineBreakMode()
		{
			if (_nativeView == null || _virtualView == null)
				return;

			switch (_virtualView.LineBreakMode)
			{
				case LineBreakMode.NoWrap:
					_nativeView.LineBreakMode = UILineBreakMode.Clip;
					break;
				case LineBreakMode.WordWrap:
					_nativeView.LineBreakMode = UILineBreakMode.WordWrap;
					break;
				case LineBreakMode.CharacterWrap:
					_nativeView.LineBreakMode = UILineBreakMode.CharacterWrap;
					break;
				case LineBreakMode.HeadTruncation:
					_nativeView.LineBreakMode = UILineBreakMode.HeadTruncation;
					break;
				case LineBreakMode.TailTruncation:
					_nativeView.LineBreakMode = UILineBreakMode.TailTruncation;
					break;
				case LineBreakMode.MiddleTruncation:
					_nativeView.LineBreakMode = UILineBreakMode.MiddleTruncation;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public void UpdateText()
		{
			if (_nativeView == null || _virtualView == null)
				return;

			var control = _nativeView;

			if (control == null)
				return;

			var transformedText = _virtualView.UpdateTransformedText(_virtualView.Text, _virtualView.TextTransform);

			control.SetTitle(transformedText, UIControlState.Normal);

			var normalTitle = control
				.GetAttributedTitle(UIControlState.Normal);

			if (_virtualView.CharacterSpacing == 0 && normalTitle == null)
			{
				control.SetTitle(transformedText, UIControlState.Normal);
				return;
			}

			if (control.Title(UIControlState.Normal) != null)
				control.SetTitle(null, UIControlState.Normal);

			string text = transformedText ?? string.Empty;
			var colorRange = new NSRange(0, text.Length);

			var normal =
				control
					.GetAttributedTitle(UIControlState.Normal)
					.AddCharacterSpacing(text, _virtualView.CharacterSpacing);

			var highlighted =
				control
					.GetAttributedTitle(UIControlState.Highlighted)
					.AddCharacterSpacing(text, _virtualView.CharacterSpacing);

			var disabled =
				control
					.GetAttributedTitle(UIControlState.Disabled)
					.AddCharacterSpacing(text, _virtualView.CharacterSpacing);

			normal?.AddAttribute(
				UIStringAttributeKey.ForegroundColor,
				control.TitleColor(UIControlState.Normal),
				colorRange);

			highlighted?.AddAttribute(
				UIStringAttributeKey.ForegroundColor,
				control.TitleColor(UIControlState.Highlighted),
				colorRange);

			disabled?.AddAttribute(
				UIStringAttributeKey.ForegroundColor,
				control.TitleColor(UIControlState.Disabled),
				colorRange);

			control.SetAttributedTitle(normal, UIControlState.Normal);
			control.SetAttributedTitle(highlighted, UIControlState.Highlighted);
			control.SetAttributedTitle(disabled, UIControlState.Disabled);

			UpdateEdgeInsets();
		}

		public void UpdatePadding()
		{
			if (_nativeView == null || _virtualView == null)
				return;

			var control = _nativeView;

			if (control == null)
				return;

			EnsureDefaultInsets();

			control.ContentEdgeInsets = GetPaddingInsets(_paddingAdjustments);
		}

		public void UpdateEdgeInsets()
		{
			if (_nativeView == null || _virtualView == null)
				return;

			var control = _nativeView;

			if (control == null)
				return;

			EnsureDefaultInsets();

			_paddingAdjustments = new UIEdgeInsets();

			var imageInsets = new UIEdgeInsets();
			var titleInsets = new UIEdgeInsets();

			// Adjust for the border
			if (_borderAdjustsPadding &&
				_virtualView is IBorder borderElement &&
				borderElement.BorderWidth != -1d)
			{
				var width = (nfloat)_virtualView.BorderWidth;
				_paddingAdjustments.Top += width;
				_paddingAdjustments.Bottom += width;
				_paddingAdjustments.Left += width;
				_paddingAdjustments.Right += width;
			}

			var layout = _virtualView.ContentLayout;

			var spacing = (nfloat)layout.Spacing;
			var halfSpacing = spacing / 2;

			var image = control.CurrentImage;
			if (image != null && !string.IsNullOrEmpty(control.CurrentTitle))
			{
				// TODO: Do not use the title label as it is not yet updated and
				//       if we move the image, then we technically have more
				//       space and will require a new layout pass.
				var title =
					control.CurrentAttributedTitle ??
					new NSAttributedString(control.CurrentTitle, new UIStringAttributes { Font = control.TitleLabel.Font });
				var titleRect = title.GetBoundingRect(
					control.Bounds.Size,
					NSStringDrawingOptions.UsesLineFragmentOrigin | NSStringDrawingOptions.UsesFontLeading,
					null);

				var titleWidth = titleRect.Width;
				var titleHeight = titleRect.Height;
				var imageWidth = image.Size.Width;
				var imageHeight = image.Size.Height;

				// Adjust the padding for the spacing
				if (layout.IsHorizontal())
				{
					var adjustment = _spacingAdjustsPadding ? halfSpacing * 2 : halfSpacing;
					_paddingAdjustments.Left += adjustment;
					_paddingAdjustments.Right += adjustment;
				}
				else
				{
					var adjustment = _spacingAdjustsPadding ? halfSpacing * 2 : halfSpacing;

					_paddingAdjustments.Top += adjustment;
					_paddingAdjustments.Bottom += adjustment;
				}

				// Move the images according to the layout
				if (layout.Position == ButtonContentLayout.ImagePosition.Left)
				{
					// Add a bit of spacing
					imageInsets.Left -= halfSpacing;
					imageInsets.Right += halfSpacing;
					titleInsets.Left += halfSpacing;
					titleInsets.Right -= halfSpacing;
				}
				else if (layout.Position == ButtonContentLayout.ImagePosition.Right)
				{
					// Swap the elements and add spacing
					imageInsets.Left += titleWidth + halfSpacing;
					imageInsets.Right -= titleWidth + halfSpacing;
					titleInsets.Left -= imageWidth + halfSpacing;
					titleInsets.Right += imageWidth + halfSpacing;
				}
				else
				{
					// We will move the image and title vertically
					var imageVertical = (titleHeight / 2) + halfSpacing;
					var titleVertical = (imageHeight / 2) + halfSpacing;

					// The width will be different now that the image is no longer next to the text
					nfloat horizontalAdjustment = 0;
					if (_collapseHorizontalPadding)
						horizontalAdjustment = (nfloat)(titleWidth + imageWidth - Math.Max(titleWidth, imageWidth)) / 2;
					_paddingAdjustments.Left -= horizontalAdjustment;
					_paddingAdjustments.Right -= horizontalAdjustment;

					// The height will also be different
					var verticalAdjustment = (nfloat)Math.Min(imageVertical, titleVertical);
					_paddingAdjustments.Top += verticalAdjustment;
					_paddingAdjustments.Bottom += verticalAdjustment;

					// If the image is at the bottom, swap the direction
					if (layout.Position == ButtonContentLayout.ImagePosition.Bottom)
					{
						imageVertical = -imageVertical;
						titleVertical = -titleVertical;
					}

					// Move the image and title vertically
					imageInsets.Top -= imageVertical;
					imageInsets.Bottom += imageVertical;
					titleInsets.Top += titleVertical;
					titleInsets.Bottom -= titleVertical;

					// Center the elements horizontally
					var imageHorizontal = titleWidth / 2;
					var titleHorizontal = imageWidth / 2;
					imageInsets.Left += imageHorizontal;
					imageInsets.Right -= imageHorizontal;
					titleInsets.Left -= titleHorizontal;
					titleInsets.Right += titleHorizontal;
				}
			}

			UpdatePadding();
			control.ImageEdgeInsets = imageInsets;
			control.TitleEdgeInsets = titleInsets;
		}

		UIEdgeInsets GetPaddingInsets(UIEdgeInsets adjustments = default)
		{
			var defaultPadding = _preserveInitialPadding && _defaultContentInsets.HasValue
				? _defaultContentInsets.Value
				: new UIEdgeInsets();

			if (_virtualView == null)
				return defaultPadding;

			return new UIEdgeInsets(
				(nfloat)_virtualView.Padding.Top + defaultPadding.Top + adjustments.Top,
				(nfloat)_virtualView.Padding.Left + defaultPadding.Left + adjustments.Left,
				(nfloat)_virtualView.Padding.Bottom + defaultPadding.Bottom + adjustments.Bottom,
				(nfloat)_virtualView.Padding.Right + defaultPadding.Right + adjustments.Right);
		}

		void EnsureDefaultInsets()
		{
			if (_nativeView == null || _virtualView == null)
				return;

			var control = _nativeView;

			if (control == null)
				return;

			if (_defaultImageInsets == null)
				_defaultImageInsets = control.ImageEdgeInsets;
			if (_defaultTitleInsets == null)
				_defaultTitleInsets = control.TitleEdgeInsets;
			if (_defaultContentInsets == null)
				_defaultContentInsets = control.ContentEdgeInsets;
		}
	}
}
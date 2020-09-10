using CoreGraphics;
using Foundation;
using UIKit;

namespace System.Maui.Platform
{
	public partial class EntryHandler : AbstractViewHandler<IEntry, UITextField>
	{
		static readonly int baseHeight = 30;

		Color _defaultPlaceholderColor;
		UIColor _defaultTextColor;
		CGSize _initialSize;
		IDisposable _selectedTextRangeObserver;
		bool _nativeSelectionIsUpdating;
		bool _cursorPositionChangePending;
		bool _selectionLengthChangePending;

		protected override UITextField CreateView()
		{
			var textField = new UITextField(CGRect.Empty)
			{
				BorderStyle = UITextBorderStyle.RoundedRect,
				ClipsToBounds = true
			};

			textField.EditingChanged += OnEditingChanged;
			textField.ShouldReturn = OnShouldReturn;

			textField.EditingDidBegin += OnEditingBegan;
			textField.EditingDidEnd += OnEditingEnded;

			textField.ShouldChangeCharacters += ShouldChangeCharacters;

			_initialSize = CGSize.Empty;

			// Placeholder default color is 70% gray
			// https://developer.apple.com/library/prerelease/ios/documentation/UIKit/Reference/UITextField_Class/index.html#//apple_ref/occ/instp/UITextField/placeholder
			_defaultPlaceholderColor = ColorExtensions.SeventyPercentGrey.ToColor();

			_selectedTextRangeObserver = textField.AddObserver("selectedTextRange", NSKeyValueObservingOptions.New, UpdateCursor);

			return textField;
		}

		protected override void DisposeView(UITextField nativeView)
		{
			_defaultTextColor = null;

			nativeView.EditingDidBegin -= OnEditingBegan;
			nativeView.EditingChanged -= OnEditingChanged;
			nativeView.EditingDidEnd -= OnEditingEnded;
			nativeView.ShouldChangeCharacters -= ShouldChangeCharacters;

			_selectedTextRangeObserver?.Dispose();

			base.DisposeView(nativeView);
		}

		public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			var baseResult = base.GetDesiredSize(widthConstraint, heightConstraint);

			if (NativeVersion.IsAtLeast(11))
				return baseResult;

			NSString testString = new NSString("Tj");
			var testSize = testString.GetSizeUsingAttributes(new UIStringAttributes { Font = TypedNativeView.Font });
			double height = baseHeight + testSize.Height - _initialSize.Height;
			height = Math.Round(height);

			return new SizeRequest(new Size(baseResult.Request.Width, height));
		}

		public static void MapPropertyText(IViewHandler Handler, IEntry view)
		{
			(Handler as EntryHandler)?.UpdateText();
		}

		public static void MapPropertyColor(IViewHandler Handler, IEntry entry)
		{
			(Handler as EntryHandler)?.UpdateTextColor();
		}

		public static void MapPropertyFont(IViewHandler Handler, IEntry entry)
		{
			(Handler as EntryHandler)?.UpdateFont();
		}

		public static void MapPropertyTextTransform(IViewHandler Handler, IEntry entry)
		{
			(Handler as EntryHandler)?.UpdateText();
		}

		public static void MapPropertyCharacterSpacing(IViewHandler Handler, IEntry entry)
		{
			(Handler as EntryHandler)?.UpdateCharacterSpacing();
		}

		public static void MapPropertyPlaceholder(IViewHandler Handler, IEntry entry)
		{
			(Handler as EntryHandler)?.UpdatePlaceholder();
		}

		public static void MapPropertyMaxLength(IViewHandler Handler, IEntry entry)
		{
			(Handler as EntryHandler)?.UpdateMaxLength();
		}

		public static void MapPropertyIsReadOnly(IViewHandler Handler, IEntry entry)
		{
			(Handler as EntryHandler)?.UpdateIsReadOnly();
		}

		public static void MapPropertyKeyboard(IViewHandler Handler, IEntry entry)
		{
			(Handler as EntryHandler)?.UpdateKeyboard();
		}

		public static void MapPropertyIsSpellCheckEnabled(IViewHandler Handler, IEntry entry)
		{
			(Handler as EntryHandler)?.UpdateKeyboard();
		}

		public static void MapPropertyPlaceholderColor(IViewHandler Handler, IEntry entry)
		{
			(Handler as EntryHandler)?.UpdatePlaceholder();
		}
				
		public static void MapPropertyHorizontalTextAlignment(IViewHandler Handler, IEntry entry)
		{
			(Handler as EntryHandler)?.UpdateHorizontalTextAlignment();
		}

		public static void MapPropertyVerticalTextAlignment(IViewHandler Handler, IEntry entry)
		{
			(Handler as EntryHandler)?.UpdateVerticalTextAlignment();
		}

		public static void MapPropertyFontSize(IViewHandler Handler, IEntry entry)
		{
			(Handler as EntryHandler)?.UpdateFont();
		}

		public static void MapPropertyFontAttributes(IViewHandler Handler, IEntry entry)
		{
			(Handler as EntryHandler)?.UpdateFont();
		}

		public static void MapPropertyIsPassword(IViewHandler Handler, IEntry entry)
		{
			(Handler as EntryHandler)?.UpdatePassword();
		}

		public static void MapPropertyReturnType(IViewHandler Handler, IEntry entry)
		{
			if (!(Handler.NativeView is UITextField textField))
				return;

			textField.ReturnKeyType = entry.ReturnType.ToNative();
		}

		public static void MapPropertyCursorPosition(IViewHandler Handler, IEntry entry)
		{
			(Handler as EntryHandler)?.UpdateCursorSelection();
		}

		public static void MapPropertySelectionLength(IViewHandler Handler, IEntry entry)
		{
			(Handler as EntryHandler)?.UpdateCursorSelection();
		}

		public static void MapPropertyIsTextPredictionEnabled(IViewHandler Handler, IEntry entry)
		{
			(Handler as EntryHandler)?.UpdateKeyboard();
		}

		public static void MapPropertyClearButtonVisibility(IViewHandler Handler, IEntry entry)
		{
			(Handler as EntryHandler)?.UpdateClearButtonVisibility();
		}

		public virtual void UpdateText()
		{
			var text = VirtualView.Text;

			if (text == null)
				return;

			if (TypedNativeView.Text != text)
				TypedNativeView.Text = text;
		}

		public virtual void UpdateTextColor()
		{
			var color = VirtualView.TextColor;
			TypedNativeView.TextColor = color.IsDefault ? _defaultTextColor : color.ToNative();
		}

		public virtual void UpdatePlaceholder()
		{
			var formatted = (FormattedString)VirtualView.Placeholder;

			if (formatted == null)
				return;

			var targetColor = VirtualView.PlaceholderColor;
			var color = targetColor.IsDefault ? _defaultPlaceholderColor : targetColor;
			UpdateAttributedPlaceholder(formatted.ToAttributed(VirtualView, color));

			UpdateAttributedPlaceholder(TypedNativeView.AttributedPlaceholder.AddCharacterSpacing(VirtualView.Placeholder, VirtualView.CharacterSpacing));
		}

		public virtual void UpdateFont()
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			if (_initialSize == CGSize.Empty)
			{
				NSString testString = new NSString("Tj");
				_initialSize = testString.StringSize(TypedNativeView.Font);
			}

			TypedNativeView.Font = VirtualView.ToUIFont();
		}

		public virtual void UpdateHorizontalTextAlignment()
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			TypedNativeView.TextAlignment = VirtualView.HorizontalTextAlignment.ToNativeTextAlignment();
		}

		public virtual void UpdateVerticalTextAlignment()
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			TypedNativeView.VerticalAlignment = VirtualView.VerticalTextAlignment.ToNativeTextVerticalAlignment();
		}

		public virtual void UpdateCharacterSpacing()
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			var textAttr = TypedNativeView.AttributedText.AddCharacterSpacing(VirtualView.Text, VirtualView.CharacterSpacing);

			if (textAttr != null)
				TypedNativeView.AttributedText = textAttr;

			var placeHolder = TypedNativeView.AttributedPlaceholder.AddCharacterSpacing(VirtualView.Placeholder, VirtualView.CharacterSpacing);

			if (placeHolder != null)
				UpdateAttributedPlaceholder(placeHolder);
		}

		public virtual void UpdateMaxLength()
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			var currentControlText = TypedNativeView.Text;

			if (currentControlText.Length > VirtualView.MaxLength)
				TypedNativeView.Text = currentControlText.Substring(0, VirtualView.MaxLength);
		}

		public virtual void UpdateIsReadOnly()
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			TypedNativeView.UserInteractionEnabled = !VirtualView.IsReadOnly;
		}

		public virtual void UpdateKeyboard()
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			var keyboard = VirtualView.Keyboard;
			TypedNativeView.ApplyKeyboard(keyboard);

			if (!(keyboard is CustomKeyboard))
			{
				if (!VirtualView.IsSpellCheckEnabled)
				{
					TypedNativeView.SpellCheckingType = UITextSpellCheckingType.No;
				}

				if (!VirtualView.IsTextPredictionEnabled)
				{
					TypedNativeView.AutocorrectionType = UITextAutocorrectionType.No;
				}
			}

			TypedNativeView.ReloadInputViews();
		}

		public virtual void UpdatePassword()
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			if (VirtualView.IsPassword && TypedNativeView.IsFirstResponder)
			{
				TypedNativeView.Enabled = false;
				TypedNativeView.SecureTextEntry = true;
				TypedNativeView.Enabled = VirtualView.IsEnabled;
				TypedNativeView.BecomeFirstResponder();
			}
			else
				TypedNativeView.SecureTextEntry = VirtualView.IsPassword;
		}

		public virtual void UpdateCursorSelection()
		{
			if (_nativeSelectionIsUpdating || TypedNativeView == null || VirtualView == null)
				return;

			_cursorPositionChangePending = _selectionLengthChangePending = true;

			// If this is run from the ctor, the control is likely too early in its lifecycle to be first responder yet. 
			// Anything done here will have no effect, so we'll skip this work until later.
			// We'll try again when the control does become first responder later OnEditingBegan
			if (TypedNativeView.BecomeFirstResponder())
			{
				try
				{
					int cursorPosition = VirtualView.CursorPosition;

					UITextPosition start = GetSelectionStart(cursorPosition, out int startOffset);
					UITextPosition end = GetSelectionEnd(cursorPosition, start, startOffset);

					TypedNativeView.SelectedTextRange = TypedNativeView.GetTextRange(start, end);
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Failed to set Control.SelectedTextRange from CursorPosition/SelectionLength: {ex}");
				}
				finally
				{
					_cursorPositionChangePending = _selectionLengthChangePending = false;
				}
			}
		}

		public virtual void UpdateClearButtonVisibility()
		{
			TypedNativeView.ClearButtonMode = VirtualView.ClearButtonVisibility == ClearButtonVisibility.WhileEditing ? UITextFieldViewMode.WhileEditing : UITextFieldViewMode.Never;
		}

		protected virtual bool OnShouldReturn(UITextField view)
		{
			TypedNativeView.ResignFirstResponder();

			return false;
		}

		void UpdateAttributedPlaceholder(NSAttributedString nsAttributedString) =>
			TypedNativeView.AttributedPlaceholder = nsAttributedString;

		UITextPosition GetSelectionStart(int cursorPosition, out int startOffset)
		{
			UITextPosition start = TypedNativeView.EndOfDocument;
			startOffset = TypedNativeView.Text.Length;

			if (VirtualView.CursorPosition != 0)
			{
				start = TypedNativeView.GetPosition(TypedNativeView.BeginningOfDocument, cursorPosition) ?? TypedNativeView.EndOfDocument;
				startOffset = Math.Max(0, (int)TypedNativeView.GetOffsetFromPosition(TypedNativeView.BeginningOfDocument, start));
			}

			if (startOffset != cursorPosition)
				SetCursorPosition(startOffset);

			return start;
		}

		UITextPosition GetSelectionEnd(int cursorPosition, UITextPosition start, int startOffset)
		{
			UITextPosition end = start;
			int endOffset = startOffset;
			int selectionLength = VirtualView.SelectionLength;

			if (VirtualView.SelectionLength != 0)
			{
				end = TypedNativeView.GetPosition(start, Math.Max(startOffset, Math.Min(TypedNativeView.Text.Length - cursorPosition, selectionLength))) ?? start;
				endOffset = Math.Max(startOffset, (int)TypedNativeView.GetOffsetFromPosition(TypedNativeView.BeginningOfDocument, end));
			}

			int newSelectionLength = Math.Max(0, endOffset - startOffset);
			if (newSelectionLength != selectionLength)
				SetSelectionLength(newSelectionLength);

			return end;
		}

		void SetCursorPosition(int start)
		{
			try
			{
				_nativeSelectionIsUpdating = true;
				VirtualView.CursorPosition = start;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Failed to set CursorPosition from Handler: {ex}");
			}
			finally
			{
				_nativeSelectionIsUpdating = false;
			}
		}

		void SetSelectionLength(int selectionLength)
		{
			try
			{
				_nativeSelectionIsUpdating = true;
				VirtualView.SelectionLength = selectionLength;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Failed to set SelectionLength from Handler: {ex}");
			}
			finally
			{
				_nativeSelectionIsUpdating = false;
			}
		}

        void UpdateCursor(NSObservedChange obj)
		{
			if (_nativeSelectionIsUpdating || TypedNativeView == null || VirtualView == null)
				return;

			var currentSelection = TypedNativeView.SelectedTextRange;
			if (currentSelection != null)
			{
				if (!_cursorPositionChangePending)
				{
					int newCursorPosition = (int)TypedNativeView.GetOffsetFromPosition(TypedNativeView.BeginningOfDocument, currentSelection.Start);
					if (newCursorPosition != VirtualView.CursorPosition)
						SetCursorPosition(newCursorPosition);
				}

				if (!_selectionLengthChangePending)
				{
					int selectionLength = (int)TypedNativeView.GetOffsetFromPosition(currentSelection.Start, currentSelection.End);

					if (selectionLength != VirtualView.SelectionLength)
						SetSelectionLength(selectionLength);
				}
			}
		}

		void OnEditingBegan(object sender, EventArgs e)
		{
			if (!_cursorPositionChangePending && !_selectionLengthChangePending)
				UpdateCursor(null);
			else
				UpdateCursorSelection();

			VirtualView.IsFocused = true;
		}

		void OnEditingChanged(object sender, EventArgs eventArgs)
		{
			VirtualView.Text = TypedNativeView.Text;
			UpdateCursor(null);
		}

		void OnEditingEnded(object sender, EventArgs e)
		{
			// Typing aid changes don't always raise EditingChanged event

			// Normalizing nulls to string.Empty allows us to ensure that a change from null to "" doesn't result in a change event.
			// While technically this is a difference it serves no functional good.
			var controlText = TypedNativeView.Text ?? string.Empty;
			var entryText = VirtualView.Text ?? string.Empty;

			if (controlText != entryText)
			{
				VirtualView.Text = controlText;
			}

			VirtualView.IsFocused = false;
		}

		bool ShouldChangeCharacters(UITextField textField, NSRange range, string replacementString)
		{
			var newLength = textField?.Text?.Length + replacementString.Length - range.Length;
			return newLength <= VirtualView?.MaxLength;
		}
	}
}

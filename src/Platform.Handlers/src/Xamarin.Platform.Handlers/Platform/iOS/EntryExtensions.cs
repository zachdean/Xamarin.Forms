using System;
using Foundation;
using UIKit;
using Xamarin.Forms;

namespace Xamarin.Platform
{
	public static class EntryExtensions
	{
		static bool NativeSelectionIsUpdating;

		static bool CursorPositionChangePending;
		static bool SelectionLengthChangePending;

		public static void UpdateText(this UITextField textField, IEntry entry)
		{
			SetText(textField, entry);
		}

		public static void UpdateColor(this UITextField textField, IEntry entry)
		{
			var textColor = entry.TextColor;

			textField.TextColor = textColor.ToNative();
		}

		public static void UpdateFont(this UITextField textField, IEntry entry)
		{
			textField.Font = entry.ToUIFont();
		}

		public static void UpdateTextTransform(this UITextField textField, IEntry entry)
		{
			SetText(textField, entry);
		}

		public static void UpdateCharacterSpacing(this UITextField textField, IEntry entry)
		{
			var textAttr = textField.AttributedText?.AddCharacterSpacing(entry.Text, entry.CharacterSpacing);

			if (textAttr != null)
				textField.AttributedText = textAttr;

			var placeHolder = textField.AttributedPlaceholder?.AddCharacterSpacing(entry.Placeholder, entry.CharacterSpacing);

			if (placeHolder != null)
				textField.UpdateAttributedPlaceholder(placeHolder);
		}

		public static void UpdatePlaceholder(this UITextField textField, IEntry entry)
		{

		}

		public static void UpdatePlaceholderColor(this UITextField textField, IEntry entry)
		{

		}

		public static void UpdateMaxLength(this UITextField textField, IEntry entry)
		{
			var currentControlText = textField.Text;

			if (currentControlText?.Length > entry.MaxLength)
				textField.Text = currentControlText.Substring(0, entry.MaxLength);
		}

		public static void UpdateIsReadOnly(this UITextField textField, IEntry entry)
		{
			textField.UserInteractionEnabled = !entry.IsReadOnly;
		}

		public static void UpdateKeyboard(this UITextField textField, IEntry entry)
		{
			SetKeyboard(textField, entry);
		}

		public static void UpdateIsSpellCheckEnabled(this UITextField textField, IEntry entry)
		{
			SetKeyboard(textField, entry);
		}

		public static void UpdateHorizontalTextAlignment(this UITextField textField, IEntry entry)
		{
			// TODO: Pass the EffectiveFlowDirection.
			textField.TextAlignment = entry.HorizontalTextAlignment.ToNativeTextAlignment(EffectiveFlowDirection.Explicit);
		}

		public static void UpdateVerticalTextAlignment(this UITextField textField, IEntry entry)
		{
			textField.VerticalAlignment = entry.VerticalTextAlignment.ToNativeTextAlignment();
		}

		public static void UpdateIsPassword(this UITextField textField, IEntry entry)
		{
			if (entry.IsPassword && textField.IsFirstResponder)
			{
				textField.Enabled = false;
				textField.SecureTextEntry = true;
				textField.Enabled = entry.IsEnabled;
				textField.BecomeFirstResponder();
			}
			else
				textField.SecureTextEntry = entry.IsPassword;
		}

		public static void UpdateReturnType(this UITextField textField, IEntry entry)
		{
			textField.ReturnKeyType = entry.ReturnType.ToUIReturnKeyType();
		}

		public static void UpdateCursorPosition(this UITextField textField, IEntry entry)
		{
			SetCursorSelection(textField, entry);
		}

		public static void UpdateSelectionLength(this UITextField textField, IEntry entry)
		{
			SetCursorSelection(textField, entry);
		}

		public static void UpdateIsTextPredictionEnabled(this UITextField textField, IEntry entry)
		{
			SetKeyboard(textField, entry);
		}

		public static void UpdateClearButtonVisibility(this UITextField textField, IEntry entry)
		{
			textField.ClearButtonMode = entry.ClearButtonVisibility == ClearButtonVisibility.WhileEditing ? UITextFieldViewMode.WhileEditing : UITextFieldViewMode.Never;
		}

		internal static void SetText(this UITextField textField, IEntry entry)
		{
			var text = entry.UpdateTransformedText(entry.Text, entry.TextTransform);

			if (textField.Text != text)
				textField.Text = text;
		}

		internal static void UpdateAttributedPlaceholder(this UITextField textField, NSAttributedString nsAttributedString) =>
			textField.AttributedPlaceholder = nsAttributedString;

		internal static void SetCursorSelection(this UITextField textField, IEntry entry)
		{
			if (NativeSelectionIsUpdating || textField == null || entry == null)
				return;

			CursorPositionChangePending = SelectionLengthChangePending = true;

			// If this is run from the ctor, the control is likely too early in its lifecycle to be first responder yet. 
			// Anything done here will have no effect, so we'll skip this work until later.
			// We'll try again when the control does become first responder later OnEditingBegan
			if (textField.BecomeFirstResponder())
			{
				try
				{
					int cursorPosition = entry.CursorPosition;

					UITextPosition start = GetSelectionStart(textField, entry, cursorPosition, out int startOffset);
					UITextPosition end = GetSelectionEnd(textField, entry, cursorPosition, start, startOffset);

					textField.SelectedTextRange = textField.GetTextRange(start, end);
				}
				catch (Exception ex)
				{
					Console.WriteLine("Entry", $"Failed to set Control.SelectedTextRange from CursorPosition/SelectionLength: {ex}");
				}
				finally
				{
					CursorPositionChangePending = SelectionLengthChangePending = false;
				}
			}
		}

		internal static void UpdateCursor(this UITextField textField, IEntry entry, NSObservedChange? obj)
		{
			var currentSelection = textField.SelectedTextRange;

			if (currentSelection != null)
			{
				if (!CursorPositionChangePending)
				{
					int newCursorPosition = (int)textField.GetOffsetFromPosition(textField.BeginningOfDocument, currentSelection.Start);

					if (newCursorPosition != entry.CursorPosition)
						SetCursorPosition(entry, newCursorPosition);
				}

				if (!SelectionLengthChangePending)
				{
					int selectionLength = (int)textField.GetOffsetFromPosition(currentSelection.Start, currentSelection.End);

					if (selectionLength != entry.SelectionLength)
						SetSelectionLength(entry, selectionLength);
				}
			}
		}

		internal static void UpdateCursorOnEditingBegan(this UITextField textField, IEntry entry)
		{
			if (!CursorPositionChangePending && !SelectionLengthChangePending)
				UpdateCursor(textField, entry, null);
			else
				SetCursorSelection(textField, entry);
		}

		internal static void SetKeyboard(this UITextField textField, IEntry entry)
		{
			var keyboard = entry.Keyboard;
			// TODO: KeyboardMananger pending to be ported.
			//textField.ApplyKeyboard(keyboard);

			if (!(keyboard is CustomKeyboard))
			{
				if (!entry.IsSpellCheckEnabled)
				{
					textField.SpellCheckingType = UITextSpellCheckingType.No;
				}

				if (!entry.IsTextPredictionEnabled)
				{
					textField.AutocorrectionType = UITextAutocorrectionType.No;
				}
			}

			textField.ReloadInputViews();
		}

		static UITextPosition GetSelectionEnd(UITextField textField, IEntry entry, int cursorPosition, UITextPosition start, int startOffset)
		{
			UITextPosition end = start;
			int endOffset = startOffset;
			int selectionLength = entry.SelectionLength;

			if (entry.SelectionLength != 0)
			{
				end = textField.GetPosition(start, Math.Max(startOffset, Math.Min(string.IsNullOrEmpty(textField.Text) ? 0 : textField.Text!.Length - cursorPosition, selectionLength))) ?? start;
				endOffset = Math.Max(startOffset, (int)textField.GetOffsetFromPosition(textField.BeginningOfDocument, end));
			}

			int newSelectionLength = Math.Max(0, endOffset - startOffset);
			if (newSelectionLength != selectionLength)
				SetSelectionLength(entry, newSelectionLength);

			return end;
		}

		static UITextPosition GetSelectionStart(UITextField textField, IEntry entry, int cursorPosition, out int startOffset)
		{
			UITextPosition start = textField.EndOfDocument;
			startOffset = string.IsNullOrEmpty(textField.Text) ? 0 : textField.Text!.Length;

			if (entry.CursorPosition != 0)
			{
				start = textField.GetPosition(textField.BeginningOfDocument, cursorPosition) ?? textField.EndOfDocument;
				startOffset = Math.Max(0, (int)textField.GetOffsetFromPosition(textField.BeginningOfDocument, start));
			}

			if (startOffset != cursorPosition)
				SetCursorPosition(entry, startOffset);

			return start;
		}

		static void SetSelectionLength(IEntry entry, int selectionLength)
		{
			try
			{
				NativeSelectionIsUpdating = true;
				entry.SelectionLength = selectionLength;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Entry", $"Failed to set SelectionLength from renderer: {ex}");
			}
			finally
			{
				NativeSelectionIsUpdating = false;
			}
		}

		static void SetCursorPosition(IEntry entry, int start)
		{
			try
			{
				NativeSelectionIsUpdating = true;
				entry.CursorPosition = start;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Entry", $"Failed to set CursorPosition from renderer: {ex}");
			}
			finally
			{
				NativeSelectionIsUpdating = false;
			}
		}
	}
}
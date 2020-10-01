using System;
using System.Collections.Generic;
using Android.Graphics.Drawables;
using Android.Text;
using Android.Text.Method;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.Core.Content;
using Xamarin.Forms;
using static Android.Views.View;

namespace Xamarin.Platform
{
	public static class EntryExtensions
	{
		//static bool CursorPositionChangePending;
		//static bool SelectionLengthChangePending;
		static bool NativeSelectionIsUpdating;
		static Drawable? ClearButton;

		public static void UpdateText(this EditText editText, IEntry entry)
		{
			SetText(editText, entry);
		}

		public static void UpdateColor(this EditText editText, IEntry entry)
		{

		}

		public static void UpdateFont(this EditText editText, IEntry entry)
		{
			editText.SetFont(entry);
		}

		public static void UpdateTextTransform(this EditText editText, IEntry entry)
		{
			SetText(editText, entry);
		}

		public static void UpdateCharacterSpacing(this EditText editText, IEntry entry)
		{
			if (NativeVersion.IsAtLeast(21))
			{
				editText.LetterSpacing = entry.CharacterSpacing.ToEm();
			}
		}

		public static void UpdatePlaceholder(this EditText editText, IEntry entry)
		{
			editText.Hint = entry.Placeholder;

			if (editText.IsFocused)
			{
				// TODO: Port KeyboardManager to Xamarin.Platform.
				//TypedNativeView.ShowKeyboard();
			}
		}

		public static void UpdatePlaceholderColor(this EditText editText, IEntry entry)
		{

		}

		public static void UpdateMaxLength(this EditText editText, IEntry entry)
		{
			var currentFilters = new List<IInputFilter>(editText?.GetFilters() ?? new IInputFilter[0]);

			for (var i = 0; i < currentFilters.Count; i++)
			{
				if (currentFilters[i] is InputFilterLengthFilter)
				{
					currentFilters.RemoveAt(i);
					break;
				}
			}

			currentFilters.Add(new InputFilterLengthFilter(entry.MaxLength));

			editText?.SetFilters(currentFilters.ToArray());

			var currentControlText = editText?.Text;

			if (editText != null && currentControlText != null && currentControlText.Length > entry.MaxLength)
				editText.Text = currentControlText.Substring(0, entry.MaxLength);
		}

		public static void UpdateIsReadOnly(this EditText editText, IEntry entry)
		{
			bool isReadOnly = !entry.IsReadOnly;
			editText.FocusableInTouchMode = isReadOnly;
			editText.Focusable = isReadOnly;
		}

		public static void UpdateKeyboard(this EditText editText, IEntry entry)
		{
			editText.SetInputType(entry);
		}

		public static void UpdateIsSpellCheckEnabled(this EditText editText, IEntry entry)
		{
			editText.SetInputType(entry);
		}

		public static void UpdateHorizontalTextAlignment(this EditText editText, IEntry entry)
		{
			editText.UpdateTextAlignment(entry.HorizontalTextAlignment, entry.VerticalTextAlignment);
		}

		public static void UpdateVerticalTextAlignment(this EditText editText, IEntry entry)
		{
			editText.UpdateTextAlignment(entry.HorizontalTextAlignment, entry.VerticalTextAlignment);
		}

		public static void UpdateIsPassword(this EditText editText, IEntry entry)
		{
			editText.SetInputType(entry);
		}

		public static void UpdateReturnType(this EditText editText, IEntry entry)
		{
			editText.ImeOptions = entry.ReturnType.ToAndroidImeAction();
		}

		public static void UpdateCursorPosition(this EditText editText, IEntry entry)
		{
			editText.SetCursorSelection(entry);
		}

		public static void UpdateSelectionLength(this EditText editText, IEntry entry)
		{
			editText.SetCursorSelection(entry);
		}

		public static void UpdateIsTextPredictionEnabled(this EditText editText, IEntry entry)
		{
			editText.SetInputType(entry);
		}

		public static void UpdateClearButtonVisibility(this EditText editText, IEntry entry)
		{
			editText.SetClearButtonVisibility(entry);
		}

		internal static void SetText(this EditText editText, IEntry entry)
		{
			var text = entry.UpdateTransformedText(entry.Text, entry.TextTransform);

			if (editText.Text == text)
				return;

			editText.Text = text;

			if (editText.IsFocused)
			{
				editText.SetSelection(text.Length);
				// TODO: Port KeyboardManager to Xamarin.Platform.
				//editText.ShowKeyboard();
			}
		}

		internal static void SetFont(this EditText editText, IEntry entry)
		{
			editText.Typeface = entry.ToTypeface();
			editText.SetTextSize(ComplexUnitType.Sp, (float)entry.FontSize);
		}

		internal static void SetInputType(this EditText editText, IEntry entry)
		{
			IEntry model = entry;
			var keyboard = model.Keyboard;

			editText.InputType = keyboard.ToInputType();

			if (!(keyboard is CustomKeyboard))
			{
				if ((editText.InputType & InputTypes.TextFlagNoSuggestions) != InputTypes.TextFlagNoSuggestions)
				{
					if (!model.IsSpellCheckEnabled)
						editText.InputType |= InputTypes.TextFlagNoSuggestions;
				}

				if ((editText.InputType & InputTypes.TextFlagNoSuggestions) != InputTypes.TextFlagNoSuggestions)
				{
					if (!model.IsTextPredictionEnabled)
						editText.InputType |= InputTypes.TextFlagNoSuggestions;
				}
			}

			if (keyboard == Keyboard.Numeric)
			{
				editText.KeyListener = GetDigitsKeyListener(editText.InputType);
			}

			if (model.IsPassword && ((editText.InputType & InputTypes.ClassText) == InputTypes.ClassText))
				editText.InputType |= InputTypes.TextVariationPassword;

			if (model.IsPassword && ((editText.InputType & InputTypes.ClassNumber) == InputTypes.ClassNumber))
				editText.InputType |= InputTypes.NumberVariationPassword;

			SetFont(editText, entry);
		}

		internal static void SetCursorSelection(this EditText editText, IEntry entry)
		{
			if (NativeSelectionIsUpdating || entry == null || editText == null)
				return;

			if (!entry.IsReadOnly && editText.RequestFocus())
			{
				try
				{
					int start = GetSelectionStart(editText, entry);
					int end = GetSelectionEnd(editText, entry, start);

					editText.SetSelection(start, end);
				}
				catch (Exception ex)
				{
					Console.WriteLine("Entry", $"Failed to set Control.Selection from CursorPosition/SelectionLength: {ex}");
				}
				/*
				finally
				{
					CursorPositionChangePending = SelectionLengthChangePending = false;
				}
				*/
			}
		}

		internal static void SetClearButtonVisibility(this EditText editText, IEntry entry)
		{
			bool isFocused = entry.IsFocused;

			if (isFocused)
			{
				bool showClearBtn = entry.ClearButtonVisibility == ClearButtonVisibility.WhileEditing;
				editText.UpdateClearButton(entry, showClearBtn);

				if (!showClearBtn && isFocused)
					editText.ListenForCloseBtnTouch(false);
			}
		}

		static int GetSelectionEnd(EditText editText, IEntry entry, int start)
		{
			int end = start;
			int selectionLength = entry.SelectionLength;

			if (entry.SelectionLength != 0)
				end = Math.Max(start, Math.Min(editText.Length(), start + selectionLength));

			int newSelectionLength = Math.Max(0, end - start);
			if (newSelectionLength != selectionLength)
				SetSelectionLength(entry, newSelectionLength);

			return end;
		}

		static int GetSelectionStart(EditText editText, IEntry entry)
		{
			int start = editText.Length();
			int cursorPosition = entry.CursorPosition;

			if (entry.CursorPosition != 0)
				start = Math.Min(string.IsNullOrEmpty(editText.Text) ? 0 : editText.Text!.Length, cursorPosition);

			if (start != cursorPosition)
				SetCursorPosition(entry, start);

			return start;
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

		static NumberKeyListener GetDigitsKeyListener(InputTypes inputTypes)
		{
			// Override this in a custom renderer to use a different NumberKeyListener
			// or to filter out input types you don't want to allow
			// (e.g., inputTypes &= ~InputTypes.NumberFlagSigned to disallow the sign)
			return LocalizedDigitsKeyListener.Create(inputTypes);
		}

		static void UpdateClearButton(this EditText editText, IEntry entry, bool showClearButton)
		{
			Drawable? drawable = showClearButton && (entry.Text?.Length > 0) ? GetCloseButtonDrawable(editText) : null;
			editText.SetCompoundDrawablesWithIntrinsicBounds(null, null, drawable, null);
			ClearButton = drawable;
		}

		static Drawable GetCloseButtonDrawable(EditText editText)
			=> ContextCompat.GetDrawable(editText.Context, Resource.Drawable.abc_ic_clear_material);

		static void ListenForCloseBtnTouch(this EditText editText, bool listen)
		{
			if (listen)
				editText.Touch += OnEditTextTouched;
			else
				editText.Touch -= OnEditTextTouched;
		}

		static void OnEditTextTouched(object sender, TouchEventArgs e)
		{
			e.Handled = false;

			var editText = (EditText)sender;
			var ev = e.Event;

			var rBounds = ClearButton?.Bounds;
			if (rBounds != null)
			{
				var x = ev?.GetX();
				var y = ev?.GetY();

				if (ev?.Action == MotionEventActions.Up
					&& x >= (editText.Right - rBounds.Width())
					&& x <= (editText.Right - editText.PaddingRight)
					&& y >= editText.PaddingTop
					&& y <= (editText.Height - editText.PaddingBottom))
				{
					editText.Text = null;
					e.Handled = true;
				}
			}
		}
	}
}
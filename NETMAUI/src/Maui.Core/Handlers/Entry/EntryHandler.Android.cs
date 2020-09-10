using System.Collections.Generic;
using System.Maui.Controls.Primitives;
using Android.Util;
using Android.Text;
using Android.Views.InputMethods;
using Android.Text.Method;
using Java.Lang;
using Android.Graphics.Drawables;
using Android.Widget;
using Android.Runtime;
using Android.Views;

#if __ANDROID_29__
using AndroidX.Core.View;
using AndroidX.AppCompat.Widget;
#else
using Android.Support.V4.View;
using Android.Support.V7.Widget;
#endif

namespace System.Maui.Platform
{
    public partial class EntryHandler : AbstractViewHandler<IEntry, NativeEntry>
	{
		ImeAction _currentInputImeFlag;
		TextColorSwitcher _textColorSwitcher;
		TextColorSwitcher _placeHolderColorSwitcher;
		bool _cursorPositionChangePending;
		bool _selectionLengthChangePending;
		bool _nativeSelectionIsUpdating;
		Drawable _clearBtn;

		protected override NativeEntry CreateView()
		{
			var editText = new NativeEntry(Context);

			_textColorSwitcher = _placeHolderColorSwitcher = new TextColorSwitcher(editText);

			editText.TextChanged += EditTextTextChanged;

			editText.AddTextChangedListener(new TextWatcher(this));
			editText.SetOnEditorActionListener(new EditorActionListener(this));

			return editText;
		}

		void EditTextTextChanged(object sender, TextChangedEventArgs args) =>
			VirtualView.Text =args.Text.ToString();

		protected override void DisposeView(NativeEntry nativeView)
		{
			_textColorSwitcher = null;
			_placeHolderColorSwitcher = null;

			nativeView.TextChanged -= EditTextTextChanged;

			nativeView.AddTextChangedListener(null);
			nativeView.SetOnEditorActionListener(null);

			_clearBtn = null;

			base.DisposeView(nativeView);
		}

		public static void MapPropertyText(IViewHandler Handler, IEntry entry)
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
			(Handler as EntryHandler)?.UpdateInputType();
		}

		public static void MapPropertyIsSpellCheckEnabled(IViewHandler Handler, IEntry entry)
		{
			(Handler as EntryHandler)?.UpdateInputType();
		}

		public static void MapPropertyPlaceholderColor(IViewHandler Handler, IEntry entry) 
		{
			(Handler as EntryHandler)?.UpdatePlaceholderColor();
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
			(Handler as EntryHandler)?.UpdateInputType();
		}

		public static void MapPropertyReturnType(IViewHandler Handler, IEntry entry)
		{
			(Handler as EntryHandler)?.UpdateReturnType();
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
			(Handler as EntryHandler)?.UpdateInputType();
		}

		public static void MapPropertyClearButtonVisibility(IViewHandler Handler, IEntry entry)
		{

		}

		public virtual void UpdateText()
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			var text = VirtualView.TextTransform.GetTextTransformText(VirtualView.Text);

			if (TypedNativeView.Text == text)
				return;

			TypedNativeView.Text = text;

			if (TypedNativeView.IsFocused)
			{
				TypedNativeView.SetSelection(text.Length);
				TypedNativeView.ShowKeyboard();
			}
		}

		public virtual void UpdateTextColor() 
		{
			_textColorSwitcher ??= new TextColorSwitcher(TypedNativeView);
			_textColorSwitcher.UpdateTextColor(VirtualView.TextColor);
		}

		public virtual void UpdatePlaceholder()
		{
			if (TypedNativeView.Hint == VirtualView.Placeholder)
				return;

			TypedNativeView.Hint = VirtualView.Placeholder;

			if (TypedNativeView.IsFocused)
			{
				TypedNativeView.ShowKeyboard();
			}
		}

		public virtual void UpdatePlaceholderColor()
		{
			_placeHolderColorSwitcher ??= new TextColorSwitcher(TypedNativeView);
			_placeHolderColorSwitcher.UpdateHintTextColor(VirtualView.PlaceholderColor);
		}

		public virtual void UpdateFont()
		{
			TypedNativeView.Typeface = VirtualView.ToTypeface();
			TypedNativeView.SetTextSize(ComplexUnitType.Sp, (float)VirtualView.FontSize);
		}

		public virtual void UpdateHorizontalTextAlignment()
		{
			TypedNativeView.UpdateTextAlignment(VirtualView.HorizontalTextAlignment, VirtualView.VerticalTextAlignment);
		}

		public virtual void UpdateVerticalTextAlignment()
		{
			TypedNativeView.UpdateTextAlignment(VirtualView.HorizontalTextAlignment, VirtualView.VerticalTextAlignment);
		}

		public virtual void UpdateMaxLength()
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			var currentFilters = new List<IInputFilter>(TypedNativeView?.GetFilters() ?? new IInputFilter[0]);

			for (var i = 0; i < currentFilters.Count; i++)
			{
				if (currentFilters[i] is InputFilterLengthFilter)
				{
					currentFilters.RemoveAt(i);
					break;
				}
			}

			currentFilters.Add(new InputFilterLengthFilter(VirtualView.MaxLength));

			TypedNativeView?.SetFilters(currentFilters.ToArray());

			var currentControlText = TypedNativeView?.Text;

			if (currentControlText.Length > VirtualView.MaxLength)
				TypedNativeView.Text = currentControlText.Substring(0, VirtualView.MaxLength);
		}

		public virtual void UpdateCharacterSpacing()
		{
			if (NativeVersion.IsAtLeast(21))
			{
				TypedNativeView.LetterSpacing = VirtualView.CharacterSpacing.ToEm();
			}
		}

		public virtual void UpdateReturnType()
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			TypedNativeView.ImeOptions = VirtualView.ReturnType.ToNative();
			_currentInputImeFlag = TypedNativeView.ImeOptions;
		}

		public virtual void UpdateIsReadOnly()
		{
			bool isReadOnly = !VirtualView.IsReadOnly;
			TypedNativeView.SetCursorVisible(isReadOnly);
		}

		public virtual void UpdateInputType()
		{
			IEntry model = VirtualView;
			var keyboard = model.Keyboard;

			TypedNativeView.InputType = keyboard.ToInputType();
			if (!(keyboard is CustomKeyboard))
			{
				if ((TypedNativeView.InputType & InputTypes.TextFlagNoSuggestions) != InputTypes.TextFlagNoSuggestions)
				{
					if (!model.IsSpellCheckEnabled)
						TypedNativeView.InputType = TypedNativeView.InputType | InputTypes.TextFlagNoSuggestions;
				}

				if ((TypedNativeView.InputType & InputTypes.TextFlagNoSuggestions) != InputTypes.TextFlagNoSuggestions)
				{
					if (!model.IsTextPredictionEnabled)
						TypedNativeView.InputType = TypedNativeView.InputType | InputTypes.TextFlagNoSuggestions;
				}
			}
			
			if (keyboard == Keyboard.Numeric)
			{
				TypedNativeView.KeyListener = GetDigitsKeyListener(TypedNativeView.InputType);
			}

			if (model.IsPassword && ((TypedNativeView.InputType & InputTypes.ClassText) == InputTypes.ClassText))
				TypedNativeView.InputType = TypedNativeView.InputType | InputTypes.TextVariationPassword;
			if (model.IsPassword && ((TypedNativeView.InputType & InputTypes.ClassNumber) == InputTypes.ClassNumber))
				TypedNativeView.InputType = TypedNativeView.InputType | InputTypes.NumberVariationPassword;

			UpdateFont();
		}

		public virtual void UpdateCursorSelection()
		{
			if (_nativeSelectionIsUpdating || TypedNativeView == null || VirtualView == null || TypedNativeView == null)
				return;

			_cursorPositionChangePending = VirtualView.CursorPosition > 0;
			_selectionLengthChangePending = VirtualView.SelectionLength > 0;

			if (!VirtualView.IsReadOnly && TypedNativeView.RequestFocus())
			{
				try
				{
					int start = GetSelectionStart();
					int end = GetSelectionEnd(start);

					TypedNativeView.SetSelection(start, end);
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Failed to set Control.Selection from CursorPosition/SelectionLength: {ex}");
				}
				finally
				{
					_cursorPositionChangePending = _selectionLengthChangePending = false;
				}
			}
		}

		NumberKeyListener GetDigitsKeyListener(InputTypes inputTypes)
		{
			// Override this in a custom Handler to use a different NumberKeyListener
			// or to filter out input types you don't want to allow
			// (e.g., inputTypes &= ~InputTypes.NumberFlagSigned to disallow the sign)
			return LocalizedDigitsKeyListener.Create(inputTypes);
		}

		int GetSelectionEnd(int start)
		{
			int end = start;
			int selectionLength = VirtualView.SelectionLength;

			if (VirtualView.SelectionLength != 0)
				end = Math.Max(start, Math.Min(TypedNativeView.Length(), start + selectionLength));

			int newSelectionLength = Math.Max(0, end - start);
			if (newSelectionLength != selectionLength)
				SetSelectionLength(newSelectionLength);

			return end;
		}

		int GetSelectionStart()
		{
			int start = TypedNativeView.Length();
			int cursorPosition = VirtualView.CursorPosition;

			if (VirtualView.CursorPosition != 0)
				start = Math.Min(TypedNativeView.Text.Length, cursorPosition);

			if (start != cursorPosition)
				SetCursorPosition(start);

			return start;
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

		internal void OnAfterTextChanged()
        {
			if (TypedNativeView.IsFocused)
				UpdateClearBtn(true);
		}

		internal void OnTextChanged(string text)
		{
			VirtualView.Text = VirtualView.TextTransform.GetTextTransformText(text);
		}

		internal bool OnEditorAction(TextView textview, ImeAction actionId, KeyEvent keyEvent)
		{
			// Fire Completed and dismiss keyboard for hardware / physical keyboards
			if (actionId == ImeAction.Done || actionId == _currentInputImeFlag || (actionId == ImeAction.ImeNull && keyEvent.KeyCode == Keycode.Enter && keyEvent.Action == KeyEventActions.Up))
			{
				TypedNativeView.ClearFocus();
				textview.HideKeyboard();
			}

			return true;
		}

		void UpdateClearBtn(bool showClearButton)
		{
			//Drawable btnDrawable = showClearButton && (VirtualView.Text?.Length > 0) ? GetCloseButtonDrawable() : null;
			Drawable btnDrawable = null;
			TypedNativeView.SetCompoundDrawablesWithIntrinsicBounds(null, null, btnDrawable, null);
			_clearBtn = btnDrawable;
		}
	}

	class TextWatcher : Java.Lang.Object, ITextWatcher
	{
		readonly EntryHandler _entryHandler;

		public TextWatcher(EntryHandler entryHandler)
		{
			_entryHandler = entryHandler;
		}

        public void AfterTextChanged(IEditable s)
        {
			_entryHandler.OnAfterTextChanged();
		}

        public void BeforeTextChanged(ICharSequence s, int start, int count, int after)
        {
            
        }

        public void OnTextChanged(ICharSequence s, int start, int before, int count)
        {
			_entryHandler.OnTextChanged(s?.ToString());
		}
    }

    class EditorActionListener : Java.Lang.Object, TextView.IOnEditorActionListener
    {
		readonly EntryHandler _entryHandler;

		public EditorActionListener(EntryHandler entryHandler)
		{
			_entryHandler = entryHandler;
		}

		public bool OnEditorAction(TextView v, [GeneratedEnum] ImeAction actionId, KeyEvent e)
        {
			return _entryHandler.OnEditorAction(v, actionId, e);
		}
    }
}

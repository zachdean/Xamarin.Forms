using System.Collections.Generic;
using System.Linq;
using Android.Text;
using Android.Text.Method;
using Android.Util;
using Android.Widget;
using Xamarin.Forms;

namespace Xamarin.Platform
{
	public static class SearchBarExtensions
	{
		public static void UpdateCancelButtonColor(this SearchView searchView, ISearch search)
		{
			int? searchViewCloseButtonId = searchView.Resources?.GetIdentifier("android:id/search_close_btn", null, null);

			if (searchViewCloseButtonId != null && searchViewCloseButtonId != 0)
			{
				var image = searchView.FindViewById<ImageView>((int)searchViewCloseButtonId);
				if (image != null && image.Drawable != null)
				{
					if (search.CancelButtonColor != Color.Default)
						image.Drawable.SetColorFilter(search.CancelButtonColor, FilterMode.SrcIn);
					else
						image.Drawable.ClearColorFilter();
				}
			}
		}

		public static void UpdateText(this SearchView searchView, ISearch search)
		{
			searchView.SetText(search);
		}

		public static void UpdateTextColor(this SearchView searchView, ISearch search)
		{

		}

		public static void UpdateTextTransform(this SearchView searchView, ISearch search)
		{
			searchView.SetText(search);
		}

		public static void UpdateCharacterSpacing(this SearchView searchView, ISearch search)
		{
			searchView.UpdateCharacterSpacing(null, search);
		}

		public static void UpdateCharacterSpacing(this SearchView searchView, EditText? editText, ISearch search)
		{
			if (!NativeVersion.IsAtLeast(21))
				return;

			editText ??= searchView.GetChildrenOfType<EditText>().FirstOrDefault();

			if (editText != null)
			{
				editText.LetterSpacing = search.CharacterSpacing.ToEm();
			}
		}

		public static void UpdatePlaceholder(this SearchView searchView, ISearch search)
		{
			searchView.SetQueryHint(search.Placeholder);
		}

		public static void UpdatePlaceholderColor(this SearchView searchView, ISearch search)
		{

		}

		public static void UpdateFontAttributes(this SearchView searchView, ISearch search)
		{
			searchView.UpdateFontAttributes(null, search);
		}

		public static void UpdateFontAttributes(this SearchView searchView, EditText? editText, ISearch search)
		{
			searchView.SetFont(editText, search);
		}

		public static void UpdateFontFamily(this SearchView searchView, ISearch search)
		{
			searchView.UpdateFontFamily(null, search);
		}

		public static void UpdateFontFamily(this SearchView searchView, EditText? editText, ISearch search)
		{
			searchView.SetFont(editText, search);
		}

		public static void UpdateFontSize(this SearchView searchView, ISearch search)
		{
			searchView.UpdateFontSize(null, search);
		}

		public static void UpdateFontSize(this SearchView searchView, EditText? editText, ISearch search)
		{
			searchView.SetFont(editText, search);
		}

		public static void UpdateMaxLength(this SearchView searchView, ISearch search)
		{
			searchView.UpdateMaxLength(null, search);
		}

		public static void UpdateMaxLength(this SearchView searchView, EditText? editText, ISearch search)
		{
			editText ??= searchView.GetChildrenOfType<EditText>().FirstOrDefault();

			var currentFilters = new List<IInputFilter>(editText?.GetFilters() ?? new IInputFilter[0]);

			for (var i = 0; i < currentFilters.Count; i++)
			{
				if (currentFilters[i] is InputFilterLengthFilter)
				{
					currentFilters.RemoveAt(i);
					break;
				}
			}

			currentFilters.Add(new InputFilterLengthFilter(search.MaxLength));

			editText?.SetFilters(currentFilters.ToArray());

			var currentControlText = searchView.Query;

			if (currentControlText?.Length > search.MaxLength)
				searchView.SetQuery(currentControlText.Substring(0, search.MaxLength), false);
		}

		public static void UpdateKeyboard(this SearchView searchView, ISearch search)
		{
			searchView.UpdateKeyboard(null, search);
		}

		public static void UpdateKeyboard(this SearchView searchView, EditText? editText, ISearch search)
		{
			searchView.SetInputType(editText, search);
		}

		public static void UpdateIsSpellCheckEnabled(this SearchView searchView, ISearch search)
		{
			searchView.UpdateIsSpellCheckEnabled(null, search);
		}

		public static void UpdateIsSpellCheckEnabled(this SearchView searchView, EditText? editText, ISearch search)
		{
			searchView.SetInputType(editText, search);
		}

		public static void UpdateHorizontalTextAlignment(this SearchView searchView, ISearch search)
		{
			searchView.UpdateHorizontalTextAlignment(null, search);
		}

		public static void UpdateHorizontalTextAlignment(this SearchView searchView, EditText? editText, ISearch search)
		{
			editText ??= searchView.GetChildrenOfType<EditText>().FirstOrDefault();

			if (editText == null)
				return;

			bool hasRtlSupport = searchView.Context != null && searchView.Context.HasRtlSupport();
			editText.UpdateHorizontalAlignment(search.HorizontalTextAlignment, hasRtlSupport, TextAlignment.Center.ToVerticalGravityFlags());
		}

		public static void UpdateVerticalTextAlignment(this SearchView searchView, ISearch search)
		{
			searchView.UpdateVerticalTextAlignment(null, search);
		}

		public static void UpdateVerticalTextAlignment(this SearchView searchView, EditText? editText, ISearch search)
		{
			editText ??= searchView.GetChildrenOfType<EditText>().FirstOrDefault();

			if (editText == null)
				return;

			editText.UpdateVerticalAlignment(search.VerticalTextAlignment, TextAlignment.Center.ToVerticalGravityFlags());
		}

		internal static void SetText(this SearchView searchView, ISearch search)
		{
			string? query = searchView.Query;
			var text = search.UpdateTransformedText(search.Text, search.TextTransform);

			if (query != text)
				searchView.SetQuery(text, false);
		}

		internal static void SetFont(this SearchView searchView, EditText? editText, ISearch search)
		{
			editText ??= searchView.GetChildrenOfType<EditText>().FirstOrDefault();

			if (editText == null)
				return;

			editText.Typeface = search.ToTypeface();
			editText.SetTextSize(ComplexUnitType.Sp, (float)search.FontSize);
		}

		internal static void SetInputType(this SearchView searchView, EditText? editText, ISearch search)
		{
			ISearch model = search;
			var keyboard = model.Keyboard;

			var inputType = keyboard.ToInputType();

			if (!(keyboard is CustomKeyboard))
			{
				if ((inputType & InputTypes.TextFlagNoSuggestions) != InputTypes.TextFlagNoSuggestions)
				{
					if (!model.IsSpellCheckEnabled)
						inputType |= InputTypes.TextFlagNoSuggestions;
				}
			}

			searchView.SetInputType(inputType);

			if (keyboard == Keyboard.Numeric)
			{
				editText ??= searchView.GetChildrenOfType<EditText>().FirstOrDefault();

				if (editText != null)
					editText.KeyListener = GetDigitsKeyListener(inputType);
			}
		}

		internal static NumberKeyListener GetDigitsKeyListener(InputTypes inputTypes)
		{
			// Override this in a custom renderer to use a different NumberKeyListener
			// or to filter out input types you don't want to allow
			// (e.g., inputTypes &= ~InputTypes.NumberFlagSigned to disallow the sign)
			return LocalizedDigitsKeyListener.Create(inputTypes);
		}
	}
}
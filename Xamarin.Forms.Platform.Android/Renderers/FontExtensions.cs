using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Android.Graphics;
using AApplication = Android.App.Application;
using Xamarin.Forms.Internals;
using System.Diagnostics;

namespace Xamarin.Forms.Platform.Android
{
	public static class FontExtensions
	{
		static readonly Dictionary<Tuple<string, FontAttributes>, Typeface> Typefaces = new Dictionary<Tuple<string, FontAttributes>, Typeface>();

		static Typeface s_defaultTypeface;

		public static float ToScaledPixel(this Font self)
		{
			if (self.IsDefault)
				return 14;

			if (self.UseNamedSize)
			{
				switch (self.NamedSize)
				{
					case NamedSize.Micro:
						return 10;

					case NamedSize.Small:
						return 12;

					case NamedSize.Default:
					case NamedSize.Medium:
						return 14;

					case NamedSize.Large:
						return 18;
				}
			}

			return (float)self.FontSize;
		}

		internal static Typeface ToTypeFace(this string fontfamily, FontAttributes attr = FontAttributes.None)
		{
			var result = fontfamily.TryGetFromAssets();
			if (result.success)
			{
				return result.typeface;
			}
			else
			{
				var style = ToTypefaceStyle(attr);
				return Typeface.Create(fontfamily, style);
			}

		}

		static (bool success, Typeface typeface) TryGetFromAssets(this string fontfamily)
		{
			var isAssetfont = IsAssetFontFamily(fontfamily);
			if (isAssetfont)
			{
				return LoadTypefaceFromAsset(fontfamily);
			}

			var extension = new[]
			{
				".ttf",
				".otf"
			};

			var folders = new[]
			{
				"",
				"Fonts/",
				"fonts/",
			};

			foreach(var ext in extension)
			{
				foreach(var folder in folders)
				{
					var formated = $"{folder}{fontfamily}{ext}#{fontfamily}";
					var result = LoadTypefaceFromAsset(formated);
					if (result.success)
						return result;
				}
			}

			return (false, null);
		}

		static (bool success, Typeface typeface) LoadTypefaceFromAsset(string fontfamily)
		{
			try
			{
				var result = Typeface.CreateFromAsset(AApplication.Context.Assets, FontNameToFontFile(fontfamily));
				return (true, result);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
				return (false, null);
			}
		}

		public static Typeface ToTypeface(this Font self)
		{
			if (self.IsDefault || (self.FontAttributes == FontAttributes.None && string.IsNullOrEmpty(self.FontFamily)))
				return s_defaultTypeface ?? (s_defaultTypeface = Typeface.Default);

			var key = new Tuple<string, FontAttributes>(self.FontFamily, self.FontAttributes);
			Typeface result;
			if (Typefaces.TryGetValue(key, out result))
				return result;

			if (self.FontFamily == null)
			{
				var style = ToTypefaceStyle(self.FontAttributes);
				result = Typeface.Create(Typeface.Default, style);
			}
			else
			{
				result = self.FontFamily.ToTypeFace(self.FontAttributes);
			}

			return (Typefaces[key] = result);
		}

		internal static bool IsDefault(this IFontElement self)
		{
			return self.FontFamily == null && self.FontSize == Device.GetNamedSize(NamedSize.Default, typeof(Label), true) && self.FontAttributes == FontAttributes.None;
		}

		static bool IsAssetFontFamily (string name)
		{
			return name.Contains(".ttf#") || name.Contains(".otf#");
		}

		internal static Typeface ToTypeface(this IFontElement self)
		{
			if (self.IsDefault())
				return s_defaultTypeface ?? (s_defaultTypeface = Typeface.Default);

			var key = new Tuple<string, FontAttributes>(self.FontFamily, self.FontAttributes);
			Typeface result;
			if (Typefaces.TryGetValue(key, out result))
				return result;

			if (self.FontFamily == null)
			{
				var style = ToTypefaceStyle(self.FontAttributes);
				result = Typeface.Create(Typeface.Default, style);
			}
			else if (IsAssetFontFamily(self.FontFamily))
			{
				result = Typeface.CreateFromAsset(AApplication.Context.Assets, FontNameToFontFile(self.FontFamily));
			}
			else
			{
				var style = ToTypefaceStyle(self.FontAttributes);
				result = Typeface.Create(self.FontFamily, style);
			}
			return (Typefaces[key] = result);
		}

		public static TypefaceStyle ToTypefaceStyle(FontAttributes attrs)
		{
			var style = TypefaceStyle.Normal;
			if ((attrs & (FontAttributes.Bold | FontAttributes.Italic)) == (FontAttributes.Bold | FontAttributes.Italic))
				style = TypefaceStyle.BoldItalic;
			else if ((attrs & FontAttributes.Bold) != 0)
				style = TypefaceStyle.Bold;
			else if ((attrs & FontAttributes.Italic) != 0)
				style = TypefaceStyle.Italic;
			return style;
		}

		static string FontNameToFontFile(string fontFamily)
		{
			int hashtagIndex = fontFamily.IndexOf('#');
			if (hashtagIndex >= 0)
				return fontFamily.Substring(0, hashtagIndex);

			throw new InvalidOperationException($"Can't parse the {nameof(fontFamily)} {fontFamily}");
		}
	}
}
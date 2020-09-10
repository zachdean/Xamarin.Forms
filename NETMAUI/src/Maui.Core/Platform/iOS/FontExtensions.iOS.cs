using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UIKit;
using NativeFont = UIKit.UIFont;

namespace System.Maui.Platform
{
	public static partial class FontExtensions
	{
		static readonly string DefaultFontName = NativeFont.SystemFontOfSize(12).Name;

		static readonly Dictionary<ToNativeFontFontKey, NativeFont> ToUiFont = new Dictionary<ToNativeFontFontKey, NativeFont>();

		public static NativeFont ToUIFont(this Font self) => ToNativeFont(self);

		internal static bool IsDefault(this Span self)
		{
			return self.FontFamily == null && self.FontAttributes == FontAttributes.None;
		}

		internal static NativeFont ToUIFont(this Span element) => ToNativeFont(element);

		internal static NativeFont ToUIFont(this IView element) => ToNativeFont(element);

		static NativeFont _ToNativeFont(string family, float size, FontAttributes attributes)
		{
			var bold = (attributes & FontAttributes.Bold) != 0;
			var italic = (attributes & FontAttributes.Italic) != 0;

			if (family != null && family != DefaultFontName)
			{
				try
				{
					NativeFont result = null;
					if (NativeFont.FamilyNames.Contains(family))
					{
						var descriptor = new UIFontDescriptor().CreateWithFamily(family);

						if (bold || italic)
						{
							var traits = (UIFontDescriptorSymbolicTraits)0;
							if (bold)
								traits |= UIFontDescriptorSymbolicTraits.Bold;
							if (italic)
								traits |= UIFontDescriptorSymbolicTraits.Italic;

							descriptor = descriptor.CreateWithTraits(traits);
							result = NativeFont.FromDescriptor(descriptor, size);
							if (result != null)
								return result;
						}
					}

					var cleansedFont = CleanseFontName(family);
					result = NativeFont.FromName(cleansedFont, size);

					if (family.StartsWith(".SFUI", System.StringComparison.InvariantCultureIgnoreCase))
					{
						var fontWeight = family.Split('-').LastOrDefault();

						if (!string.IsNullOrWhiteSpace(fontWeight) && System.Enum.TryParse<UIFontWeight>(fontWeight, true, out var uIFontWeight))
						{
							result = NativeFont.SystemFontOfSize(size, uIFontWeight);
							return result;
						}

						result = NativeFont.SystemFontOfSize(size, UIFontWeight.Regular);
						return result;
					}

					if (result == null)
						result = NativeFont.FromName(family, size);

					if (result != null)
						return result;
				}
				catch
				{
					Debug.WriteLine("Could not load font named: {0}", family);
				}
			}

			if (bold && italic)
			{
				var defaultFont = NativeFont.SystemFontOfSize(size);

				var descriptor = defaultFont.FontDescriptor.CreateWithTraits(UIFontDescriptorSymbolicTraits.Bold | UIFontDescriptorSymbolicTraits.Italic);
				return NativeFont.FromDescriptor(descriptor, 0);
			}

			if (italic)
				return NativeFont.ItalicSystemFontOfSize(size);

			if (bold)
				return NativeFont.BoldSystemFontOfSize(size);

			return NativeFont.SystemFontOfSize(size);
		}

		internal static string CleanseFontName(string fontName)
		{
			// TODO:
			return string.Empty;
		}

		static NativeFont ToNativeFont(this Span span)
		{
			var fontFamily = span.FontFamily;
			var fontSize = (float)span.FontSize;
			var fontAttributes = span.FontAttributes;
			return ToNativeFont(fontFamily, fontSize, fontAttributes, _ToNativeFont);
		}

		static NativeFont ToNativeFont(this IView element)
		{
			if (!(element is IFont font))
				return null;

			var fontFamily = font.FontFamily;
			var fontSize = (float)font.FontSize;
			var fontAttributes = font.FontAttributes;

			// TODO:
			if (fontSize == 0)
				fontSize = 14.0f;

			return ToNativeFont(fontFamily, fontSize, fontAttributes, _ToNativeFont);
		}

		static NativeFont ToNativeFont(this Font self)
		{
			var size = (float)self.FontSize;
			if (self.UseNamedSize)
			{
				switch (self.NamedSize)
				{
					case NamedSize.Micro:
						size = 12;
						break;
					case NamedSize.Small:
						size = 14;
						break;
					case NamedSize.Medium:
						size = 17; // As defined by iOS documentation
						break;
					case NamedSize.Large:
						size = 22;
						break;
					default:
						size = 17;
						break;
				}
			}

			var fontAttributes = self.FontAttributes;

			return ToNativeFont(self.FontFamily, size, fontAttributes, _ToNativeFont);
		}

		static NativeFont ToNativeFont(string family, float size, FontAttributes attributes, Func<string, float, FontAttributes, NativeFont> factory)
		{
			var key = new ToNativeFontFontKey(family, size, attributes);

			lock (ToUiFont)
			{
				if (ToUiFont.TryGetValue(key, out NativeFont value))
					return value;
			}

			var generatedValue = factory(family, size, attributes);

			lock (ToUiFont)
			{
				if (!ToUiFont.TryGetValue(key, out NativeFont value))
					ToUiFont.Add(key, value = generatedValue);
				return value;
			}
		}

		struct ToNativeFontFontKey
		{
			internal ToNativeFontFontKey(string family, float size, FontAttributes attributes)
			{
				_family = family;
				_size = size;
				_attributes = attributes;
			}
#pragma warning disable 0414 // these are not called explicitly, but they are used to establish uniqueness. allow it!
			string _family;
			float _size;
			FontAttributes _attributes;
#pragma warning restore 0414
		}
	}
}
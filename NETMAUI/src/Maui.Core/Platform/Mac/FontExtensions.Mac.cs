using System.Collections.Generic;
using System.Diagnostics;
using AppKit;
using NativeFont = AppKit.NSFont;

namespace System.Maui.Platform
{
	public static partial class FontExtensions
	{
		static readonly string DefaultFontName = NSFont.SystemFontOfSize(12).FontName;

		static readonly Dictionary<ToNativeFontFontKey, NativeFont> ToUiFont = new Dictionary<ToNativeFontFontKey, NativeFont>();

		internal static bool IsDefault(this Span self)
		{
			return self.FontFamily == null && self.FontAttributes == FontAttributes.None;
		}

		public static NativeFont ToNSFont(this Font self) => ToNativeFont(self);

		internal static NativeFont ToNSFont(this Span element) => ToNativeFont(element);

		internal static NativeFont ToNSFont(this IView element) => ToNativeFont(element);

		static NativeFont _ToNativeFont(string family, float size, FontAttributes attributes)
		{
			NativeFont defaultFont = NativeFont.SystemFontOfSize(size);
			NativeFont font = null;
			NSFontDescriptor descriptor = null;
			var bold = (attributes & FontAttributes.Bold) != 0;
			var italic = (attributes & FontAttributes.Italic) != 0;

			if (family != null && family != DefaultFontName)
			{
				try
				{
					descriptor = new NSFontDescriptor().FontDescriptorWithFamily(family);
					font = NativeFont.FromDescription(descriptor, size);

					if (font == null)
					{
						var cleansedFont = CleanseFontName(family);
						font = NativeFont.FromFontName(cleansedFont, size);
					}
				}
				catch
				{
					Debug.WriteLine("Could not load font named: {0}", family);
				}
			}

			// If we didn't found a Font or Descriptor for the FontFamily use the default one 
			if (font == null)
			{
				font = defaultFont;
				descriptor = defaultFont.FontDescriptor;
			}

			if (descriptor == null)
				descriptor = defaultFont.FontDescriptor;


			if (bold || italic)
			{
				var traits = (NSFontSymbolicTraits)0;
				if (bold)
					traits |= NSFontSymbolicTraits.BoldTrait;
				if (italic)
					traits |= NSFontSymbolicTraits.ItalicTrait;

				var fontDescriptorWithTraits = descriptor.FontDescriptorWithSymbolicTraits(traits);

				font = NativeFont.FromDescription(fontDescriptorWithTraits, size);
			}

			return font.ScreenFontWithRenderingMode(NSFontRenderingMode.AntialiasedIntegerAdvancements);
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
				NativeFont value;
				if (ToUiFont.TryGetValue(key, out value))
					return value;
			}

			var generatedValue = factory(family, size, attributes);

			lock (ToUiFont)
			{
				NativeFont value;
				if (!ToUiFont.TryGetValue(key, out value))
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
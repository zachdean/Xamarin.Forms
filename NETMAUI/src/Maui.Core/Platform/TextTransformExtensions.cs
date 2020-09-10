namespace System.Maui.Platform
{
    public static class TextTransformExtensions
    {
		internal static string GetTextTransformText(this TextTransform textTransform, string text)
		{
			string transformedText = textTransform switch
			{
				TextTransform.Lowercase => text.ToLowerInvariant(),
				TextTransform.Uppercase => text.ToUpperInvariant(),
				_ => text,
			};

			return transformedText;
		}
	}
}

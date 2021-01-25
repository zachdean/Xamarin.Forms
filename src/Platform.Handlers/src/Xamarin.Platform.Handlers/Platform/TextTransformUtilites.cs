using System.ComponentModel;
using Xamarin.Forms;

namespace Xamarin.Platform
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class TextTransformUtilites
	{
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static string GetTransformedText(string source, TextTransform textTransform)
		{
			if (string.IsNullOrEmpty(source))
				return string.Empty;

			switch (textTransform)
			{
				case TextTransform.None:
				default:
					return source;
				case TextTransform.Lowercase:
					return source.ToLowerInvariant();
				case TextTransform.Uppercase:
					return source.ToUpperInvariant();
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void SetPlainText(IText? inputView, string nativeText)
		{
			if (inputView == null || nativeText == null)
				return;

			var textTransform = inputView.TextTransform;
			var nativeTextWithTransform = inputView.UpdateTransformedText(nativeText, textTransform);
			var formsText = inputView.UpdateTransformedText(inputView.Text, textTransform);

			if ((string.IsNullOrEmpty(formsText) && nativeText.Length == 0))
				return;

			if (formsText == nativeTextWithTransform)
				return;

			inputView.Text = nativeText;
		}
	}
}
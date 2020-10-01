using System.ComponentModel;

namespace Xamarin.Forms.Internals
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class PlainTextUtilites
	{
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void SetPlainText(InputView inputView, string nativeText)
		{
			if (inputView == null)
				return;

			var textTransform = inputView.TextTransform;
			var nativeTextWithTransform = inputView.UpdateFormsText(nativeText, textTransform);
			var formsText = inputView.UpdateFormsText(inputView.Text, textTransform);

			if ((string.IsNullOrEmpty(formsText) && nativeText.Length == 0))
				return;

			if (formsText == nativeTextWithTransform)
				return;

			inputView.SetValueFromRenderer(InputView.TextProperty, nativeText);
		}
	}
}
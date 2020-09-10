using Foundation;
using UIKit;

namespace System.Maui.Platform
{
	// TODO: The entire layout system. iOS buttons were not designed for
	//       anything but image left, text right, single line layouts.
	public class ButtonLayoutManager : IDisposable
	{
		bool _disposed;
		IViewHandler _Handler;
		readonly IButton _element;
		readonly UIButton _control;

		public ButtonLayoutManager(IViewHandler Handler, IButton element)
		{
			_Handler = Handler ?? throw new ArgumentNullException(nameof(Handler));
			_element = element;
			_control = (UIButton)_Handler.NativeView;
		}

		public void Dispose()
		{
			if (!_disposed)
			{
				if (_Handler != null)
					_Handler = null;

				_disposed = true;
			}
		}

		public void UpdateText()
		{
			if (_disposed || _Handler == null || _control == null || _element == null)
				return;

			var transformedText = GetTextTransformText(_element.Text, _element.TextTransform);

			_control.SetTitle(transformedText, UIControlState.Normal);

			var normalTitle = _control
				.GetAttributedTitle(UIControlState.Normal);

			if (_element.CharacterSpacing == 0 && normalTitle == null)
			{
				_control.SetTitle(transformedText, UIControlState.Normal);
				return;
			}

			if (_control.Title(UIControlState.Normal) != null)
				_control.SetTitle(null, UIControlState.Normal);

			string text = transformedText ?? string.Empty;
			var colorRange = new NSRange(0, text.Length);

			var normal =
				_control
					.GetAttributedTitle(UIControlState.Normal)
					.AddCharacterSpacing(text, _element.CharacterSpacing);

			var highlighted =
				_control
					.GetAttributedTitle(UIControlState.Highlighted)
					.AddCharacterSpacing(text, _element.CharacterSpacing);

			var disabled =
				_control
					.GetAttributedTitle(UIControlState.Disabled)
					.AddCharacterSpacing(text, _element.CharacterSpacing);

			normal.AddAttribute(
				UIStringAttributeKey.ForegroundColor,
				_control.TitleColor(UIControlState.Normal),
				colorRange);

			highlighted.AddAttribute(
				UIStringAttributeKey.ForegroundColor,
				_control.TitleColor(UIControlState.Highlighted),
				colorRange);

			disabled.AddAttribute(
				UIStringAttributeKey.ForegroundColor,
				_control.TitleColor(UIControlState.Disabled),
				colorRange);

			_control.SetAttributedTitle(normal, UIControlState.Normal);
			_control.SetAttributedTitle(highlighted, UIControlState.Highlighted);
			_control.SetAttributedTitle(disabled, UIControlState.Disabled);
		}
		
		string GetTextTransformText(string text, TextTransform textTransform)
		{
			string textTransformText = textTransform switch
			{
				TextTransform.Lowercase => text.ToLowerInvariant(),
				TextTransform.Uppercase => text.ToUpperInvariant(),
				_ => text,
			};

			return textTransformText;
		}
	}
}
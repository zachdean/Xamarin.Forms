using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Platform;

namespace Sample.ReactiveControl
{
	public class ReactiveLabel : ReactiveView, ILabel
	{
		private string _text;
		private Color _textColor;

		public TextType TextType => throw new NotImplementedException();

		public double LineHeight => throw new NotImplementedException();

		public int MaxLines => throw new NotImplementedException();

		public TextDecorations TextDecorations => throw new NotImplementedException();

		public LineBreakMode LineBreakMode => throw new NotImplementedException();

		public string Text
		{
			get
			{
				return _text;
			}
			set
			{
				_text = value;
				OnPropertyChanged();
			}
		}

		public Color TextColor
		{
			get
			{
				return _textColor;
			}
			set
			{
				_textColor = value;
				OnPropertyChanged();
			}
		}

		public Font Font => throw new NotImplementedException();

		public TextTransform TextTransform => throw new NotImplementedException();

		public double CharacterSpacing => throw new NotImplementedException();

		public FontAttributes FontAttributes => throw new NotImplementedException();

		public string FontFamily => throw new NotImplementedException();

		public double FontSize => throw new NotImplementedException();

		public TextAlignment HorizontalTextAlignment => throw new NotImplementedException();

		public TextAlignment VerticalTextAlignment => throw new NotImplementedException();

		public Thickness Padding => throw new NotImplementedException();

		public string UpdateTransformedText(string source, TextTransform textTransform)
		{
			throw new NotImplementedException();
		}
	}
}

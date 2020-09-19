using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Platform;

namespace Sample.ReactiveControl
{
	public class ReactiveButton : ReactiveView, IButton
	{
		private string _text;
		private Color _textColor;

		public string Text
		{
			get => _text;
			set
			{
				_text = value;
				OnPropertyChanged();
			}
		}

		public Color TextColor
		{
			get => _textColor;
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

		public void Clicked()
		{
			throw new NotImplementedException();
		}

		public void Pressed()
		{
			throw new NotImplementedException();
		}

		public void Released()
		{
			throw new NotImplementedException();
		}

		public string UpdateTransformedText(string source, TextTransform textTransform)
		{
			throw new NotImplementedException();
		}
	}
}

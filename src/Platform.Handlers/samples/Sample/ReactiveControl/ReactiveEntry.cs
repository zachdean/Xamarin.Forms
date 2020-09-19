using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Platform;

namespace Sample.ReactiveControl
{
	public class ReactiveEntry : ReactiveView, IEntry
	{
		public ReactiveEntry()
		{
		}

		public bool IsPassword => throw new NotImplementedException();

		public ReturnType ReturnType => throw new NotImplementedException();

		public ICommand ReturnCommand => throw new NotImplementedException();

		public object ReturnCommandParameter => throw new NotImplementedException();

		public int CursorPosition { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public int SelectionLength { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public bool IsTextPredictionEnabled => throw new NotImplementedException();

		public ClearButtonVisibility ClearButtonVisibility => throw new NotImplementedException();

		public Keyboard Keyboard => throw new NotImplementedException();

		public bool IsSpellCheckEnabled => throw new NotImplementedException();

		public int MaxLength => throw new NotImplementedException();

		public string Placeholder => throw new NotImplementedException();

		public Color PlaceholderColor => throw new NotImplementedException();

		public bool IsReadOnly => throw new NotImplementedException();

		public string Text => throw new NotImplementedException();

		public Color TextColor => throw new NotImplementedException();

		public Font Font => throw new NotImplementedException();

		public TextTransform TextTransform => throw new NotImplementedException();

		public double CharacterSpacing => throw new NotImplementedException();

		public FontAttributes FontAttributes => throw new NotImplementedException();

		public string FontFamily => throw new NotImplementedException();

		public double FontSize => throw new NotImplementedException();

		public TextAlignment HorizontalTextAlignment => throw new NotImplementedException();

		public TextAlignment VerticalTextAlignment => throw new NotImplementedException();

		public void Completed()
		{
			throw new NotImplementedException();
		}

		public string UpdateTransformedText(string source, TextTransform textTransform)
		{
			throw new NotImplementedException();
		}
	}
}

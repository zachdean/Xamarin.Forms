using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Platform;

namespace Sample
{
	public class Button : FontElement, IButton
	{
		const double DefaultSpacing = 10;

		public string Text { get; set; }

		public Color Color { get; set; }

		public int CornerRadius { get; set; } = -1;

		public Color BorderColor { get; set; }

		public double BorderWidth { get; set; } = -1d;

		public TextTransform TextTransform { get; set; }

		public TextAlignment HorizontalTextAlignment { get; set; }

		public TextAlignment VerticalTextAlignment { get; set; }

		public double CharacterSpacing { get; set; }

		public LineBreakMode LineBreakMode { get; set; }

		public ButtonContentLayout ContentLayout { get; set; } = new ButtonContentLayout(ButtonContentLayout.ImagePosition.Left, DefaultSpacing);

		public Thickness Padding { get; set; } = default;

		public ICommand Command { get; set; }

		public object CommandParameter { get; set; }

		string IText.UpdateTransformedText(string source, TextTransform textTransform)
			=> TextTransformUtilites.GetTransformedText(source, textTransform);

		public Action Pressed { get; set; }
		public Action Released { get; set; }
		public Action Clicked { get; set; }

		void IButton.Pressed() => Pressed?.Invoke();
		void IButton.Released() => Released?.Invoke();

		void IButton.Clicked()
		{
			Command?.Execute(CommandParameter);
			Clicked?.Invoke();
		}
	}
}
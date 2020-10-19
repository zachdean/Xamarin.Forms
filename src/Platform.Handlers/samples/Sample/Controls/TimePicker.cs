using System;
using Xamarin.Forms;
using Xamarin.Platform;

namespace Sample
{
	public class TimePicker : View, ITimePicker
	{
		public string Format { get; set; } = "t";

		public TimeSpan Time { get; set; } = new TimeSpan(0);

		public string Text { get; set; }

		public Color Color { get; set; } = Color.Default;

		public Font Font { get; set; }

		public TextTransform TextTransform { get; set; }

		public double CharacterSpacing { get; set; }

		public FontAttributes FontAttributes { get; set; }

		public string FontFamily { get; set; }

		public double FontSize { get; set; }

		public TextAlignment HorizontalTextAlignment { get; set; }

		public TextAlignment VerticalTextAlignment { get; set; }
	}
}
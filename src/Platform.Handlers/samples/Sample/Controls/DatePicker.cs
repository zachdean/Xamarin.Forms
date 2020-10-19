using System;
using Xamarin.Forms;
using Xamarin.Platform;

namespace Sample
{
	public class DatePicker : View, IDatePicker
	{
		public string Format { get; set; } = "d";

		public DateTime Date { get; set; } = DateTime.Today;

		public DateTime MinimumDate { get; set; } = new DateTime(1900, 1, 1);

		public DateTime MaximumDate { get; set; } = new DateTime(2100, 12, 31);

		public string Text { get; set; }

		public Color Color { get; set; }

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
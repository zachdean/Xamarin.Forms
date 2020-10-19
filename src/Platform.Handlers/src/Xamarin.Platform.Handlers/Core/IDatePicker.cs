using System;

namespace Xamarin.Platform
{
	public interface IDatePicker : IText
	{
		public string Format { get; set; }
		DateTime Date { get; set; }
		DateTime MinimumDate { get; }
		DateTime MaximumDate { get; }
	}
}
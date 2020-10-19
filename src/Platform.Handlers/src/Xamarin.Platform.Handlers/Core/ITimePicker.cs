using System;

namespace Xamarin.Platform
{
	public interface ITimePicker : IText
	{
		string Format { get; set; }
		TimeSpan Time { get; set; }
	}
}
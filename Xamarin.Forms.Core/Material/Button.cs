using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Forms.Material
{
	public enum Style
	{
		Filled, Outline, Text
	}

	public static class Button
	{
		public static readonly BindableProperty StyleProperty = BindableProperty.CreateAttached("Style", typeof(Style), typeof(Button), Style.Filled);

		public static Style GetStyle(BindableObject bindable)
		{
			return (Style)bindable.GetValue(StyleProperty);
		}
		public static void SetStyle(BindableObject bindable, Style value)
		{
			bindable.SetValue(StyleProperty, value);
		}
	}
}
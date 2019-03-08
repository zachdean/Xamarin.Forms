using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

[assembly: XmlnsDefinition("http://xamarin.com/schemas/2014/forms", "Xamarin.Forms.Material")]
namespace Xamarin.Forms.Material
{
	public enum Style
	{
		Filled, Outline, Text
	}


	public static class MaterialButton
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
	public static class FormsMaterial
	{
		public static void KeepAround()
		{
			
		}
	}
}
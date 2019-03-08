using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

[assembly: XmlnsDefinition("http://xamarin.com/schemas/2014/forms", "Xamarin.Forms.Material")]
namespace Xamarin.Forms.Material
{
	public enum ButtonStyle
	{
		Filled, Outline, Text
	}


	public static class Material
	{
		public static readonly BindableProperty ButtonStyleProperty = BindableProperty.CreateAttached("ButtonStyle", typeof(ButtonStyle), typeof(Material), ButtonStyle.Filled);

		public static ButtonStyle GetButtonStyle(BindableObject bindable)
		{
			return (ButtonStyle)bindable.GetValue(ButtonStyleProperty);
		}
		public static void SetButtonStyle(BindableObject bindable, ButtonStyle value)
		{
			bindable.SetValue(ButtonStyleProperty, value);
		}
	}

	public static class FormsMaterial
	{
		public static void Init()
		{
			// my only purpose is to exist so when called
			// this dll doesn't get removed

			VisualMarker.RegisterMaterial();
		}
	}
}
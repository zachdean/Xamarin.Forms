using System.Windows.Input;

namespace Xamarin.Forms
{
	public abstract class ContextItem : ContentView
	{
		public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(ContextItem), string.Empty, BindingMode.OneWay, null, OnTextChanged);
		public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(double), typeof(ContextItem), 10.0d, BindingMode.OneWay, null, OnTextChanged);
		public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(ContextItem), string.Empty, BindingMode.OneWay, null, OnTextChanged);
		public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(ContextItem), Color.Default, BindingMode.OneWay, null, OnTextChanged);
		public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ContextItem), null);
		public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ContextItem), null);
		public static readonly BindableProperty IconProperty = BindableProperty.Create(nameof(Icon), typeof(ImageSource), typeof(ContextItem), default(ImageSource), BindingMode.OneWay, null, OnIconChanged);

		public string FontFamily
		{
			get { return (string)GetValue(FontFamilyProperty); }
			set { SetValue(FontFamilyProperty, value); }
		}

		[TypeConverter(typeof(FontSizeConverter))]
		public double FontSize
		{
			get { return (double)GetValue(FontSizeProperty); }
			set { SetValue(FontSizeProperty, value); }
		}

		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		public Color TextColor
		{
			get { return (Color)GetValue(TextColorProperty); }
			set { SetValue(TextColorProperty, value); }
		}

		public ICommand Command
		{
			get { return (ICommand)GetValue(CommandProperty); }
			set { SetValue(CommandProperty, value); }
		}

		public object CommandParameter
		{
			get { return GetValue(CommandParameterProperty); }
			set { SetValue(CommandParameterProperty, value); }
		}

		public ImageSource Icon
		{
			get { return (ImageSource)GetValue(IconProperty); }
			set { SetValue(IconProperty, value); }
		}

		protected virtual void OnFontFamilyChanged(string fontFamily)
		{
		}

		protected virtual void OnFontSizeChanged(double fontSize)
		{
		}

		protected virtual void OnTextChanged(string text)
		{
		}

		protected virtual void OnTextColorChanged(Color color)
		{
		}

		protected virtual void OnIconChanged(ImageSource icon)
		{
		}

		private static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var contextItem = (ContextItem)bindable;

			if (Equals(newValue, null) && !Equals(oldValue, null))
				return;

			contextItem.OnTextChanged(contextItem.Text);
			contextItem.OnTextColorChanged(contextItem.TextColor);
			contextItem.OnFontFamilyChanged(contextItem.FontFamily);
			contextItem.OnFontSizeChanged(contextItem.FontSize);
		}

		private static void OnIconChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var contextItem = (ContextItem)bindable;

			if (Equals(newValue, null) && !Equals(oldValue, null))
				return;

			contextItem.OnIconChanged(contextItem.Icon);
		}
	}
}
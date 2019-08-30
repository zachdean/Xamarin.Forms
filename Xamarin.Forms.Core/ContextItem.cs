using System;
using System.Windows.Input;

namespace Xamarin.Forms
{
	public class ContextItem : ContentView, IDisposable
	{
		private TapGestureRecognizer _tapGestureRecognizer;

		public ContextItem()
		{
			_tapGestureRecognizer = new TapGestureRecognizer
			{
				Command = Command,
				CommandParameter = CommandParameter
			};

			GestureRecognizers.Add(_tapGestureRecognizer);

			Margin = new Thickness(0);

			var content = new Grid
			{
				RowSpacing = 0
			};
			content.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
			content.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

			ItemIcon = new Image
			{
				Aspect = Aspect.AspectFit,
				Source = Icon,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				Margin = new Thickness(6)
			};
			content.Children.Add(ItemIcon, 0, 0);

			ItemText = new Label
			{
				Text = Text,
				HorizontalOptions = LayoutOptions.Center
			};
			content.Children.Add(ItemText, 0, 1);

			Content = content;
		}

		public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(ContextItem), string.Empty, BindingMode.OneWay, null, OnTextChanged);
		public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(ContextItem), Color.Default, BindingMode.OneWay, null, OnTextColorChanged);
		public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ContextItem), null);
		public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ContextItem), null);
		public static readonly BindableProperty IconProperty = BindableProperty.Create(nameof(Icon), typeof(ImageSource), typeof(ContextItem), default(ImageSource), BindingMode.OneWay, null, OnIconChanged);

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

        internal Image ItemIcon { get; private set; }

        internal Label ItemText { get; private set; }
		       
		public void Dispose()
		{
			GestureRecognizers.Remove(_tapGestureRecognizer);
			_tapGestureRecognizer = null;
			ItemIcon = null;
			ItemText = null;
		}

		private static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var contextItem = (ContextItem)bindable;

			if (Equals(newValue, null) && !Equals(oldValue, null))
				return;

			contextItem.ItemText.Text = contextItem.Text;
		}

		private static void OnTextColorChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var contextItem = (ContextItem)bindable;

			if (Equals(newValue, null) && !Equals(oldValue, null))
				return;

			contextItem.ItemText.TextColor = contextItem.TextColor;
		}

		private static void OnIconChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var contextItem = (ContextItem)bindable;

			if (Equals(newValue, null) && !Equals(oldValue, null))
				return;

			contextItem.ItemIcon.Source = contextItem.Icon;
		}
	}
}

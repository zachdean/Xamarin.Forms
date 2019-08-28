using System.Windows.Input;

namespace Xamarin.Forms
{
	public class ContextItem : ContentView
	{
		public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(ContextItem), string.Empty);
		public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ContextItem), null);
		public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ContextItem), null);
		public static readonly BindableProperty IconProperty = BindableProperty.Create(nameof(Icon), typeof(ImageSource), typeof(ContextItem), default(ImageSource));

		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
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

		public ContextItem()
		{
			GestureRecognizers.Add(new TapGestureRecognizer
			{
				Command = Command,
				CommandParameter = CommandParameter
			});


			var content = new Grid();
			var textLabel = new Label
			{
				Text = Text
			};
			content.Children.Add(textLabel);
			Content = content;
		}
	}
}

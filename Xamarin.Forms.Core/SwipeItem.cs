using System;
using System.ComponentModel;

namespace Xamarin.Forms
{
	public class SwipeItem : ContextItem
	{
		public event EventHandler<SwipeItemInvokedEventArgs> Invoked;

		public SwipeItem()
		{
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

		internal Image ItemIcon { get; private set; }

		internal Label ItemText { get; private set; }
  
		protected override void OnTextChanged(string text)
		{
			base.OnTextChanged(text);
			ItemText.Text = text;
		}

		protected override void OnTextColorChanged(Color color)
		{
			base.OnTextColorChanged(color);
			ItemText.TextColor = color;
		}

		protected override void OnFontFamilyChanged(string fontFamily)
		{
			base.OnFontFamilyChanged(fontFamily);
			ItemText.FontFamily = fontFamily;
		}

		protected override void OnFontSizeChanged(double fontSize)
		{
			base.OnFontSizeChanged(fontSize);
			ItemText.FontSize = fontSize;
		}

		protected override void OnIconChanged(ImageSource icon)
		{
			base.OnIconChanged(icon);
			ItemIcon.Source = icon;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public void OnInvoked() => Invoked?.Invoke(this, new SwipeItemInvokedEventArgs(this));
	}

	public class SwipeItemInvokedEventArgs : EventArgs
	{
		public SwipeItemInvokedEventArgs(SwipeItem swipeItem)
		{
			SwipeItem = swipeItem;
		}

		public SwipeItem SwipeItem { get; set; }
	}
}
using Xamarin.Forms;
using Xamarin.Platform;
using Xamarin.Platform.Core;

namespace Sample
{
	public class MyApp : IApp
	{
		public MyApp()
		{
			Sample.Platform.Init();
		}

		public IView CreateView()
		{
			return new Button();
		}

		//public IView[] TestEntry = new IView[]
		//{
		//	new Label { Text = "Entry Gallery", FontSize = 24 },
		//	new Entry { BackgroundColor = Color.Blue },
		//	new Entry { Text = "Text" },
		//	new Entry { Text = "TextTransform (Uppercase)", TextTransform = TextTransform.Uppercase },
		//	new Entry { Text = "TextColor", TextColor = Color.OrangeRed },
		//	new Entry { Placeholder = "Placeholder" },
		//	new Entry { Placeholder = "PlaceholderColor", PlaceholderColor = Color.AliceBlue },
		//	new Entry { Text = "FontAttributes", FontAttributes = FontAttributes.Bold },
		//	new Entry { Text = "FontSize", FontSize = 24 },
		//	new Entry { Text = "CharacterSpacing", BackgroundColor = Color.Blue, CharacterSpacing = 12 },
		//	new Entry { Text = "HorizontalTextAlignment", HorizontalTextAlignment = TextAlignment.End },
		//};

		//public IView[] TestLabel = new IView[]
		//{
		//	new Label { Text = "Label Gallery", FontSize = 24 },
		//	new Label { Text = "Label" },
		//	new Label { Text = "BackgroundColor", BackgroundColor = Color.Orange },
		//	new Label { Text = "TextColor", TextColor = Color.Red },
		//	new Label { Text = "FontAttributes", FontAttributes =  FontAttributes.Bold },
		//	new Label { Text = "FontSize", FontSize = 24 },
		//	new Label { Text = "LineHeight", LineHeight = 12 },
		//	new Label { Text = "CharacterSpacing", CharacterSpacing = 12 },
		//	new Label { Text = "TextDecorations", TextDecorations = TextDecorations.Underline },
		//	new Label { Text = "TextTransform (Uppercase)", TextTransform = TextTransform.Uppercase },
		//	new Label { Text = "HorizontalTextAlignment", HorizontalTextAlignment = TextAlignment.End },
		//	new Label
		//	{
		//		Text = "This is <strong style=\"color:red\">HTML</strong> text.",
		//		TextType = TextType.Html
		//	},
		//	CreateFormattedLabel()
		//};

		//static Label CreateFormattedLabel()
		//{
		//	FormattedString formattedString = new FormattedString();

		//	formattedString.Spans.Add(new Span
		//	{
		//		Text = "Lorem ipsum"
		//	});

		//	formattedString.Spans.Add(new Span
		//	{
		//		Text = "dolor sit amet."
		//	});

		//	Label label = new Label
		//	{
		//		FormattedText = formattedString
		//	};

		//	return label;
		//}
	}
}
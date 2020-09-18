using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;

namespace Xamarin.Platform.Handlers
{
	public partial class EntryHandler : AbstractViewHandler<IEntry, UITextField>
	{
		protected override UITextField CreateView()
		{
			var textField = new UITextField(CGRect.Empty)
			{
				BorderStyle = UITextBorderStyle.RoundedRect,
				ClipsToBounds = true
			};

			return textField;
		}

		public static void MapText(IViewHandler handler, IEntry view)
		{
			(handler as EntryHandler)?.UpdateText();
		}

		public static void MapColor(IViewHandler handler, IEntry entry)
		{
	
		}

		public static void MapFont(IViewHandler handler, IEntry entry)
		{
		
		}

		public static void MapTextTransform(IViewHandler handler, IEntry entry)
		{
			(handler as EntryHandler)?.UpdateText();
		}

		public static void MapCharacterSpacing(IViewHandler handler, IEntry entry)
		{
			(handler as EntryHandler)?.UpdateCharacterSpacing();
		}

		public static void MapPlaceholder(IViewHandler handler, IEntry entry)
		{
	
		}

		public static void MapMaxLength(IViewHandler handler, IEntry entry)
		{
			(handler as EntryHandler)?.UpdateMaxLength();
		}

		public static void MapIsReadOnly(IViewHandler handler, IEntry entry)
		{
			
		}

		public static void MapKeyboard(IViewHandler handler, IEntry entry)
		{
			
		}

		public static void MapIsSpellCheckEnabled(IViewHandler handler, IEntry entry)
		{
		
		}

		public static void MapPlaceholderColor(IViewHandler handler, IEntry entry)
		{
		
		}
				
		public static void MapHorizontalTextAlignment(IViewHandler handler, IEntry entry)
		{
			(handler as EntryHandler)?.UpdateHorizontalTextAlignment();
		}

		public static void MapVerticalTextAlignment(IViewHandler handler, IEntry entry)
		{
			(handler as EntryHandler)?.UpdateVerticalTextAlignment();
		}

		public static void MapFontSize(IViewHandler handler, IEntry entry)
		{
		
		}

		public static void MapFontAttributes(IViewHandler handler, IEntry entry)
		{
			
		}

		public static void MapIsPassword(IViewHandler handler, IEntry entry)
		{
		
		}

		public static void MapReturnType(IViewHandler handler, IEntry entry)
		{
		
		}

		public static void MapCursorPosition(IViewHandler handler, IEntry entry)
		{
		
		}

		public static void MapSelectionLength(IViewHandler handler, IEntry entry)
		{
			
		}

		public static void MapIsTextPredictionEnabled(IViewHandler handler, IEntry entry)
		{
		
		}

		public static void MapClearButtonVisibility(IViewHandler handler, IEntry entry)
		{
		
		}

		void UpdateText()
		{
			var text = VirtualView.UpdateTransformedText(VirtualView.Text, VirtualView.TextTransform);
	
			if (TypedNativeView.Text != text)
				TypedNativeView.Text = text;
		}

		void UpdateCharacterSpacing()
		{
			var textAttr = TypedNativeView.AttributedText.AddCharacterSpacing(VirtualView.Text, VirtualView.CharacterSpacing);

			if (textAttr != null)
				TypedNativeView.AttributedText = textAttr;

			var placeHolder = TypedNativeView.AttributedPlaceholder.AddCharacterSpacing(VirtualView.Placeholder, VirtualView.CharacterSpacing);

			if (placeHolder != null)
				UpdateAttributedPlaceholder(placeHolder);
		}

		void UpdateMaxLength()
		{
			var currentControlText = TypedNativeView.Text;

			if (currentControlText.Length > VirtualView.MaxLength)
				TypedNativeView.Text = currentControlText.Substring(0, VirtualView.MaxLength);
		}

		void UpdateHorizontalTextAlignment()
		{
			// TODO: Pass the EffectiveFlowDirection.
			TypedNativeView.TextAlignment = VirtualView.HorizontalTextAlignment.ToNativeTextAlignment(EffectiveFlowDirection.Explicit);
		}

		void UpdateVerticalTextAlignment()
		{
			TypedNativeView.VerticalAlignment = VirtualView.VerticalTextAlignment.ToNativeTextAlignment();
		}

		void UpdateAttributedPlaceholder(NSAttributedString nsAttributedString) =>
			TypedNativeView.AttributedPlaceholder = nsAttributedString;
	}
}

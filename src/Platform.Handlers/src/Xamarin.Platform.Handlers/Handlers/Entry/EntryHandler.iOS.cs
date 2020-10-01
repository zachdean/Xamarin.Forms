using System;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;

namespace Xamarin.Platform.Handlers
{
	public partial class EntryHandler : AbstractViewHandler<IEntry, UITextField>
	{
		IDisposable? _selectedTextRangeObserver;

		protected override UITextField CreateView()
		{
			var textField = new UITextField(CGRect.Empty)
			{
				BorderStyle = UITextBorderStyle.RoundedRect,
				ClipsToBounds = true
			};

			textField.EditingChanged += OnEditingChanged;
			textField.ShouldReturn = OnShouldReturn;

			textField.EditingDidBegin += OnEditingBegan;
			textField.EditingDidEnd += OnEditingEnded;
			textField.ShouldChangeCharacters += ShouldChangeCharacters;

			_selectedTextRangeObserver = textField.AddObserver("selectedTextRange", NSKeyValueObservingOptions.New, UpdateCursor);

			return textField;
		}

		public override void TearDown()
		{
			base.TearDown();

			_selectedTextRangeObserver?.Dispose();
		}

		void OnEditingChanged(object sender, EventArgs e)
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			TypedNativeView.Text = VirtualView.Text;
			TypedNativeView.UpdateCursor(VirtualView, null);
		}

		bool OnShouldReturn(UITextField textField)
		{
			textField.ResignFirstResponder();
			VirtualView?.Completed();

			if (VirtualView != null && VirtualView.ReturnType == ReturnType.Next)
			{
				//FocusSearch(true);
			}

			return false;
		}

		void OnEditingBegan(object sender, EventArgs e)
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			TypedNativeView.UpdateCursorOnEditingBegan(VirtualView);
		}

		void OnEditingEnded(object sender, EventArgs e)
		{
			// Typing aid changes don't always raise EditingChanged event
			// Normalizing nulls to string.Empty allows us to ensure that a change from null to "" doesn't result in a change event.
			// While technically this is a difference it serves no functional good.
			if (VirtualView == null)
				return;

			var controlText = TypedNativeView?.Text ?? string.Empty;
			var entryText = VirtualView.Text ?? string.Empty;

			if (controlText != entryText)
			{
				VirtualView.Text = controlText;
			}
		}

		bool ShouldChangeCharacters(UITextField textField, NSRange range, string replacementString)
		{
			var newLength = textField?.Text?.Length + replacementString?.Length - range.Length;
			return newLength <= VirtualView?.MaxLength;
		}

		void UpdateCursor(NSObservedChange obj)
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			TypedNativeView.UpdateCursor(VirtualView, null);
		}
	}
}
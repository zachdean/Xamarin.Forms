using System;
using System.ComponentModel;
using Foundation;
using UIKit;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using RectangleF = CoreGraphics.CGRect;

namespace Xamarin.Forms.Platform.iOS
{
	public class TimePickerRenderer : TimePickerRendererBase<UIControl>
	{
		[Internals.Preserve(Conditional = true)]
		public TimePickerRenderer()
		{

		}

		protected override UIControl CreateNativeControl()
		{
			var datePickerStyle = Element.OnThisPlatform().UIDatePickerStyle();
			if (datePickerStyle != PlatformConfiguration.iOSSpecific.UIDatePickerStyle.Automatic)
				return new UIDatePicker { Mode = UIDatePickerMode.Time, TimeZone = new NSTimeZone("UTC") };
			else
				return new NoCaretField { BorderStyle = UITextBorderStyle.RoundedRect };
		}
	}

	public abstract class TimePickerRendererBase<TControl> : ViewRenderer<TimePicker, TControl>
		where TControl : UIControl
	{
		UIDatePicker _picker;
		protected UITextField TextField => Control as UITextField;
		UIColor _defaultTextColor;
		bool _disposed;
		bool _useLegacyColorManagement;

		IElementController ElementController => Element as IElementController;

		[Internals.Preserve(Conditional = true)]
		public TimePickerRendererBase()
		{

		}

		protected override void Dispose(bool disposing)
		{
			if (_disposed)
				return;

			_disposed = true;

			if (disposing)
			{
				_defaultTextColor = null;

				if (_picker != null)
				{
					_picker.RemoveFromSuperview();
					_picker.ValueChanged -= OnValueChanged;
					_picker.Dispose();
					_picker = null;
				}

				if (Control != null)
				{
					Control.EditingDidBegin -= OnStarted;
					Control.EditingDidEnd -= OnEnded;
				}
			}

			base.Dispose(disposing);
		}

		protected abstract override TControl CreateNativeControl();

		protected override void OnElementChanged(ElementChangedEventArgs<TimePicker> e)
		{
			if (e.NewElement != null)
			{
				if (Control == null)
				{
					var control = CreateNativeControl();

					//iOS14 uses UIDatePicker compact
					if ((Control is UIDatePicker))
					{
						_picker = control as UIDatePicker;
					}
					else
					{
						_picker = new UIDatePicker { Mode = UIDatePickerMode.Time, TimeZone = new NSTimeZone("UTC") };

						if (Forms.IsiOS14OrNewer)
						{
							_picker.PreferredDatePickerStyle = UIKit.UIDatePickerStyle.Wheels;
						}

						//Not using iOS14 or using the MaterialTimePicker
						var entry = control as UITextField;

						InitTextField(entry);
					}

					UpdateDatePickerStyle();
					_picker.ValueChanged += OnValueChanged;

					SetNativeControl(control);
				}

				UpdateFont();
				UpdateTime();
				UpdateTextColor();
				UpdateCharacterSpacing();
				UpdateFlowDirection();
			}

			base.OnElementChanged(e);
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == TimePicker.TimeProperty.PropertyName || e.PropertyName == TimePicker.FormatProperty.PropertyName)
			{
				UpdateTime();
				UpdateCharacterSpacing();
			}
			else if (e.PropertyName == TimePicker.TextColorProperty.PropertyName || e.PropertyName == VisualElement.IsEnabledProperty.PropertyName)
				UpdateTextColor();
			else if (e.PropertyName == TimePicker.CharacterSpacingProperty.PropertyName)
				UpdateCharacterSpacing();
			else if (e.PropertyName == TimePicker.FontAttributesProperty.PropertyName ||
					 e.PropertyName == TimePicker.FontFamilyProperty.PropertyName || e.PropertyName == TimePicker.FontSizeProperty.PropertyName)
				UpdateFont();
			else if (e.PropertyName == VisualElement.FlowDirectionProperty.PropertyName)
				UpdateFlowDirection();
		}

		void OnEnded(object sender, EventArgs eventArgs)
		{
			ElementController.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
		}

		void OnStarted(object sender, EventArgs eventArgs)
		{
			ElementController.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, true);
		}

		void OnValueChanged(object sender, EventArgs e)
		{
			if (Element.OnThisPlatform().UpdateMode() == UpdateMode.Immediately)
			{
				UpdateElementTime();
			}
		}

		void UpdateFlowDirection()
		{
			TextField?.UpdateTextAlignment(Element);
		}

		protected internal virtual void UpdateFont()
		{
			if (!Forms.IsiOS14OrNewer)
				TextField.Font = Element.ToUIFont();
		}

		protected internal virtual void UpdateTextColor()
		{
			var textColor = Element.TextColor;
			if (TextField != null)
			{
				if (textColor.IsDefault || (!Element.IsEnabled && _useLegacyColorManagement))
					TextField.TextColor = _defaultTextColor;
				else
					TextField.TextColor = textColor.ToUIColor();

				// HACK This forces the color to update; there's probably a more elegant way to make this happen
				TextField.Text = TextField.Text;
			}
		}

		void UpdateCharacterSpacing()
		{
			if (TextField != null)
			{
				var textAttr = TextField.AttributedText.AddCharacterSpacing(TextField.Text, Element.CharacterSpacing);

				if (textAttr != null)
					TextField.AttributedText = textAttr;
			}
		}

		void UpdateTime()
		{
			_picker.Date = new DateTime(1, 1, 1).Add(Element.Time).ToNSDate();
			if (TextField != null)
			{
				TextField.Text = DateTime.Today.Add(Element.Time).ToString(Element.Format);
				Element.InvalidateMeasureNonVirtual(Internals.InvalidationTrigger.MeasureChanged);
			}
		}

		void UpdateElementTime()
		{
			ElementController.SetValueFromRenderer(TimePicker.TimeProperty, _picker.Date.ToDateTime() - new DateTime(1, 1, 1));
		}

		void InitTextField(UITextField textField)
		{
			if (textField == null)
				return;

			textField.EditingDidBegin += OnStarted;
			textField.EditingDidEnd += OnEnded;

			var width = UIScreen.MainScreen.Bounds.Width;
			var toolbar = new UIToolbar(new RectangleF(0, 0, width, 44)) { BarStyle = UIBarStyle.Default, Translucent = true };
			var spacer = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);

			var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, (o, a) =>
			{
				UpdateElementTime();
				textField.ResignFirstResponder();
			});

			toolbar.SetItems(new[] { spacer, doneButton }, false);

			textField.InputView = _picker;
			textField.InputAccessoryView = toolbar;

			textField.InputView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;
			textField.InputAccessoryView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;

			textField.InputAssistantItem.LeadingBarButtonGroups = null;
			textField.InputAssistantItem.TrailingBarButtonGroups = null;

			_defaultTextColor = textField.TextColor;

			textField.AccessibilityTraits = UIAccessibilityTrait.Button;
		}

		void UpdateDatePickerStyle()
		{
			var datePickerStyle = Element.OnThisPlatform().UIDatePickerStyle();

			switch (datePickerStyle)
			{
				case PlatformConfiguration.iOSSpecific.UIDatePickerStyle.Compact:
					_picker.PreferredDatePickerStyle = UIKit.UIDatePickerStyle.Compact;
					break;
				case PlatformConfiguration.iOSSpecific.UIDatePickerStyle.Inline:
					_picker.PreferredDatePickerStyle = UIKit.UIDatePickerStyle.Inline;
					break;
				case PlatformConfiguration.iOSSpecific.UIDatePickerStyle.Wheels:
					_picker.PreferredDatePickerStyle = UIKit.UIDatePickerStyle.Wheels;
					break;
				default:
					break;
			}
		}
	}
}
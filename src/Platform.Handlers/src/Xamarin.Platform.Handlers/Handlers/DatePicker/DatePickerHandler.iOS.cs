using System;
using Foundation;
using UIKit;
using RectangleF = CoreGraphics.CGRect;

namespace Xamarin.Platform.Handlers
{
	public partial class DatePickerHandler : AbstractViewHandler<IDatePicker, NativeDatePicker>
	{
		static UIDatePicker? Picker;
		static UIColor? DefaultTextColor;

		protected override NativeDatePicker CreateNativeView()
		{
			NativeDatePicker nativeDatePicker = new NativeDatePicker();

			Picker = new UIDatePicker { Mode = UIDatePickerMode.Date, TimeZone = new NSTimeZone("UTC") };

			if (NativeVersion.IsAtLeast(14))
			{
				Picker.PreferredDatePickerStyle = UIDatePickerStyle.Wheels;
			}

			var width = UIScreen.MainScreen.Bounds.Width;
			var toolbar = new UIToolbar(new RectangleF(0, 0, width, 44)) { BarStyle = UIBarStyle.Default, Translucent = true };
			var spacer = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
			var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, (o, a) =>
			{
				SetVirtualViewDate();
				nativeDatePicker.ResignFirstResponder();
			});

			toolbar.SetItems(new[] { spacer, doneButton }, false);

			nativeDatePicker.InputView = Picker;
			nativeDatePicker.InputAccessoryView = toolbar;

			nativeDatePicker.InputView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;
			nativeDatePicker.InputAccessoryView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;

			nativeDatePicker.InputAssistantItem.LeadingBarButtonGroups = null;
			nativeDatePicker.InputAssistantItem.TrailingBarButtonGroups = null;

			nativeDatePicker.AccessibilityTraits = UIAccessibilityTrait.Button;

			return nativeDatePicker;
		}

		protected override void ConnectHandler(NativeDatePicker nativeView)
		{
			nativeView.EditingDidBegin += OnStarted;
			nativeView.EditingDidEnd += OnEnded;

			if (Picker != null)
				Picker.ValueChanged += OnValueChanged;

			base.ConnectHandler(nativeView);
		}

		protected override void DisconnectHandler(NativeDatePicker nativeView)
		{
			nativeView.EditingDidBegin -= OnStarted;
			nativeView.EditingDidEnd -= OnEnded;

			if (Picker != null)
				Picker.ValueChanged -= OnValueChanged;

			base.DisconnectHandler(nativeView);
		}

		protected override void SetupDefaults(NativeDatePicker nativeView)
		{
			DefaultTextColor = nativeView.TextColor;

			base.SetupDefaults(nativeView);
		}

		public static void MapFormat(DatePickerHandler handler, IDatePicker datePicker)
		{
			ViewHandler.CheckParameters(handler, datePicker);
			handler.TypedNativeView?.UpdateFormat(datePicker);
		}

		public static void MapDate(DatePickerHandler handler, IDatePicker datePicker)
		{
			ViewHandler.CheckParameters(handler, datePicker);
			handler.TypedNativeView?.UpdateDate(datePicker);
		}

		public static void MapMinimumDate(DatePickerHandler handler, IDatePicker datePicker)
		{
			ViewHandler.CheckParameters(handler, datePicker);
			handler.TypedNativeView?.UpdateMinimumDate(datePicker, Picker);
		}

		public static void MapMaximumDate(DatePickerHandler handler, IDatePicker datePicker)
		{
			ViewHandler.CheckParameters(handler, datePicker);
			handler.TypedNativeView?.UpdateMaximumDate(datePicker, Picker);
		}

		public static void MapColor(DatePickerHandler handler, IDatePicker datePicker)
		{
			ViewHandler.CheckParameters(handler, datePicker);
			handler.TypedNativeView?.UpdateTextColor(datePicker, DefaultTextColor);
		}

		public static void MapFont(DatePickerHandler handler, IDatePicker datePicker)
		{
			ViewHandler.CheckParameters(handler, datePicker);
			handler.TypedNativeView?.UpdateFont(datePicker);
		}

		public static void MapCharacterSpacing(DatePickerHandler handler, IDatePicker datePicker)
		{
			ViewHandler.CheckParameters(handler, datePicker);
			handler.TypedNativeView?.UpdateCharacterSpacing(datePicker);
		}

		void OnStarted(object sender, EventArgs e)
		{

		}

		void OnEnded(object sender, EventArgs e)
		{

		}

		void OnValueChanged(object sender, EventArgs e)
		{
			SetVirtualViewDate();
		}

		void SetVirtualViewDate()
		{
			if (VirtualView == null || Picker == null)
				return;

			VirtualView.Date = Picker.Date.ToDateTime().Date;
		}
	}
}
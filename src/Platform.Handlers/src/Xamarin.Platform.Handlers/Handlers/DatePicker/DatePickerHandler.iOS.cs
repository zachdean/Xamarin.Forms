using System;
using Foundation;
using UIKit;
using RectangleF = CoreGraphics.CGRect;

namespace Xamarin.Platform.Handlers
{
	public partial class DatePickerHandler : AbstractViewHandler<IDatePicker, NativeDatePicker>
	{
		NativeDatePicker? _nativeDatePicker;
		static UIDatePicker? Picker;
		static UIColor? DefaultTextColor;

		protected override NativeDatePicker CreateView()
		{
			_nativeDatePicker = new NativeDatePicker();

			_nativeDatePicker.EditingDidBegin += OnStarted;
			_nativeDatePicker.EditingDidEnd += OnEnded;

			Picker = new UIDatePicker { Mode = UIDatePickerMode.Date, TimeZone = new NSTimeZone("UTC") };

			if (NativeVersion.IsAtLeast(14))
			{
				Picker.PreferredDatePickerStyle = UIKit.UIDatePickerStyle.Wheels;
			}

			Picker.ValueChanged += OnValueChanged;

			var width = UIScreen.MainScreen.Bounds.Width;
			var toolbar = new UIToolbar(new RectangleF(0, 0, width, 44)) { BarStyle = UIBarStyle.Default, Translucent = true };
			var spacer = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
			var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, (o, a) =>
			{
				SetVirtualViewDate();
				_nativeDatePicker.ResignFirstResponder();
			});

			toolbar.SetItems(new[] { spacer, doneButton }, false);

			_nativeDatePicker.InputView = Picker;
			_nativeDatePicker.InputAccessoryView = toolbar;

			_nativeDatePicker.InputView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;
			_nativeDatePicker.InputAccessoryView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;

			_nativeDatePicker.InputAssistantItem.LeadingBarButtonGroups = null;
			_nativeDatePicker.InputAssistantItem.TrailingBarButtonGroups = null;

			_nativeDatePicker.AccessibilityTraits = UIAccessibilityTrait.Button;

			return _nativeDatePicker;
		}

		protected override void SetupDefaults()
		{
			DefaultTextColor = _nativeDatePicker?.TextColor;

			base.SetupDefaults();
		}

		public override void TearDown()
		{
			DefaultTextColor = null;

			if (Picker != null)
			{
				Picker.RemoveFromSuperview();
				Picker.ValueChanged -= OnValueChanged;
				Picker.Dispose();
				Picker = null;
			}

			if (TypedNativeView != null)
			{
				TypedNativeView.EditingDidBegin -= OnStarted;
				TypedNativeView.EditingDidEnd -= OnEnded;
			}

			base.TearDown();
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
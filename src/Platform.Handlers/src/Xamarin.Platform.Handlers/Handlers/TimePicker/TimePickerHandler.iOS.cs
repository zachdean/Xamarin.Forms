using System;
using Foundation;
using UIKit;
using RectangleF = CoreGraphics.CGRect;

namespace Xamarin.Platform.Handlers
{
	public partial class TimePickerHandler : AbstractViewHandler<ITimePicker, NativeTimePicker>
	{
		NativeTimePicker? _nativeTimePicker;
		static UIDatePicker? Picker;
		static UIColor? DefaultTextColor;

		protected override NativeTimePicker CreateView()
		{
			_nativeTimePicker = new NativeTimePicker();

			_nativeTimePicker.EditingDidBegin += OnStarted;
			_nativeTimePicker.EditingDidEnd += OnEnded;

			Picker = new UIDatePicker { Mode = UIDatePickerMode.Time, TimeZone = new NSTimeZone("UTC") };

			if (NativeVersion.IsAtLeast(14))
			{
				Picker.PreferredDatePickerStyle = UIDatePickerStyle.Wheels;
			}

			var width = UIScreen.MainScreen.Bounds.Width;
			var toolbar = new UIToolbar(new RectangleF(0, 0, width, 44)) { BarStyle = UIBarStyle.Default, Translucent = true };
			var spacer = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);

			var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, (o, a) =>
			{
				SetVirtualViewTime();
				_nativeTimePicker.ResignFirstResponder();
			});

			toolbar.SetItems(new[] { spacer, doneButton }, false);

			_nativeTimePicker.InputView = Picker;
			_nativeTimePicker.InputAccessoryView = toolbar;

			_nativeTimePicker.InputView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;
			_nativeTimePicker.InputAccessoryView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;

			_nativeTimePicker.InputAssistantItem.LeadingBarButtonGroups = null;
			_nativeTimePicker.InputAssistantItem.TrailingBarButtonGroups = null;

			Picker.ValueChanged += OnValueChanged;

			_nativeTimePicker.AccessibilityTraits = UIAccessibilityTrait.Button;

			return _nativeTimePicker;
		}

		protected override void SetupDefaults()
		{
			DefaultTextColor = _nativeTimePicker?.TextColor;

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

			if (_nativeTimePicker != null)
			{
				_nativeTimePicker.EditingDidBegin -= OnStarted;
				_nativeTimePicker.EditingDidEnd -= OnEnded;
			}

			base.TearDown();
		}

		public static void MapFormat(TimePickerHandler handler, ITimePicker timePicker)
		{
			ViewHandler.CheckParameters(handler, timePicker);
			handler.TypedNativeView?.UpdateFormat(timePicker, Picker);
		}

		public static void MapTime(TimePickerHandler handler, ITimePicker timePicker)
		{
			ViewHandler.CheckParameters(handler, timePicker);
			handler.TypedNativeView?.UpdateTime(timePicker, Picker);
		}

		public static void MapColor(TimePickerHandler handler, ITimePicker timePicker)
		{
			ViewHandler.CheckParameters(handler, timePicker);
			handler.TypedNativeView?.UpdateTextColor(timePicker, DefaultTextColor);
		}

		public static void MapFont(TimePickerHandler handler, ITimePicker timePicker)
		{
			ViewHandler.CheckParameters(handler, timePicker);
			handler.TypedNativeView?.UpdateFont(timePicker);
		}

		public static void MapCharacterSpacing(TimePickerHandler handler, ITimePicker timePicker)
		{
			ViewHandler.CheckParameters(handler, timePicker);
			handler.TypedNativeView?.UpdateCharacterSpacing(timePicker);
		}

		void OnStarted(object sender, EventArgs e)
		{
		
		}

		void OnEnded(object sender, EventArgs e)
		{
	
		}

		void OnValueChanged(object sender, EventArgs e)
		{
			SetVirtualViewTime();
		}

		void SetVirtualViewTime()
		{
			if (VirtualView == null || Picker == null)
				return;

			VirtualView.Time = Picker.Date.ToDateTime() - new DateTime(1, 1, 1);
		}
	}
}
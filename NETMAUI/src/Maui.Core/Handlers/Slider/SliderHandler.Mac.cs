using AppKit;
using Foundation;
using System.Maui.Controls.Primitives;

namespace System.Maui.Platform
{
	public partial class SliderHandler : AbstractViewHandler<ISlider, NativeSlider>
	{
		protected override NativeSlider CreateView()
		{
			var slider = new NativeSlider
			{
				Cell = new NativeSliderCell(),
				Action = new ObjCRuntime.Selector(nameof(ValueChanged))
			};

			return slider;
		}

		protected override void DisposeView(NativeSlider slider)
		{
			slider.Action = null;

			base.DisposeView(slider);
		}

		public static void MapPropertyMinimum(IViewHandler Handler, ISlider slider)
		{
			if (!(Handler.NativeView is NativeSlider NativeSlider))
				return;

			NativeSlider.MinValue = (float)slider.Minimum;
		}

		public static void MapPropertyMaximum(IViewHandler Handler, ISlider slider)
		{
			if (!(Handler.NativeView is NativeSlider NativeSlider))
				return;

			NativeSlider.MaxValue = (float)slider.Maximum;
		}

		public static void MapPropertyValue(IViewHandler Handler, ISlider slider)
		{
			if (!(Handler.NativeView is NativeSlider NativeSlider))
				return;

			if (Math.Abs(slider.Value - NativeSlider.DoubleValue) > 0)
				NativeSlider.DoubleValue = (float)slider.Value;
		}

		public static void MapPropertyMinimumTrackColor(IViewHandler Handler, ISlider slider)
		{
			if (!(Handler.NativeView is NativeSlider NativeSlider))
				return;

			// Cell could be overwritten with an other custom cell
			if (NativeSlider.Cell is NativeSliderCell sliderCell)
			{
				sliderCell.MinimumTrackColor = slider.MinimumTrackColor;
			}
		}

		public static void MapPropertyMaximumTrackColor(IViewHandler Handler, ISlider slider)
		{
			if (!(Handler.NativeView is NativeSlider NativeSlider))
				return;

			// Cell could be overwritten with an other custom cell
			if (NativeSlider.Cell is NativeSliderCell sliderCell)
			{
				sliderCell.MaximumTrackColor = slider.MaximumTrackColor;
			}
		}

		public static void MapPropertyThumbColor(IViewHandler Handler, ISlider slider)
		{
			if (!(Handler.NativeView is NativeSlider NativeSlider))
				return;

			// Cell could be overwritten with an other custom cell
			if (NativeSlider.Cell is NativeSliderCell sliderCell)
			{
				sliderCell.ThumbColor = slider.ThumbColor;
			}
		}

		[Export(nameof(ValueChanged))]
		void ValueChanged()
		{
			VirtualView.Value = TypedNativeView.DoubleValue;
			VirtualView.ValueChanged();

			var controlEvent = NSApplication.SharedApplication.CurrentEvent;

			if (controlEvent.Type == NSEventType.LeftMouseDown)
				VirtualView.DragStarted();
			else if (controlEvent.Type == NSEventType.LeftMouseUp)
				VirtualView.DragCompleted();
		}
	}
}
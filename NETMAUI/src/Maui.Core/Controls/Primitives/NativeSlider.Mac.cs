using System.Maui.Platform;
using AppKit;
using CoreGraphics;

namespace System.Maui.Controls.Primitives
{
	public class NativeSlider : NSSlider
	{
		readonly CGSize _fitSize;

		internal NativeSlider() : base(CGRect.Empty)
		{
			Continuous = true;
			SizeToFit();
			var size = Bounds.Size;

			// This size will be set as default for horizontal NSSlider, if you try to create it via XCode (drag and drope)
			// See this screenshot: https://user-images.githubusercontent.com/10124814/52661252-aecb8100-2f12-11e9-8f45-c0dab8bc8ffc.png
			_fitSize = size.Width > 0 && size.Height > 0 ? size : new CGSize(96, 21);
		}

		public override CGSize SizeThatFits(CGSize size) => _fitSize;
	}

	public class NativeSliderCell : NSSliderCell
	{
		public Color MinimumTrackColor { get; set; }

		public Color MaximumTrackColor { get; set; }

		public Color ThumbColor { get; set; }

		public override void DrawBar(CGRect aRect, bool flipped)
		{
			// Mimick the dimensions of the original slider
			var originalHeight = aRect.Height;
			aRect.Height = 2.7f;
			var radius = aRect.Height / 2;
			aRect.Y += (originalHeight - aRect.Height) / 2;

			// Calc the progress percentage to know where one bar starts
			var progress = (float)((DoubleValue - MinValue) / (MaxValue - MinValue));

			var minTrackRect = aRect;
			minTrackRect.Width *= progress;

			var maxTrackRect = aRect;
			maxTrackRect.X += maxTrackRect.Width * progress;
			maxTrackRect.Width *= (1 - progress);

			// Draw min track
			var minTrackPath = NSBezierPath.FromRoundedRect(minTrackRect, radius, radius);

			var defaultMinTrackColor = Color.Accent.ToNative();

			if (NativeVersion.IsAtLeast(14))
				defaultMinTrackColor = NSColor.ControlAccentColor;

			var minTrackColor = MinimumTrackColor.IsDefault ? defaultMinTrackColor : MinimumTrackColor.ToNative();
			minTrackColor.SetFill();
			minTrackPath.Fill();

			var defaultMaxTrackColor = NSColor.ControlShadow;

			if (NativeVersion.IsAtLeast(14))
				defaultMaxTrackColor = NSColor.SeparatorColor;

			// Draw max track
			var maxTrackPath = NSBezierPath.FromRoundedRect(maxTrackRect, radius, radius);
			var maxTrackColor = MaximumTrackColor.IsDefault ? defaultMaxTrackColor : MaximumTrackColor.ToNative();
			maxTrackColor.SetFill();
			maxTrackPath.Fill();
		}

		public override void DrawKnob(CGRect knobRect)
		{
			// Mimick the dimensions of the original slider
			knobRect.Width -= 6;
			knobRect.Height -= 6;
			knobRect.Y += 3;
			knobRect.X += 3;
			var radius = 7.5f;

			var path = new NSBezierPath();
			path.AppendPathWithRoundedRect(knobRect, radius, radius);
			// Draw inside

			var defaultKnobColor = NSColor.ControlLightHighlight;

			if (NativeVersion.IsAtLeast(14))
				defaultKnobColor = NSColor.Highlight;

			var knobColor = ThumbColor.IsDefault ? defaultKnobColor : ThumbColor.ToNative();
			knobColor.SetFill();
			path.Fill();

			// Draw border
			if(NativeVersion.IsAtLeast(14))
				NSColor.ControlShadow.SetStroke();
			else
				NSColor.SeparatorColor.SetStroke();

			path.Stroke();
		}
	}
}
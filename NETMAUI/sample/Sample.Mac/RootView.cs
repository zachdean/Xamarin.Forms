using System;
using System.Collections.Generic;
using AppKit;
using CoreGraphics;
using System.Maui;

namespace Sample.Mac
{
	class RootView : NSScrollView
	{
		readonly StackView _stackView = new StackView();

		public RootView()
		{
			BackgroundColor = NSColor.White;

			DocumentView = _stackView;
		}

		public nfloat Spacing { get; set; } = 6;

		public void Add(IEnumerable<IView> views)
		{
			foreach (var view in views)
				Add(view);
		}

		public void Add(IView view)
		{
			_stackView.Add(view);
		}

		public override void Layout()
		{
			_stackView.Layout();
			base.Layout();
		}
	}

	class StackView : NSView
	{
		readonly List<IView> _views;

		public StackView()
		{
			_views = new List<IView>();
		}

		public nfloat Spacing { get; set; } = 6.0f;

		public nfloat MinimumHeight { get; set; } = 24.0f;

		public void Add(IView view)
		{
			_views.Add(view);
			AddSubview(view.ToNative());
		}

		public override bool IsFlipped => true;

		public override void Layout()
		{
			base.Layout();

			var bounds = Bounds;
			nfloat lastY = 0;
			CGRect frame;
			var width = Superview.Bounds.Width;
			nfloat maxWidth = width;

			foreach (var subView in Subviews)
			{
				//TODO: Lets hook into the renders size!
				(subView as NSControl)?.SizeToFit();
				frame = subView.Bounds;

				maxWidth = NMath.Max(maxWidth, frame.Width);
				frame.Y = lastY;

				frame.X = 0;
				lastY = frame.Bottom + Spacing;

				if (SizeToFill(subView))
					frame.Width = maxWidth;

				if (frame.Height == 0)
					frame.Height = MinimumHeight;

				subView.Frame = frame;
				Console.WriteLine(frame);
			}

			bounds.Height = lastY;
			bounds.Width = maxWidth;

			Frame = bounds;

			if (Superview is NSScrollView scroll)
			{
				scroll.ContentView.Frame = bounds;
			}
		}

		bool SizeToFill(NSView view)
		{
			if (view is NSStepper || view is NSSwitch)
				return false;

			return true;
		}
	}
}
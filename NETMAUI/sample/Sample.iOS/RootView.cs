using System;
using System.Collections.Generic;
using System.Maui;
using System.Maui.Platform;
using CoreGraphics;
using UIKit;

namespace Sample.iOS
{
	class RootView : UIScrollView
	{
		readonly StackView _stackView = new StackView();

		public nfloat Spacing { get; set; } = 6;

		public RootView()
		{
			BackgroundColor = UIColor.White;
			AddSubview(_stackView);
			TranslatesAutoresizingMaskIntoConstraints = false;
		}

		public void Add(IEnumerable<IView> views)
		{
			foreach (var view in views)
				Add(view);
		}

		public void Add(IView view)
		{
			_stackView.Add(view);
		}
	}

	class StackView : UIView
	{
		readonly List<IView> _views;

		public StackView()
		{
			_views = new List<IView>();
		}

		public nfloat Spacing { get; set; } = 6;

		public void Add(IView view)
		{
			_views.Add(view);
			AddSubview(view.ToNative());
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			var bounds = Bounds;
			nfloat lastY = Superview.SafeAreaInsets.Top;
			CGRect frame;
			var width = Superview.Bounds.Width;
			nfloat maxWidth = width;

			foreach (var subView in Subviews)
			{
				//TODO: Tests hook into the renders size!
				subView.SizeToFit();

				frame = subView.Bounds;
				maxWidth = NMath.Max(maxWidth, frame.Width);

				frame.Y = lastY;
				frame.X = 0;

				frame.Width = maxWidth;

				lastY = frame.Bottom + Spacing;
				subView.Frame = frame;
				Console.WriteLine(frame);
			}

			bounds.Height = lastY;
			bounds.Width = maxWidth;

			Frame = bounds;

			if (Superview is UIScrollView scroll)
				scroll.ContentSize = bounds.Size;
		}
	}
}
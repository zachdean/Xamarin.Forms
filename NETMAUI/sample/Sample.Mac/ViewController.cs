using System;
using AppKit;
using Foundation;

namespace Sample.Mac
{
	public partial class ViewController : NSViewController
	{
		public ViewController(IntPtr handle) : base(handle)
		{
			var app = new MyApp();

			var rootView = new RootView();

			rootView.Add(app.TestActivityIndicator);
			rootView.Add(app.TestButton);
			rootView.Add(app.TestEllipse);
			rootView.Add(app.TestEntry);
			rootView.Add(app.TestLabel);
			rootView.Add(app.TestLine);
			rootView.Add(app.TestLine);
			rootView.Add(app.TestPath);
			rootView.Add(app.TestPolygon);
			rootView.Add(app.TestPolyline);
			rootView.Add(app.TestRectangle);
			rootView.Add(app.TestSlider);
			rootView.Add(app.TestStepper);
			rootView.Add(app.TestSwitch);

			View = rootView;
		}

		public override NSObject RepresentedObject
		{
			get
			{
				return base.RepresentedObject;
			}
			set
			{
				base.RepresentedObject = value;
				// Update the view, if already loaded.
			}
		}
	}
}

using System;
using AppKit;
using Foundation;
using Xamarin.Forms;
using Xamarin.Platform;

namespace Sample.Mac
{
	public partial class ViewController : NSViewController
	{
		public ViewController(IntPtr handle) : base(handle)
		{
			var app = new MyApp();
			View = (app.MainPage as ContentPage).Content.ToNative();
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
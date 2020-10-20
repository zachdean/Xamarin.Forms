using System.ComponentModel;
using System.Drawing;
using AppKit;
using CoreImage;
using System.Linq;
using Xamarin.Platform;

namespace Xamarin.Forms.Platform.MacOS
{
	public class ActivityIndicatorRenderer : ViewRenderer<ActivityIndicator, NSProgressIndicator>
	{
		static CIColorPolynomial s_currentColorFilter;
		static NSColor s_currentColor;

		[PortHandler]
		protected override void OnElementChanged(ElementChangedEventArgs<ActivityIndicator> e)
		{
			if (e.NewElement != null)
			{
				if (Control == null)
					SetNativeControl(new NSProgressIndicator(RectangleF.Empty) { Style = NSProgressIndicatorStyle.Spinning });

				UpdateColor();
				UpdateIsRunning();
			}

			base.OnElementChanged(e);
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == ActivityIndicator.ColorProperty.PropertyName)
				UpdateColor();
			else if (e.PropertyName == ActivityIndicator.IsRunningProperty.PropertyName)
				UpdateIsRunning();
		}

		[PortHandler]
		void UpdateColor()
		{
			var color = Element.Color;
			if (s_currentColorFilter == null && color.IsDefault)
				return;

			if (color.IsDefault)
				Control.ContentFilters = new CIFilter[0];

			var newColor = Element.Color.ToNSColor();
			if (Equals(s_currentColor, newColor))
			{
				if(Control.ContentFilters?.FirstOrDefault() != s_currentColorFilter)
				{
					Control.ContentFilters = new CIFilter[] { s_currentColorFilter };
				}
				return;
			}

			s_currentColor = newColor;

			s_currentColorFilter = new CIColorPolynomial
			{
				RedCoefficients = new CIVector(s_currentColor.RedComponent),
				BlueCoefficients = new CIVector(s_currentColor.BlueComponent),
				GreenCoefficients = new CIVector(s_currentColor.GreenComponent)
			};

			Control.ContentFilters = new CIFilter[] { s_currentColorFilter };
		}

		[PortHandler]
		void UpdateIsRunning()
		{
			if (Element.IsRunning)
				Control.StartAnimation(this);
			else
				Control.StopAnimation(this);
		}
	}
}
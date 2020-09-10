using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Platform;

namespace Xamarin.Forms
{
	public partial class Button : IButton
	{		
		public IViewRenderer Renderer { get; set; }

		TextType IText.TextType => throw new NotImplementedException();

		Color IText.Color => throw new NotImplementedException();

		bool IFrameworkElement.IsEnabled => throw new NotImplementedException();

		Color IFrameworkElement.BackgroundColor => throw new NotImplementedException();

		Rectangle IFrameworkElement.Frame => throw new NotImplementedException();

		IFrameworkElement IFrameworkElement.Parent => throw new NotImplementedException();

		SizeRequest IFrameworkElement.DesiredSize => throw new NotImplementedException();

		bool IFrameworkElement.IsMeasureValid => throw new NotImplementedException();

		bool IFrameworkElement.IsArrangeValid => throw new NotImplementedException();

		void IFrameworkElement.Arrange(Rectangle bounds)
		{
			throw new NotImplementedException();
		}

		void IButton.Clicked()
		{
			throw new NotImplementedException();
		}

		void IFrameworkElement.InvalidateArrange()
		{
			throw new NotImplementedException();
		}

		void IFrameworkElement.InvalidateMeasure()
		{
			throw new NotImplementedException();
		}

		SizeRequest IFrameworkElement.Measure(double widthConstraint, double heightConstraint)
		{
			throw new NotImplementedException();
		}
	}
}

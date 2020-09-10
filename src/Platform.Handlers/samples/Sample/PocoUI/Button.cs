using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Platform;

namespace Sample.PocoUI
{
	public class Button : IButton
	{
		public string Text { get; set; }

		public TextType TextType => throw new NotImplementedException();

		public Color Color => throw new NotImplementedException();

		public bool IsEnabled => throw new NotImplementedException();

		public Color BackgroundColor => throw new NotImplementedException();

		public Rectangle Frame => throw new NotImplementedException();

		public IViewRenderer Renderer { get; set; }

		public IFrameworkElement Parent => throw new NotImplementedException();

		public SizeRequest DesiredSize => throw new NotImplementedException();

		public bool IsMeasureValid => throw new NotImplementedException();

		public bool IsArrangeValid => throw new NotImplementedException();

		public void Arrange(Rectangle bounds)
		{
			throw new NotImplementedException();
		}

		public void Clicked()
		{
			throw new NotImplementedException();
		}

		public void InvalidateArrange()
		{
			throw new NotImplementedException();
		}

		public void InvalidateMeasure()
		{
			throw new NotImplementedException();
		}

		public SizeRequest Measure(double widthConstraint, double heightConstraint)
		{
			throw new NotImplementedException();
		}
	}
}

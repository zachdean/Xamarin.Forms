using System;
using System.Linq;
using System.Maui;
using System.Maui.Core.Layout;

namespace Xamarin.Forms
{
	public class DummyStackLayout : DummyLayout, IStackLayout
	{
		SizeRequest IFrameworkElement.Measure(double widthConstraint, double heightConstraint)
		{
			if (IsMeasureValid)
			{
				return DesiredSize;
			}

			DesiredSize = Stack.MeasureStack(widthConstraint, heightConstraint, Children, Orientation);

			IsMeasureValid = true;

			return DesiredSize;
		}

		void IFrameworkElement.Arrange(Rectangle bounds)
		{
			if (IsArrangeValid)
			{
				return;
			}

			Stack.ArrangeStack(bounds, Children, Orientation);
			IsArrangeValid = true;
			Renderer.SetFrame(bounds);
		}

		bool isMeasureValid;
		public bool IsMeasureValid
		{
			get
			{
				return isMeasureValid
					&& Children.All(child => child.IsMeasureValid);
			}

			protected set
			{
				isMeasureValid = value;
			}
		}

		void IFrameworkElement.InvalidateMeasure()
		{
			isMeasureValid = false;
			IsArrangeValid = false;
		}

		public Orientation Orientation { get; set; } = Orientation.Vertical;
		public SizeRequest DesiredSize { get; private set; }
		public bool IsArrangeValid { get; private set; }
	}
}

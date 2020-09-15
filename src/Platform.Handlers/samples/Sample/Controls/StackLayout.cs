using System.Linq;
using Xamarin.Forms;
using Xamarin.Platform;

namespace Sample
{
	public class StackLayout : Layout, IStackLayout
	{
		bool _isMeasureValid;

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
			Handler.SetFrame(bounds);
		}

		public bool IsMeasureValid
		{
			get
			{
				return _isMeasureValid
					&& Children.All(child => child.IsMeasureValid);
			}

			protected set
			{
				_isMeasureValid = value;
			}
		}

		void IFrameworkElement.InvalidateMeasure()
		{
			_isMeasureValid = false;
			IsArrangeValid = false;
		}

		public Orientation Orientation { get; set; } = Orientation.Vertical;
		public SizeRequest DesiredSize { get; private set; }
		public bool IsArrangeValid { get; private set; }
	}
}

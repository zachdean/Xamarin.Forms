using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Xamarin.Platform.Layouts
{
	public class GridLayoutManager : LayoutManager
	{
		public GridLayoutManager(IGridLayout layout) : base(layout)
		{
			Grid = layout;
		}

		public IGridLayout Grid { get; }

		public override void ArrangeChildren(Rectangle childBounds) => Arrange(childBounds, Grid.Children);
		
		// TODO ezhart Include row/col spacing
		public override Size Measure(double widthConstraint, double heightConstraint) => Measure(widthConstraint, heightConstraint, Grid.Children);

		static void Arrange(Rectangle childBounds, IReadOnlyList<IView> views)
		{
			foreach (var view in views)
			{
				// Just shoving them all in the same place for the moment
				view.Arrange(childBounds);
			}
		}

		static Size Measure(double widthConstraint, double heightConstraint, IReadOnlyList<IView> views)
		{
			var size = Size.Zero;

			// This is obviously wrong, just wanted to a get something on screen
			foreach (var view in views)
			{
				size = view.Measure(widthConstraint, heightConstraint);
			}

			return size;
		}
	}
}

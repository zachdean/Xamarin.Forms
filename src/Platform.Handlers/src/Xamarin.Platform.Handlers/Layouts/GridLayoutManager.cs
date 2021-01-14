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

		public override void Arrange(Rectangle bounds) => Arrange(bounds, Grid.Children);
		
		// TODO ezhart Include row/col spacing
		public override Size Measure(double widthConstraint, double heightConstraint) => Measure(widthConstraint, heightConstraint, Grid.Children);


		static void Arrange(Rectangle bounds, IReadOnlyList<IView> views) 
		{ 
		
		}

		static Size Measure(double widthConstraint, double heightConstraint, IReadOnlyList<IView> views)
		{
			return Size.Zero;
		}
	}
}

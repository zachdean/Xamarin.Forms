using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using Xamarin.Forms;
using Xamarin.Platform.Handlers.Tests;
using Xamarin.Platform.Layouts;

namespace Xamarin.Platform.Handlers.UnitTests.Layouts
{
	[TestFixture(Category = TestCategory.Layout)]
	public class GridLayoutManagerTests 
	{
		IGridLayout CreateTestGridAllAuto(int rows = 1, int columns = 1) 
		{
			var grid = Substitute.For<IGridLayout>();

			grid.Height.Returns(-1);
			grid.Width.Returns(-1);
			grid.ColumnSpacing.Returns(0);
			grid.RowSpacing.Returns(0);

			var rowDefs = new List<IGridRowDefinition>();

			for (int row = 0; row < rows; row++)
			{
				var rowDef = Substitute.For<IGridRowDefinition>();
				rowDef.Height.Returns(Forms.GridLength.Auto);
				rowDefs.Add(rowDef);
			}

			grid.RowDefinitions.Returns(rowDefs);

			var colDefs = new List<IGridColumnDefinition>();

			for (int col = 0; col < columns; col++)
			{
				var colDef = Substitute.For<IGridColumnDefinition>();
				colDef.Width.Returns(Forms.GridLength.Auto);
				colDefs.Add(colDef);
			}

			grid.ColumnDefinitions.Returns(colDefs);

			return grid;
		}

		[Test]
		public void SingleItem() 
		{
			// A one-row, one-column grid
			var grid = CreateTestGridAllAuto();

			// A 100x100 IView
			var view = LayoutTestHelpers.CreateTestView(new Size(100, 100));

			// Set up the grid to have a single child
			LayoutTestHelpers.SubstituteChildren(grid, view);

			// Set up the row/column values and spans
			grid.GetRow(view).Returns(0);
			grid.GetRowSpan(view).Returns(1);
			grid.GetColumn(view).Returns(0);
			grid.GetColumnSpan(view).Returns(1);

			var manager = new GridLayoutManager(grid);

			// Assuming no constraints on space
			var measuredSize = manager.Measure(double.PositiveInfinity, double.PositiveInfinity);
			manager.Arrange(new Rectangle(Point.Zero, measuredSize));

			// We expect that the only child of the grid will be given its full size
			var expectedRectangle = new Rectangle(0, 0, 100, 100);
			grid.Children[0].Received().Arrange(Arg.Is(expectedRectangle));
		}
	}
}

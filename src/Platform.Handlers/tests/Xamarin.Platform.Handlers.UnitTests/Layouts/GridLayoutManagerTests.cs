using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using Xamarin.Forms;
using Xamarin.Platform.Handlers.Tests;
using Xamarin.Platform.Layouts;
using static Xamarin.Platform.Handlers.UnitTests.Layouts.LayoutTestHelpers;

namespace Xamarin.Platform.Handlers.UnitTests.Layouts
{
	[TestFixture(Category = TestCategory.Layout)]
	public class GridLayoutManagerTests 
	{
		// TODO ezhart Include row/col spacing tests

		// TODO ezhart Need a test to assert that auto rows/cols with no controls in them get 0
		// TOOD ezhart What happens if There is row/col spacing and an auto with no controls is in the middle? is the spacing around it included (2 spacings), or just one spacing? (I think just one is least astonishing)

		IGridLayout CreateGridLayout(int rowSpacing = 0, int colSpacing = 0,
			IEnumerable<IGridRowDefinition> rows = null, IEnumerable < IGridColumnDefinition> columns = null)
		{
			var grid = Substitute.For<IGridLayout>();

			grid.RowSpacing.Returns(rowSpacing);
			grid.ColumnSpacing.Returns(colSpacing);

			SubRowDefs(grid, rows);
			SubColDefs(grid, columns);

			return grid;
		}

		void SubRowDefs(IGridLayout grid, IEnumerable<IGridRowDefinition> rows = null) 
		{
			if (rows == null)
			{
				var rowDef = Substitute.For<IGridRowDefinition>();
				rowDef.Height.Returns(GridLength.Auto);
				var rowDefs = new List<IGridRowDefinition>
				{
					rowDef
				};
				grid.RowDefinitions.Returns(rowDefs);
			}
			else
			{
				grid.RowDefinitions.Returns(rows);
			}
		}

		void SubColDefs(IGridLayout grid, IEnumerable<IGridColumnDefinition> cols = null)
		{
			if (cols == null)
			{
				var colDefs = CreateTestColumns("auto");
				grid.ColumnDefinitions.Returns(colDefs);
			}
			else
			{
				grid.ColumnDefinitions.Returns(cols);
			}
		}

		List<IGridColumnDefinition> CreateTestColumns(params string[] columnWidths) 
		{
			var converter = new GridLengthTypeConverter();

			var colDefs = new List<IGridColumnDefinition>();

			foreach (var width in columnWidths)
			{
				var gridLength = converter.ConvertFromInvariantString(width);
				var colDef = Substitute.For<IGridColumnDefinition>();
				colDef.Width.Returns(gridLength);
				colDefs.Add(colDef);
			}

			return colDefs;
		}

		List<IGridRowDefinition> CreateTestRows(params string[] rowHeights)
		{
			var converter = new GridLengthTypeConverter();

			var rowDefs = new List<IGridRowDefinition>();

			foreach (var height in rowHeights)
			{
				var gridLength = converter.ConvertFromInvariantString(height);
				var rowDef = Substitute.For<IGridRowDefinition>();
				rowDef.Height.Returns(gridLength);
				rowDefs.Add(rowDef);
			}

			return rowDefs;
		}

		void SetLocation(IGridLayout grid, IView view, int row = 0, int col = 0, int rowSpan = 1, int colSpan = 1) 
		{
			grid.GetRow(view).Returns(row);
			grid.GetRowSpan(view).Returns(rowSpan);
			grid.GetColumn(view).Returns(col);
			grid.GetColumnSpan(view).Returns(colSpan);
		}

		void AssertArranged(IView view, double x, double y, double width, double height)
		{
			var expected = new Rectangle(x, y, width, height);
			view.Received().Arrange(Arg.Is(expected));
		}

		[Test]
		public void SingleItem() 
		{
			// A one-row, one-column grid
			var grid = CreateGridLayout();

			// A 100x100 IView
			var view = CreateTestView(new Size(100, 100));

			// Set up the grid to have a single child
			AddChildren(grid, view);

			// Set up the row/column values and spans
			grid.GetRow(view).Returns(0);
			grid.GetRowSpan(view).Returns(1);
			grid.GetColumn(view).Returns(0);
			grid.GetColumnSpan(view).Returns(1);

			var manager = new GridLayoutManager(grid);

			// Assuming no constraints on space
			var measuredSize = manager.Measure(double.PositiveInfinity, double.PositiveInfinity);
			manager.ArrangeChildren(new Rectangle(Point.Zero, measuredSize));

			// We expect that the only child of the grid will be given its full size
			var expectedRectangle = new Rectangle(0, 0, 100, 100);
			grid.Children[0].Received().Arrange(Arg.Is(expectedRectangle));
		}

		[Test]
		public void TwoColumnsOneRowAbsolute()
		{
			var colDefs = CreateTestColumns("100", "100");
			var rowDefs = CreateTestRows("10");
			var grid = CreateGridLayout(columns: colDefs, rows: rowDefs);

			var viewSize = new Size(10, 10);

			var view0 = CreateTestView(viewSize);
			var view1 = CreateTestView(viewSize);

			AddChildren(grid, view0, view1);

			SetLocation(grid, view0);
			SetLocation(grid, view1, col: 1);

			var manager = new GridLayoutManager(grid);

			// Assuming no constraints on space
			var measuredSize = manager.Measure(double.PositiveInfinity, double.PositiveInfinity);
			manager.ArrangeChildren(new Rectangle(Point.Zero, measuredSize));

			// Column width is 100, so despite the view being 10 wide, we expect it to be arranged with width 100
			AssertArranged(view0, 0, 0, 100, viewSize.Height);

			// Since the first column is 100 wide, we expect the view in the second column to start at x = 100
			AssertArranged(view1, 100, 0, 100, viewSize.Height);
		}

		[Test]
		public void TwoColumnsTwoRowsAbsolute()
		{
			var colDefs = CreateTestColumns("100", "100");
			var rowDefs = CreateTestRows("10", "30");
			var grid = CreateGridLayout(columns: colDefs, rows: rowDefs);

			var viewSize = new Size(10, 10);

			var view0 = CreateTestView(viewSize);
			var view1 = CreateTestView(viewSize);
			var view2 = CreateTestView(viewSize);
			var view3 = CreateTestView(viewSize);

			AddChildren(grid, view0, view1, view2, view3);

			SetLocation(grid, view0);
			SetLocation(grid, view1, col: 1);
			SetLocation(grid, view2, row: 1);
			SetLocation(grid, view3, row: 1, col: 1);

			var manager = new GridLayoutManager(grid);

			// Assuming no constraints on space
			var measuredSize = manager.Measure(double.PositiveInfinity, double.PositiveInfinity);
			manager.ArrangeChildren(new Rectangle(Point.Zero, measuredSize));

			// Column width is 100, so despite the view being 10 wide, we expect it to be arranged with width 100
			AssertArranged(view0, 0, 0, 100, 10);

			// Since the first column is 100 wide, we expect the view in the second column to start at x = 100
			AssertArranged(view1, 100, 0, 100, 10);

			// First column, width 100, second row, height 30 and y 10
			AssertArranged(view2, 0, 10, 100, 30);

			// Second column, width 100 and x 100, second row, height 30
			AssertArranged(view3, 100, 10, 100, 30);
		}

		[Test]
		public void TwoColumnsAbsolute()
		{
			var colDefs = CreateTestColumns("100", "100");
			var grid = CreateGridLayout(columns: colDefs);

			var viewSize = new Size(10, 10);

			var view0 = CreateTestView(viewSize);
			var view1 = CreateTestView(viewSize);

			AddChildren(grid, view0, view1);

			SetLocation(grid, view0);
			SetLocation(grid, view1, col: 1);

			var manager = new GridLayoutManager(grid);

			// Assuming no constraints on space
			var measuredSize = manager.Measure(double.PositiveInfinity, double.PositiveInfinity);
			manager.ArrangeChildren(new Rectangle(Point.Zero, measuredSize));

			// Column width is 100, so despite the view being 10 wide, we expect it to be arranged with width 100
			AssertArranged(view0, 0, 0, 100, viewSize.Height);

			// Since the first column is 100 wide, we expect the view in the second column to start at x = 100
			AssertArranged(view1, 100, 0, 100, viewSize.Height);
		}

		[Test]
		public void TwoRowsAbsolute()
		{
			var rowDefs = CreateTestRows("100", "100");
			var grid = CreateGridLayout(rows: rowDefs);

			var viewSize = new Size(10, 10);

			var view0 = CreateTestView(viewSize);
			var view1 = CreateTestView(viewSize);

			AddChildren(grid, view0, view1);

			SetLocation(grid, view0);
			SetLocation(grid, view1, row: 1);

			var manager = new GridLayoutManager(grid);

			// Assuming no constraints on space
			var measuredSize = manager.Measure(double.PositiveInfinity, double.PositiveInfinity);
			manager.ArrangeChildren(new Rectangle(Point.Zero, measuredSize));

			// Row height is 100, so despite the view being 10 tall, we expect it to be arranged with height 100
			AssertArranged(view0, 0, 0, viewSize.Width, 100);

			// Since the first row is 100 tall, we expect the view in the second row to start at y = 100
			var expectedRectangle1 = new Rectangle(0, 100, viewSize.Width, 100);
			AssertArranged(view1, 0, 100, viewSize.Width, 100);
		}
	}
}

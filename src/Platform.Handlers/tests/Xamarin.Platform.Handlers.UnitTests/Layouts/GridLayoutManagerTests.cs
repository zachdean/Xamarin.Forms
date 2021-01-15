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
				var rowDefs = new List<IGridRowDefinition>();
				rowDefs.Add(rowDef);
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
				var colDefs = CreateTestColums("auto");
				grid.ColumnDefinitions.Returns(colDefs);
			}
			else
			{
				grid.ColumnDefinitions.Returns(cols);
			}
		}

		List<IGridColumnDefinition> CreateTestColums(params string[] columnWidths) 
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
		public void TwoColumnsAbsolute()
		{
			var colDefs = CreateTestColums("100", "100");
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
			var expectedRectangle0 = new Rectangle(0, 0, 100, viewSize.Height);

			// Since the first column is 100 wide, we expect the view in the second column to start at x = 100
			var expectedRectangle1 = new Rectangle(100, 0, 100, viewSize.Height);

			grid.Children[0].Received().Arrange(Arg.Is(expectedRectangle0));
			grid.Children[1].Received().Arrange(Arg.Is(expectedRectangle1));
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
			var expectedRectangle0 = new Rectangle(0, 0, viewSize.Width, 100);

			// Since the first row is 100 tall, we expect the view in the second row to start at y = 100
			var expectedRectangle1 = new Rectangle(0, 100, viewSize.Width, 100);

			grid.Children[0].Received().Arrange(Arg.Is(expectedRectangle0));
			grid.Children[1].Received().Arrange(Arg.Is(expectedRectangle1));
		}
	}
}

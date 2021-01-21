using System;
using System.Collections.Generic;
using System.Linq;
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

		public override Size Measure(double widthConstraint, double heightConstraint) 
		{
			var structure = new GridStructure(Grid, widthConstraint, heightConstraint);

			return new Size(structure.GetWidth(), structure.GetHeight());
		}

		public override void ArrangeChildren(Rectangle childBounds) 
		{
			var structure = new GridStructure(Grid, childBounds.Width, childBounds.Height);

			foreach (var view in Grid.Children)
			{
				var row = Grid.GetRow(view);
				var col = Grid.GetColumn(view);

				double top = structure.GetTopEdgeOfRow(row);
				double left = structure.GetLeftEdgeOfColumn(col);
				double width = structure.Columns[col].ActualWidth;
				double height = structure.Rows[row].ActualHeight;

				view.Arrange(new Rectangle(left, top, width, height));
			}
		}

		class GridStructure 
		{
			readonly IGridLayout _grid;
			readonly double _gridWidthConstraint;
			readonly double _gridHeightConstraint;

			public Row[] Rows { get; }
			public Column[] Columns { get; }

			public GridStructure(IGridLayout grid, double widthConstraint, double heightConstraint) 
			{
				_grid = grid;
				_gridWidthConstraint = widthConstraint;
				_gridHeightConstraint = heightConstraint;
				Rows = new Row[_grid.RowDefinitions.Count];

				for (int n = 0; n < _grid.RowDefinitions.Count; n++)
				{
					Rows[n] = new Row(_grid.RowDefinitions[n]);
				}

				Columns = new Column[_grid.ColumnDefinitions.Count];

				for (int n = 0; n < _grid.ColumnDefinitions.Count; n++)
				{
					Columns[n] = new Column(_grid.ColumnDefinitions[n]);
				}

				CalculateAutoRowHeights();
				CalculateAutoColumnWidths();
			}

			public void CalculateAutoRowHeights() 
			{
				for (int rowIndex = 0; rowIndex < Rows.Length; rowIndex++)
				{
					var row = Rows[rowIndex];
					if (row.IsMeasured || row.IsStar)
					{
						continue;
					}

					var availableWidth = _gridWidthConstraint - GetWidth();
					var availableHeight = _gridHeightConstraint - GetHeight();

					var rowHeight = CalculateAutoRowHeight(rowIndex, availableWidth, availableHeight);
					Rows[rowIndex].ActualHeight = rowHeight;
				}
			}

			public void CalculateAutoColumnWidths()
			{
				for (int columnIndex = 0; columnIndex < Columns.Length; columnIndex++)
				{
					var column = Columns[columnIndex];
					if (column.IsMeasured || column.IsStar)
					{
						continue;
					}

					var availableWidth = _gridWidthConstraint - GetWidth();
					var availableHeight = _gridHeightConstraint - GetHeight();

					var columnWidth = CalculateAutoColumnWidth(columnIndex, availableWidth, availableHeight);
					Columns[columnIndex].ActualWidth = columnWidth;
				}
			}

			public double GetHeight() 
			{
				double height = 0;

				for (int n = 0; n < Rows.Length; n++)
				{
					var rowHeight = Rows[n].ActualHeight;
					if (rowHeight == -1)
					{
						continue;
					}
					height += rowHeight;
				}

				return height;
			}

			public double GetWidth() 
			{
				double width = 0;

				for (int n = 0; n < Columns.Length; n++)
				{
					var colWidth = Columns[n].ActualWidth;
					if (colWidth == -1)
					{
						continue;
					}
					width += colWidth;
				}

				return width;
			}

			public double GetLeftEdgeOfColumn(int column) 
			{
				double left = 0;

				for (int n = 0; n < column; n++)
				{
					left += Columns[n].ActualWidth;
				}

				return left;
			}

			public double GetTopEdgeOfRow(int row) 
			{
				double top = 0;

				for (int n = 0; n < row; n++)
				{
					top += Rows[n].ActualHeight;
				}

				return top;
			}

			public double CalculateAutoRowHeight(int row, double availableWidth, double availableHeight) 
			{
				double height = 0;
				foreach (var view in _grid.Children)
				{
					if (_grid.GetRow(view) != row)
					{
						continue;
					}

					height = Math.Max(height, view.Measure(availableWidth, availableHeight).Height);
				}

				return height;
			}

			public double CalculateAutoColumnWidth(int column, double availableWidth, double availableHeight)
			{
				double width = 0;
				foreach (var view in _grid.Children)
				{
					if (_grid.GetColumn(view) != column)
					{
						continue;
					}

					width = Math.Max(width, view.Measure(availableWidth, availableHeight).Width);
				}

				return width;
			}
		}

		class Column 
		{
			public IGridColumnDefinition ColumnDefinition { get; set; }

			public double ActualWidth { get; set; } = -1;

			public Column(IGridColumnDefinition columnDefinition)
			{
				ColumnDefinition = columnDefinition;
				if (columnDefinition.Width.IsAbsolute)
				{
					ActualWidth = columnDefinition.Width.Value;
				}
			}

			public bool IsMeasured => ActualWidth > -1;

			public bool IsStar => ColumnDefinition.Width.IsStar;

			public bool IsAuto => ColumnDefinition.Width.IsAuto;
		}

		class Row 
		{
			public IGridRowDefinition RowDefinition { get; set; }

			public double ActualHeight { get; set; } = -1;

			public Row(IGridRowDefinition rowDefinition) 
			{ 
				RowDefinition = rowDefinition;
				if (rowDefinition.Height.IsAbsolute)
				{
					ActualHeight = rowDefinition.Height.Value;
				}
			}

			public bool IsMeasured => ActualHeight > -1;

			public bool IsStar => RowDefinition.Height.IsStar;

			public bool IsAuto => RowDefinition.Height.IsAuto;
		}
	}
}

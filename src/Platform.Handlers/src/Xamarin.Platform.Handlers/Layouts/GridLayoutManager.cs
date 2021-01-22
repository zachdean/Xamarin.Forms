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
				view.Arrange(structure.GetTargetFrame(view));
			}
		}

		class GridStructure 
		{
			readonly IGridLayout _grid;
			readonly double _gridWidthConstraint;
			readonly double _gridHeightConstraint;

			Row[] _rows { get; }
			Column[] _columns { get; }

			public GridStructure(IGridLayout grid, double widthConstraint, double heightConstraint) 
			{
				_grid = grid;
				_gridWidthConstraint = widthConstraint;
				_gridHeightConstraint = heightConstraint;
				_rows = new Row[_grid.RowDefinitions.Count];

				for (int n = 0; n < _grid.RowDefinitions.Count; n++)
				{
					_rows[n] = new Row(_grid.RowDefinitions[n]);
				}

				_columns = new Column[_grid.ColumnDefinitions.Count];

				for (int n = 0; n < _grid.ColumnDefinitions.Count; n++)
				{
					_columns[n] = new Column(_grid.ColumnDefinitions[n]);
				}

				CalculateAutoRowHeights();
				CalculateAutoColumnWidths();
			}

			public Rectangle GetTargetFrame(IView view) 
			{
				var row = _grid.GetRow(view);
				var column = _grid.GetColumn(view);

				double top = GetTopEdgeOfRow(row);
				double left = GetLeftEdgeOfColumn(column);
				double width = _columns[column].ActualWidth;
				double height = _rows[row].ActualHeight;

				return new Rectangle(left, top, width, height);
			}

			public double GetHeight()
			{
				double height = 0;

				for (int n = 0; n < _rows.Length; n++)
				{
					var rowHeight = _rows[n].ActualHeight;
					if (rowHeight == -1)
					{
						continue;
					}
					height += rowHeight;
				}

				if (_rows.Length > 1)
				{
					height += (_rows.Length - 1) * _grid.RowSpacing;
				}

				return height;
			}

			public double GetWidth()
			{
				double width = 0;

				for (int n = 0; n < _columns.Length; n++)
				{
					var colWidth = _columns[n].ActualWidth;
					if (colWidth == -1)
					{
						continue;
					}
					width += colWidth;
				}

				if (_columns.Length > 1)
				{
					width += (_columns.Length - 1) * _grid.ColumnSpacing;
				}

				return width;
			}

			void CalculateAutoRowHeights() 
			{
				for (int rowIndex = 0; rowIndex < _rows.Length; rowIndex++)
				{
					var row = _rows[rowIndex];
					if (row.IsMeasured || row.IsStar)
					{
						continue;
					}

					var availableWidth = _gridWidthConstraint - GetWidth();
					var availableHeight = _gridHeightConstraint - GetHeight();

					var rowHeight = CalculateAutoRowHeight(rowIndex, availableWidth, availableHeight);
					_rows[rowIndex].ActualHeight = rowHeight;
				}
			}

			void CalculateAutoColumnWidths()
			{
				for (int columnIndex = 0; columnIndex < _columns.Length; columnIndex++)
				{
					var column = _columns[columnIndex];
					if (column.IsMeasured || column.IsStar)
					{
						continue;
					}

					var availableWidth = _gridWidthConstraint - GetWidth();
					var availableHeight = _gridHeightConstraint - GetHeight();

					var columnWidth = CalculateAutoColumnWidth(columnIndex, availableWidth, availableHeight);
					_columns[columnIndex].ActualWidth = columnWidth;
				}
			}

			double CalculateAutoRowHeight(int row, double availableWidth, double availableHeight) 
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

			double CalculateAutoColumnWidth(int column, double availableWidth, double availableHeight)
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

			double GetLeftEdgeOfColumn(int column)
			{
				double left = 0;

				for (int n = 0; n < column; n++)
				{
					left += _columns[n].ActualWidth;
					left += _grid.ColumnSpacing;
				}

				return left;
			}

			double GetTopEdgeOfRow(int row)
			{
				double top = 0;

				for (int n = 0; n < row; n++)
				{
					top += _rows[n].ActualHeight;
					top += _grid.RowSpacing;
				}

				return top;
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

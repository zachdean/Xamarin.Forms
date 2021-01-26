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

			return new Size(structure.GridWidth(), structure.GridHeight());
		}

		public override void ArrangeChildren(Rectangle childBounds) 
		{
			var structure = new GridStructure(Grid, childBounds.Width, childBounds.Height);

			foreach (var view in Grid.Children)
			{
				var cell = structure.ComputeFrameFor(view);

				// This is basically LayoutOptions.Start as a default
				var destination = new Rectangle(cell.X, cell.Y, view.DesiredSize.Width, view.DesiredSize.Height);
				view.Arrange(destination);
			}
		}

		class GridStructure 
		{
			readonly IGridLayout _grid;
			readonly double _gridWidthConstraint;
			readonly double _gridHeightConstraint;

			Row[] _rows { get; }
			Column[] _columns { get; }
			Cell[] _cells { get; }

			readonly Dictionary<SpanKey, Span> _spans = new Dictionary<SpanKey, Span>();

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

				_cells = new Cell[_grid.Children.Count];

				InitializeCells();

				MeasureCells();
			}

			void InitializeCells() 
			{
				for (int n = 0; n < _grid.Children.Count; n++)
				{
					var view = _grid.Children[n];
					var column = _grid.GetColumn(view);
					var columnSpan = _grid.GetColumnSpan(view);

					var columnGridLengthType = GridLengthType.None;

					for (int columnIndex = column; columnIndex < column + columnSpan; columnIndex++)
					{
						columnGridLengthType |= ToGridLengthType(_columns[columnIndex].ColumnDefinition.Width.GridUnitType);
					}

					var row = _grid.GetRow(view);
					var rowSpan = _grid.GetRowSpan(view);

					var rowGridLengthType = GridLengthType.None;

					for (int rowIndex = column; rowIndex < row + rowSpan; rowIndex++)
					{
						rowGridLengthType |= ToGridLengthType(_rows[rowIndex].RowDefinition.Height.GridUnitType);
					}

					_cells[n] = new Cell(n, row, column, rowSpan, columnSpan, columnGridLengthType, rowGridLengthType);
				}
			}

			public Rectangle ComputeFrameFor(IView view) 
			{
				var rowStart = _grid.GetRow(view);
				var column = _grid.GetColumn(view);

				var rowEnd = rowStart + _grid.GetRowSpan(view);

				double top = TopEdgeOfRow(rowStart);
				double left = LeftEdgeOfColumn(column);

				double width = _columns[column].ActualWidth;

				double height = 0;

				for (int n = rowStart; n < rowEnd; n++)
				{ 
					height += _rows[n].ActualHeight;
				}

				return new Rectangle(left, top, width, height);
			}

			public double GridHeight()
			{
				double height = 0;

				for (int n = 0; n < _rows.Length; n++)
				{
					var rowHeight = _rows[n].ActualHeight;
					
					if (rowHeight <= 0)
					{
						continue;
					}
					
					height += rowHeight;
					
					if (n > 0)
					{
						height += _grid.RowSpacing;
					}
				}

				return height;
			}

			public double GridWidth()
			{
				double width = 0;

				for (int n = 0; n < _columns.Length; n++)
				{
					var colWidth = _columns[n].ActualWidth;
					if (colWidth <= 0)
					{
						continue;
					}

					width += colWidth;

					if (n > 0)
					{
						width += _grid.ColumnSpacing;
					}
				}

				return width;
			}

			void MeasureCells() 
			{
				for (int n = 0; n < _cells.Length; n++)
				{
					var cell = _cells[n];

					if (cell.ColumnGridLengthType == GridLengthType.Absolute
						&& cell.RowGridLengthType == GridLengthType.Absolute)
					{
						continue;
					}

					var availableWidth = _gridWidthConstraint - GridWidth();
					var availableHeight = _gridHeightConstraint - GridHeight();

					var measure = _grid.Children[cell.ViewIndex].Measure(availableWidth, availableHeight);

					if (cell.ColumnGridLengthType == GridLengthType.Auto)
					{
						if (cell.ColumnSpan == 1)
						{
							_columns[cell.Column].Update(measure.Width);
						}
						else
						{
							var span = new Span(cell.Column, cell.ColumnSpan, true, measure.Width);
							TrackSpan(span);
						}
					}

					if (cell.RowGridLengthType == GridLengthType.Auto)
					{
						if (cell.RowSpan == 1)
						{
							_rows[cell.Row].Update(measure.Height);
						}
						else
						{
							var span = new Span(cell.Row, cell.RowSpan, false, measure.Height);
							TrackSpan(span);
						}
					}
				}

				ResolveSpans();
			}

			void TrackSpan(Span span) 
			{
				if (_spans.TryGetValue(span.Key, out Span otherSpan))
				{
					// This span may replace an equivalent but smaller span
					if (span.Requested > otherSpan.Requested)
					{
						_spans[span.Key] = span;
					}
				}
				else
				{
					_spans[span.Key] = span;
				}
			}

			void ResolveSpans() 
			{
				foreach (var span in _spans.Values)
				{
					if (span.IsColumn)
					{
						ResolveColumnSpan(span.Start, span.Length, span.Requested);
					}
					else
					{
						ResolveRowSpan(span.Start, span.Length, span.Requested);
					}
				}
			}

			void ResolveColumnSpan(int start, int length, double requested)
			{
				double currentSize = 0;
				var end = start + length;

				for (int n = start; n < end; n++)
				{
					currentSize += _columns[n].ActualWidth;
				}

				if (requested <= currentSize)
				{
					return;
				}

				double required = requested - currentSize;

				for (int n = start; n < end; n++)
				{
					_columns[n].ActualWidth += (required / length);
				}
			}

			void ResolveRowSpan(int start, int length, double requested) 
			{
				double currentSize = 0;
				var end = start + length;

				for (int n = start; n < end; n++)
				{
					currentSize += _rows[n].ActualHeight;
				}

				if (requested <= currentSize)
				{
					return;
				}

				double required = requested - currentSize;

				for (int n = start; n < end; n++)
				{
					_rows[n].ActualHeight += (required / length);
				}
			}

			double LeftEdgeOfColumn(int column)
			{
				double left = 0;

				for (int n = 0; n < column; n++)
				{
					left += _columns[n].ActualWidth;
					left += _grid.ColumnSpacing;
				}

				return left;
			}

			double TopEdgeOfRow(int row)
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

		class Span
		{
			public int Start { get; }
			public int Length { get; }
			public bool IsColumn { get; }
			public double Requested { get; }

			public SpanKey Key { get; }

			public Span(int start, int length, bool isColumn, double value)
			{
				Start = start;
				Length = length;
				IsColumn = isColumn;
				Requested = value;

				Key = new SpanKey(Start, Length, IsColumn);
			}
		}

		internal class SpanKey
		{
			public SpanKey(int start, int length, bool isColumn)
			{
				Start = start;
				Length = length;
				IsColumn = isColumn;
			}

			public int Start { get; }
			public int Length { get; }
			public bool IsColumn { get; }

			public override bool Equals(object? obj)
			{
				return obj is SpanKey key &&
					   Start == key.Start &&
					   Length == key.Length &&
					   IsColumn == key.IsColumn;
			}

			public override int GetHashCode()
			{
				return Start.GetHashCode() ^ Length.GetHashCode() ^ IsColumn.GetHashCode();
			}
		}

		class Cell 
		{ 
			public int ViewIndex { get; }
			public int Row { get; }
			public int Column { get; }
			public int RowSpan { get; }
			public int ColumnSpan { get; }

			public GridLengthType ColumnGridLengthType { get; }
			public GridLengthType RowGridLengthType { get; }

			public Cell(int viewIndex, int row, int column, int rowSpan, int columnSpan, 
				GridLengthType columnGridLengthType, GridLengthType rowGridLengthType) 
			{
				ViewIndex = viewIndex;
				Row = row;
				Column = column;
				RowSpan = rowSpan;
				ColumnSpan = columnSpan;
				ColumnGridLengthType = columnGridLengthType;
				RowGridLengthType = rowGridLengthType;
			}
		}

		[Flags]
		enum GridLengthType 
		{ 
			None = 0, 
			Absolute = 1,
			Auto = 2,
			Star = 4
		}

		static GridLengthType ToGridLengthType(GridUnitType gridUnitType) 
		{
			return gridUnitType switch
			{
				GridUnitType.Absolute => GridLengthType.Absolute,
				GridUnitType.Star => GridLengthType.Star,
				GridUnitType.Auto => GridLengthType.Auto,
				_ => GridLengthType.None,
			};
		}

		class Column 
		{
			public IGridColumnDefinition ColumnDefinition { get; set; }

			public double ActualWidth { get; set; }

			public Column(IGridColumnDefinition columnDefinition)
			{
				ColumnDefinition = columnDefinition;
				if (columnDefinition.Width.IsAbsolute)
				{
					ActualWidth = columnDefinition.Width.Value;
				}
			}

			public void Update(double value) 
			{
				if (value > ActualWidth)
				{
					ActualWidth = value;
				}
			}
		}

		class Row 
		{
			public IGridRowDefinition RowDefinition { get; set; }

			public double ActualHeight { get; set; }

			public Row(IGridRowDefinition rowDefinition) 
			{ 
				RowDefinition = rowDefinition;
				if (rowDefinition.Height.IsAbsolute)
				{
					ActualHeight = rowDefinition.Height.Value;
				}
			}

			public void Update(double value)
			{
				if (value > ActualHeight)
				{
					ActualHeight = value;
				}
			}
		}
	}
}

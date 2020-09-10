using System.ComponentModel;

namespace System.Maui
{
    public class PathGeometry : Geometry
    {
        public PathGeometry()
        {
            Figures = new PathFigureCollection();
        }

        [TypeConverter(typeof(Xaml.PathFigureCollectionConverter))]
        public PathFigureCollection Figures { get; set; }

        public FillRule FillRule { get; set; }
    }
}

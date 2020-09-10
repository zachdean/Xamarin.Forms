namespace System.Maui.Shapes
{
    public class Polygon : Shape, IPolygon
    {
        public Polygon()
        {

        }

        public PointCollection Points { get; set; }

        public FillRule FillRule { get; set; }
    }
}

namespace System.Maui.Shapes
{
    public class Polyline : Shape, IPolyline
    {
        public Polyline()
        {

        }

        public PointCollection Points { get; set; }

        public FillRule FillRule { get; set; } = FillRule.EvenOdd;
    }
}
namespace System.Maui
{ 
    public interface IPolyline : IShape
    {
        PointCollection Points { get; }
        FillRule FillRule { get; }
    }
}
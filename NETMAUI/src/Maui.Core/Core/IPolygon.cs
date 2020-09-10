namespace System.Maui
{
    public interface IPolygon : IShape
    {
        PointCollection Points { get; }
        FillRule FillRule { get; }
    }
}

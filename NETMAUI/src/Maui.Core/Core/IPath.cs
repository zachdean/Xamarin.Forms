namespace System.Maui
{
    public interface IPath : IShape
    {
        Geometry Data { get; }
        Transform RenderTransform { get; }
    }
}
using CoreGraphics;

namespace System.Maui.Controls.Primitives
{
    public class NativePolygon : NativeShape
    {
        public void UpdatePoints(CGPoint[] points)
        {
            var path = new CGPath();
            path.AddLines(points);
            path.CloseSubpath();

            ShapeLayer.UpdateShape(path);
        }

        public void UpdateFillMode(bool fillMode)
        {
            ShapeLayer.UpdateFillMode(fillMode);
        }
    }
}
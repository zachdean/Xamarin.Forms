using CoreGraphics;

namespace System.Maui.Controls.Primitives
{
    public class NativePolyline : NativeShape
    {
        public void UpdatePoints(CGPoint[] points)
        {
            var path = new CGPath();
            path.AddLines(points);

            ShapeLayer.UpdateShape(path);
        }

        public void UpdateFillMode(bool fillMode)
        {
            ShapeLayer.UpdateFillMode(fillMode);
        }
    }
}
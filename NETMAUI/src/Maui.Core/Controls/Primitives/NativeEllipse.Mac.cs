using CoreGraphics;

namespace System.Maui.Controls.Primitives
{
    public class NativeEllipse : NativeShape
    {
        public NativeEllipse()
        {
            UpdateShape();
        }

        void UpdateShape()
        {
            var path = new CGPath();
            path.AddEllipseInRect(new CGRect(0, 0, 1, 1));
            ShapeLayer.UpdateShape(path);
        }
    }
}
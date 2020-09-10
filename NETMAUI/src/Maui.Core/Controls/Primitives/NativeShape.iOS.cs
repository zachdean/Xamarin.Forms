using UIKit;

namespace System.Maui.Controls.Primitives
{
    public class NativeShape : UIView
    {
        public NativeShape()
        {
            BackgroundColor = UIColor.Clear;
            ShapeLayer = new ShapeLayer();
            Layer.AddSublayer(ShapeLayer);
            Layer.MasksToBounds = false;
        }

        public ShapeLayer ShapeLayer
        {
            private set;
            get;
        }
    }
}
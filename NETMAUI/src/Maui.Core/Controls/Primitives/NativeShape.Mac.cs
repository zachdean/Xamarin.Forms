using AppKit;

namespace System.Maui.Controls.Primitives
{
    public class NativeShape : NSView
    {
        public NativeShape()
        {
            WantsLayer = true;
            ShapeLayer = new ShapeLayer();
            Layer.AddSublayer(ShapeLayer);
            Layer.MasksToBounds = false;
        }

        public ShapeLayer ShapeLayer
        {
            private set;
            get;
        }

        public override bool IsFlipped => true;
    }
}
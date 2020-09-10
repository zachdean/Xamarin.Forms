using System.Maui.Controls.Primitives;
using CoreGraphics;

namespace System.Maui.Platform
{
    public partial class ShapeHandler
    {
        public static void MapPropertyFrame(IViewHandler Handler, IShape view)
        {
            double x = view.Frame.X;
            double y = view.Frame.Y;
            double height = view.Frame.Height;
            double width = view.Frame.Width;

            Handler?.SetFrame(new Rect(x, y, width, height));
            var nativeShape = Handler.NativeView as NativeShape;
            nativeShape?.ShapeLayer.UpdateSize(new CGSize(width, height));
        }

        public static void MapPropertyFill(IViewHandler Handler, IShape view)
        {
            var nativeShape = Handler.NativeView as NativeShape;
            nativeShape?.ShapeLayer.UpdateFill(view.Fill);
        }

        public static void MapPropertyStroke(IViewHandler Handler, IShape view)
        {
            var nativeShape = Handler.NativeView as NativeShape;
            nativeShape?.ShapeLayer.UpdateStroke(view.Stroke);
        }

        public static void MapPropertyStrokeThickness(IViewHandler Handler, IShape view)
        {
            var nativeShape = Handler.NativeView as NativeShape;
            nativeShape?.ShapeLayer.UpdateStrokeThickness(view.StrokeThickness);
        }

        public static void MapPropertyStrokeDashArray(IViewHandler Handler, IShape view)
        {
            var nativeShape = Handler.NativeView as NativeShape;

            if (view.StrokeDashArray == null || view.StrokeDashArray.Count == 0)
                nativeShape?.ShapeLayer.UpdateStrokeDash(new nfloat[0]);
            else
            {
                nfloat[] dashArray;
                double[] array;

                if (view.StrokeDashArray.Count % 2 == 0)
                {
                    array = new double[view.StrokeDashArray.Count];
                    dashArray = new nfloat[view.StrokeDashArray.Count];
                    view.StrokeDashArray.CopyTo(array, 0);
                }
                else
                {
                    array = new double[2 * view.StrokeDashArray.Count];
                    dashArray = new nfloat[2 * view.StrokeDashArray.Count];
                    view.StrokeDashArray.CopyTo(array, 0);
                    view.StrokeDashArray.CopyTo(array, view.StrokeDashArray.Count);
                }

                double thickness = view.StrokeThickness;

                for (int i = 0; i < array.Length; i++)
                    dashArray[i] = new nfloat(thickness * array[i]);

                nativeShape?.ShapeLayer.UpdateStrokeDash(dashArray);
            }
        }

        public static void MapPropertyStrokeDashOffset(IViewHandler Handler, IShape view)
        {
            var nativeShape = Handler.NativeView as NativeShape;
            nativeShape?.ShapeLayer.UpdateStrokeDashOffset((nfloat)view.StrokeDashOffset);
        }

        public static void MapPropertyStrokeLineCap(IViewHandler Handler, IShape view)
        {
            PenLineCap lineCap = view.StrokeLineCap;
            CGLineCap iLineCap = CGLineCap.Butt;

            switch (lineCap)
            {
                case PenLineCap.Flat:
                    iLineCap = CGLineCap.Butt;
                    break;
                case PenLineCap.Square:
                    iLineCap = CGLineCap.Square;
                    break;
                case PenLineCap.Round:
                    iLineCap = CGLineCap.Round;
                    break;
            }

            var nativeShape = Handler.NativeView as NativeShape;
            nativeShape?.ShapeLayer.UpdateStrokeLineCap(iLineCap);
        }

        public static void MapPropertyStrokeLineJoin(IViewHandler Handler, IShape view)
        {
            PenLineJoin lineJoin = view.StrokeLineJoin;
            CGLineJoin iLineJoin = CGLineJoin.Miter;

            switch (lineJoin)
            {
                case PenLineJoin.Miter:
                    iLineJoin = CGLineJoin.Miter;
                    break;
                case PenLineJoin.Bevel:
                    iLineJoin = CGLineJoin.Bevel;
                    break;
                case PenLineJoin.Round:
                    iLineJoin = CGLineJoin.Round;
                    break;
            }

            var nativeShape = Handler.NativeView as NativeShape;
            nativeShape?.ShapeLayer.UpdateStrokeLineJoin(iLineJoin);
        }

        public static void MapPropertyStrokeMiterLimit(IViewHandler Handler, IShape view)
        {
            var nativeShape = Handler.NativeView as NativeShape;
            nativeShape?.ShapeLayer.UpdateStrokeMiterLimit(new nfloat(view.StrokeMiterLimit));
        }

        public static void MapPropertyAspect(IViewHandler Handler, IShape view)
        {
            var nativeShape = Handler.NativeView as NativeShape;
            nativeShape?.ShapeLayer.UpdateAspect(view.Aspect);
        }
    }
}
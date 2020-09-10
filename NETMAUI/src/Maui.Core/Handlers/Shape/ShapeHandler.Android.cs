using System.Maui.Controls.Primitives;
using Android.Graphics;

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
            nativeShape?.UpdateSize(width, height);
        }

        public static void MapPropertyFill(IViewHandler Handler, IShape view)
        {
            var nativeShape = Handler.NativeView as NativeShape;
            nativeShape?.UpdateFill(view.Fill);
        }

        public static void MapPropertyStroke(IViewHandler Handler, IShape view)
        {
            var nativeShape = Handler.NativeView as NativeShape;
            nativeShape?.UpdateStroke(view.Stroke);
        }

        public static void MapPropertyStrokeThickness(IViewHandler Handler, IShape view)
        {
            var nativeShape = Handler.NativeView as NativeShape;
            nativeShape?.UpdateStrokeThickness((float)view.StrokeThickness);
        }

        public static void MapPropertyStrokeDashArray(IViewHandler Handler, IShape view)
        {
            var nativeShape = Handler.NativeView as NativeShape;
            nativeShape?.UpdateStrokeDashArray(view.StrokeDashArray.ToNative());
        }

        public static void MapPropertyStrokeDashOffset(IViewHandler Handler, IShape view)
        {
            var nativeShape = Handler.NativeView as NativeShape;
            nativeShape?.UpdateStrokeDashOffset((float)view.StrokeDashOffset);
        }

        public static void MapPropertyStrokeLineCap(IViewHandler Handler, IShape view)
        {
            PenLineCap lineCap = view.StrokeLineCap;
            Paint.Cap aLineCap = Paint.Cap.Butt;

            switch (lineCap)
            {
                case PenLineCap.Flat:
                    aLineCap = Paint.Cap.Butt;
                    break;
                case PenLineCap.Square:
                    aLineCap = Paint.Cap.Square;
                    break;
                case PenLineCap.Round:
                    aLineCap = Paint.Cap.Round;
                    break;
            }

            var nativeShape = Handler.NativeView as NativeShape;
            nativeShape?.UpdateStrokeLineCap(aLineCap);
        }

        public static void MapPropertyStrokeLineJoin(IViewHandler Handler, IShape view)
        {
            PenLineJoin lineJoin = view.StrokeLineJoin;
            Paint.Join aLineJoin = Paint.Join.Miter;

            switch (lineJoin)
            {
                case PenLineJoin.Miter:
                    aLineJoin = Paint.Join.Miter;
                    break;
                case PenLineJoin.Bevel:
                    aLineJoin = Paint.Join.Bevel;
                    break;
                case PenLineJoin.Round:
                    aLineJoin = Paint.Join.Round;
                    break;
            }

            var nativeShape = Handler.NativeView as NativeShape;
            nativeShape?.UpdateStrokeLineJoin(aLineJoin);
        }

        public static void MapPropertyStrokeMiterLimit(IViewHandler Handler, IShape view)
        {
            var nativeShape = Handler.NativeView as NativeShape;
            nativeShape?.UpdateStrokeMiterLimit((float)view.StrokeMiterLimit);
        }

        public static void MapPropertyAspect(IViewHandler Handler, IShape view)
        {
            var nativeShape = Handler.NativeView as NativeShape;
            nativeShape?.UpdateAspect(view.Aspect);
        }
    }
}

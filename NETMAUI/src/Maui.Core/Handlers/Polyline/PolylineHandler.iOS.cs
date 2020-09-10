using System.Maui.Controls.Primitives;

namespace System.Maui.Platform
{
    public partial class PolylineHandler : AbstractViewHandler<IPolyline, NativePolyline>
    {
        protected override NativePolyline CreateView()
        {
            return new NativePolyline();
        }

        public static void MapPropertyPoints(IViewHandler Handler, IPolyline view)
        {
            (Handler as PolylineHandler)?.UpdatePoints();
        }

        public static void MapPropertyFillRule(IViewHandler Handler, IPolyline view)
        {
            (Handler as PolylineHandler)?.UpdateFillRule();
        }

        public virtual void UpdatePoints()
        {
            TypedNativeView.UpdatePoints(VirtualView.Points.ToNative());
        }

        public virtual void UpdateFillRule()
        {
            TypedNativeView.UpdateFillMode(VirtualView.FillRule == FillRule.Nonzero);
        }
    }
}
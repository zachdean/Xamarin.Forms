using System.Maui.Controls.Primitives;

namespace System.Maui.Platform
{
    public partial class PolygonHandler : AbstractViewHandler<IPolygon, NativePolygon>
    {
        protected override NativePolygon CreateView()
        {
            return new NativePolygon(Context);
        }

        public static void MapPropertyPoints(IViewHandler Handler, IPolygon view)
        {
            (Handler as PolygonHandler)?.UpdatePoints();
        }

        public static void MapPropertyFillRule(IViewHandler Handler, IPolygon view)
        {
            (Handler as PolygonHandler)?.UpdateFillRule();
        }

        public virtual void UpdatePoints()
        {
            TypedNativeView.UpdatePoints(VirtualView.Points);
        }

        public virtual void UpdateFillRule()
        {
            TypedNativeView.UpdateFillMode(VirtualView.FillRule == FillRule.Nonzero);
        }
    }
}
using System.Maui.Controls.Primitives;

namespace System.Maui.Platform
{
    public partial class RectangleHandler : AbstractViewHandler<IRectangle, NativeRectangle>
    {
        protected override NativeRectangle CreateView()
        {
            return new NativeRectangle(Context);
        }

        public static void MapPropertyRadiusX(IViewHandler Handler, IRectangle rectangle)
        {
            (Handler as RectangleHandler)?.UpdateRadiusX();
        }

        public static void MapPropertyRadiusY(IViewHandler Handler, IRectangle rectangle)
        {
            (Handler as RectangleHandler)?.UpdateRadiusY();
        }

        public virtual void UpdateRadiusX()
        {
            TypedNativeView.UpdateRadiusX(VirtualView.RadiusX / VirtualView.Frame.Width);
        }

        public virtual void UpdateRadiusY()
        {
            TypedNativeView.UpdateRadiusY(VirtualView.RadiusY / VirtualView.Frame.Height);
        }
    }
}
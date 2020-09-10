using System.Maui.Controls.Primitives;

namespace System.Maui.Platform
{
    public partial class PathHandler : AbstractViewHandler<IPath, NativePath>
    {
        protected override NativePath CreateView()
        {
            return new NativePath(Context);
        }

        public static void MapPropertyData(IViewHandler Handler, IPath view)
        {
            (Handler as PathHandler)?.UpdateData();
        }

        public static void MapPropertyRenderTransform(IViewHandler Handler, IPath view)
        {
            (Handler as PathHandler)?.UpdateRenderTransform();
        }

        public virtual void UpdateData()
        {
            TypedNativeView.UpdateData(VirtualView.Data.ToNative(Context));
        }

        public virtual void UpdateRenderTransform()
        {
            if (VirtualView.RenderTransform != null)
            {
                var density = Context.Resources.DisplayMetrics.Density;
                TypedNativeView.UpdateTransform(VirtualView.RenderTransform.ToNative(density));
            }
        }
    }
}
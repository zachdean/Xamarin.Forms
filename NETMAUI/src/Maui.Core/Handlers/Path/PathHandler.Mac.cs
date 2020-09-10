using System.Maui.Controls.Primitives;

namespace System.Maui.Platform
{
    public partial class PathHandler : AbstractViewHandler<IPath, NativePath>
    {
        protected override NativePath CreateView()
        {
            return new NativePath();
        }

        public static void MapPropertyData(IViewHandler Handler, IPath view)
        {
            (Handler as PathHandler)?.UpdatePath();
        }

        public static void MapPropertyRenderTransform(IViewHandler Handler, IPath view)
        {
            (Handler as PathHandler)?.UpdatePath();
        }

        public virtual void UpdatePath()
        {
            TypedNativeView.UpdatePath(VirtualView.Data.ToNative(VirtualView.RenderTransform));
        }
    }
}
namespace System.Maui.Platform
{
    public partial class PathHandler : AbstractViewHandler<IPath, object>
	{
		public static void MapPropertyData(IViewHandler Handler, IPath view) { }
		public static void MapPropertyRenderTransform(IViewHandler Handler, IPath view) { }

		protected override object CreateView() => throw new NotImplementedException();
	}
}
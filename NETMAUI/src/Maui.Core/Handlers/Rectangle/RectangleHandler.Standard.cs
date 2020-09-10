namespace System.Maui.Platform
{
    public partial class RectangleHandler : AbstractViewHandler<IRectangle, object>
	{
		public static void MapPropertyRadiusX(IViewHandler Handler, IRectangle rectangle) { }
		public static void MapPropertyRadiusY(IViewHandler Handler, IRectangle rectangle) { }

		protected override object CreateView() => throw new NotImplementedException();
	}
}
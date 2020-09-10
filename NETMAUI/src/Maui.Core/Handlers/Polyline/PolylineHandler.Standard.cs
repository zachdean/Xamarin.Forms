namespace System.Maui.Platform
{
    public partial class PolylineHandler : AbstractViewHandler<IPolyline, object>
	{
		public static void MapPropertyPoints(IViewHandler Handler, IPolyline view) { }
		public static void MapPropertyFillRule(IViewHandler Handler, IPolyline view) { }

		protected override object CreateView() => throw new NotImplementedException();
	}
}
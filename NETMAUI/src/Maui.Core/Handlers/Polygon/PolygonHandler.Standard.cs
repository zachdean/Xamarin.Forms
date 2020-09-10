namespace System.Maui.Platform
{
    public partial class PolygonHandler : AbstractViewHandler<IPolygon, object>
	{
		public static void MapPropertyPoints(IViewHandler Handler, IPolygon view) { }
		public static void MapPropertyFillRule(IViewHandler Handler, IPolygon view) { }

		protected override object CreateView() => throw new NotImplementedException();
	}
}
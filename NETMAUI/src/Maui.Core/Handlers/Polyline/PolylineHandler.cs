namespace System.Maui.Platform
{
    public partial class PolylineHandler
    {
		public static PropertyMapper<IPolyline> PolylineMapper = new PropertyMapper<IPolyline>(ShapeHandler.ShapeMapper)
		{
			[nameof(IPolyline.Points)] = MapPropertyPoints,
			[nameof(IPolyline.FillRule)] = MapPropertyFillRule
		};

		public PolylineHandler() : base(PolylineMapper)
		{

		}

		public PolylineHandler(PropertyMapper mapper) : base(mapper ?? PolylineMapper)
		{

		}
	}
}
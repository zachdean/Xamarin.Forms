namespace System.Maui.Platform
{
    public partial class LineHandler : AbstractViewHandler<ILine, object>
	{
		public static void MapPropertyX1(IViewHandler Handler, ILine view) { }
		public static void MapPropertyY1(IViewHandler Handler, ILine view) { }
		public static void MapPropertyX2(IViewHandler Handler, ILine view) { }
		public static void MapPropertyY2(IViewHandler Handler, ILine view) { }

		protected override object CreateView() => throw new NotImplementedException();
	}
}
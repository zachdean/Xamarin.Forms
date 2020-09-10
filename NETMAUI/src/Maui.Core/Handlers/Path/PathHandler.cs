namespace System.Maui.Platform
{
    public partial class PathHandler
    {
		public static PropertyMapper<IPath> PathMapper = new PropertyMapper<IPath>(ShapeHandler.ShapeMapper)
		{
			[nameof(IPath.Data)] = MapPropertyData,
			[nameof(IPath.RenderTransform)] = MapPropertyRenderTransform
		};

		public PathHandler() : base(PathMapper)
		{

		}

		public PathHandler(PropertyMapper mapper) : base(mapper ?? PathMapper)
		{

		}
	}
}
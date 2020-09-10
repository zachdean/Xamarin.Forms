namespace System.Maui.Platform
{
	public partial class ActivityIndicatorHandler : AbstractViewHandler<IActivityIndicator, object>
	{
		public static void MapPropertyIsRunning(IViewHandler Handler, IActivityIndicator activityIndicator) { }
		public static void MapPropertyColor(IViewHandler Handler, IActivityIndicator activityIndicator) { }

		protected override object CreateView() => throw new NotImplementedException();
	}
}
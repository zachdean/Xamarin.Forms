namespace System.Maui.Platform
{
	public partial class ViewHandler
	{
		public static PropertyMapper<IView> ViewMapper = new PropertyMapper<IView>
		{
			[nameof(IView.IsEnabled)] = MapPropertyIsEnabled,
			[nameof(IView.BackgroundColor)] = MapBackgroundColor,
			[nameof(IView.Frame)] = MapPropertyFrame
		};

		public static void MapPropertyFrame(IViewHandler Handler, IView view)
			=> Handler?.SetFrame(view.Frame);
	}
}
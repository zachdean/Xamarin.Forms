namespace Xamarin.Platform.Handlers
{
	public partial class ViewHandler
	{
		public static PropertyMapper<IView> ViewMapper = new PropertyMapper<IView>
		{
			[nameof(IView.BackgroundColor)] = MapBackgroundColor,
			[nameof(IView.Frame)] = MapPropertyFrame,
		};

		public static void MapPropertyFrame(IViewHandler renderer, IView view)
			=> renderer?.SetFrame(view.Frame);
	}
}
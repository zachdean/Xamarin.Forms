namespace System.Maui.Platform
{
	public partial class ActivityIndicatorHandler
	{
		public static PropertyMapper<IActivityIndicator> ActivityIndicatorMapper = new PropertyMapper<IActivityIndicator>(ViewHandler.ViewMapper)
		{
			[nameof(IActivityIndicator.Color)] = MapPropertyColor,
			[nameof(IActivityIndicator.IsRunning)] = MapPropertyIsRunning
		};

		public ActivityIndicatorHandler() : base(ActivityIndicatorMapper)
		{

		}

		public ActivityIndicatorHandler(PropertyMapper mapper) : base(mapper ?? ActivityIndicatorMapper)
		{

		}
	}
}
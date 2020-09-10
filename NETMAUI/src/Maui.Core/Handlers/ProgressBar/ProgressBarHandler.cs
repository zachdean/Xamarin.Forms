namespace System.Maui.Platform
{
	public partial class ProgressBarHandler
	{
		public static PropertyMapper<IProgress> ProgressMapper = new PropertyMapper<IProgress>(ViewHandler.ViewMapper)
		{
			[nameof(IProgress.Progress)] = MapPropertyProgress,
			[nameof(IProgress.ProgressColor)] = MapPropertyProgressColor
		};

		public ProgressBarHandler() : base(ProgressMapper)
		{

		}

		public ProgressBarHandler(PropertyMapper mapper) : base(mapper ?? ProgressMapper)
		{

		}
	}
}

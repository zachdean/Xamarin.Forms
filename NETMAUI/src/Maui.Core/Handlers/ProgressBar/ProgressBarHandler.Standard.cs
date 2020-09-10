namespace System.Maui.Platform
{
	public partial class ProgressBarHandler : AbstractViewHandler<IProgress, object>
	{
		protected override object CreateView() => throw new NotImplementedException();

		public static void MapPropertyProgress(IViewHandler Handler, IProgress progressBar) { }
		public static void MapPropertyProgressColor(IViewHandler Handler, IProgress progressBar) { }
	}
}
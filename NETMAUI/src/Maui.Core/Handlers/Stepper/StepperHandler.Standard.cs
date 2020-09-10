namespace System.Maui.Platform
{
	public partial class StepperHandler : AbstractViewHandler<IStepper, object>
	{
		protected override object CreateView() => throw new NotImplementedException();

		public static void MapPropertyMinimum(IViewHandler Handler, IStepper slider) { }
		public static void MapPropertyMaximum(IViewHandler Handler, IStepper slider) { }
		public static void MapPropertyIncrement(IViewHandler Handler, IStepper slider) { }
		public static void MapPropertyValue(IViewHandler Handler, IStepper slider) { }
	}
}
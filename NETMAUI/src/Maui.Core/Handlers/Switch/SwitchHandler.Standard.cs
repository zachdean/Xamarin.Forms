namespace System.Maui.Platform
{
	public partial class SwitchHandler : AbstractViewHandler<ISwitch, object>
	{
		public static void MapPropertyIsToggled(IViewHandler Handler, ISwitch view) { }
		public static void MapPropertyOnColor(IViewHandler Handler, ISwitch view) { }
		public static void MapPropertyThumbColor(IViewHandler Handler, ISwitch view) { }

		protected override object CreateView() => throw new NotImplementedException();
	}
}
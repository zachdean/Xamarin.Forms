namespace System.Maui.Platform
{
	public partial class SliderHandler : AbstractViewHandler<ISlider, object>
	{
		protected override object CreateView() => throw new NotImplementedException();

		public static void MapPropertyMinimum(IViewHandler Handler, ISlider slider) { }
		public static void MapPropertyMaximum(IViewHandler Handler, ISlider slider) { }
		public static void MapPropertyValue(IViewHandler Handler, ISlider slider) { }
		public static void MapPropertyMinimumTrackColor(IViewHandler Handler, ISlider slider) { }
		public static void MapPropertyMaximumTrackColor(IViewHandler Handler, ISlider slider) { }
		public static void MapPropertyThumbColor(IViewHandler Handler, ISlider slider) { }
	}
}
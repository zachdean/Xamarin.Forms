namespace Xamarin.Platform.Handlers
{
	public partial class PolylineHandler : AbstractViewHandler<IPolyline, NativePolyline>
	{
		protected override NativePolyline CreateNativeView() => new NativePolyline(Context);
	}
}
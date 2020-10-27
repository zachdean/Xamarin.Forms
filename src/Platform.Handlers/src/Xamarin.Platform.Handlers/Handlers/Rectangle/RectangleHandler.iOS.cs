namespace Xamarin.Platform.Handlers
{
	public partial class RectangleHandler : AbstractViewHandler<IRectangle, NativeRectangle>
	{
		protected override NativeRectangle CreateNativeView() => new NativeRectangle();
	}
}
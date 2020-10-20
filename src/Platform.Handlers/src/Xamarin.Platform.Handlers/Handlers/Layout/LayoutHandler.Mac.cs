using AppKit;

namespace Xamarin.Platform.Handlers
{
	public partial class LayoutHandler : AbstractViewHandler<ILayout, NSView>
	{
		protected override NSView CreateNativeView()
		{
			return new NSView();
		}
	}
}
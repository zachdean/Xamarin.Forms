using AppKit;

namespace Xamarin.Platform.Handlers
{
	public partial class LayoutHandler : AbstractViewHandler<ILayout, NSView>
	{
		protected override NSView CreateView()
		{
			return new NSView();
		}
	}
}

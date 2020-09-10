using AppKit;

namespace Xamarin.Platform
{
	public interface INativeViewRenderer : IViewRenderer
	{
		NSView View { get; }
	}
}

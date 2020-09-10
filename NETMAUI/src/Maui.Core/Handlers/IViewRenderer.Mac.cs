using AppKit;

namespace System.Maui
{
	public interface INativeViewHandler : IViewHandler
	{
		NSView View { get; }
	}
}
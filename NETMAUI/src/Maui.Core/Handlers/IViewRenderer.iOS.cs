using CoreGraphics;
using UIKit;

namespace System.Maui
{
	public interface INativeViewHandler : IViewHandler
	{
		UIView View { get; }
	}
}
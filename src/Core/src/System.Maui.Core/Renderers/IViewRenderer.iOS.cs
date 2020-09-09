using System;
using UIKit;

namespace System.Maui {
	public interface INativeViewRenderer : IViewRenderer {

		UIView View { get; }

	}
}

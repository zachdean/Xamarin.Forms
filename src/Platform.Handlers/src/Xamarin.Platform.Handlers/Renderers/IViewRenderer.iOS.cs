using System;
using UIKit;

namespace Xamarin.Platform {
	public interface INativeViewRenderer : IViewRenderer {

		UIView View { get; }

	}
}

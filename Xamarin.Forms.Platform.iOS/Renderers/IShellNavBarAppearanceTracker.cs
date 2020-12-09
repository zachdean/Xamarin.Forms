using System;
using UIKit;

namespace Xamarin.Forms.Platform.iOS
{

	public interface IShellNavBarAppearanceTracker : IDisposable
	{
		ShellAppearance CurrentAppearance { get; }
		void ResetAppearance(UINavigationController controller);
		void SetAppearance(UINavigationController controller, ShellAppearance appearance);
		void UpdateLayout(UINavigationController controller);
		void SetHasShadow(UINavigationController controller, bool hasShadow);
	}
}
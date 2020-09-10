using UIKit;

namespace System.Maui.Platform
{
	public partial class ProgressBarHandler : AbstractViewHandler<IProgress, UIProgressView>
	{
		protected override UIProgressView CreateView()
		{
			return new UIProgressView(UIProgressViewStyle.Default);
		}

		public static void MapPropertyProgress(IViewHandler Handler, IProgress progressBar)
		{
			if (!(Handler.NativeView is UIProgressView uIProgressView))
				return;

			uIProgressView.Progress = (float)progressBar.Progress;
		}

		public static void MapPropertyProgressColor(IViewHandler Handler, IProgress progressBar)
		{
			if (!(Handler.NativeView is UIProgressView uIProgressView))
				return;

			uIProgressView.ProgressTintColor = progressBar.ProgressColor == Color.Default ? null : progressBar.ProgressColor.ToNative();
		}
	}
}
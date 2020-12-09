using Android.Graphics.Drawables;
using AndroidX.AppCompat.Widget;
using AColor = Android.Graphics.Color;

namespace Xamarin.Forms.Platform.Android
{
	public class ShellToolbarAppearanceTracker : IShellToolbarAppearanceTracker
	{
		bool _disposed;
		IShellContext _shellContext;
		int _titleTextColor = -1;

		public ShellToolbarAppearanceTracker(IShellContext shellContext)
		{
			_shellContext = shellContext;
		}

		public virtual void SetAppearance(Toolbar toolbar, IShellToolbarTracker toolbarTracker, ShellAppearance appearance)
		{
			var foreground = appearance.ForegroundColor;
			var backgroundColor = appearance.BackgroundColor;
			var background = appearance.Background;
			var titleColor = appearance.TitleColor;

			SetColors(toolbar, toolbarTracker, foreground, backgroundColor, background, titleColor);
		}

		public virtual void ResetAppearance(Toolbar toolbar, IShellToolbarTracker toolbarTracker)
		{
			SetColors(toolbar, toolbarTracker, ShellRenderer.DefaultForegroundColor, ShellRenderer.DefaultBackgroundColor, ShellRenderer.DefaultBackground, ShellRenderer.DefaultTitleColor);
		}

		protected virtual void SetColors(Toolbar toolbar, IShellToolbarTracker toolbarTracker, Color foreground, Color backgroundColor, Brush background, Color title)
		{
			var titleArgb = title.ToAndroid(ShellRenderer.DefaultTitleColor).ToArgb();

			if (_titleTextColor != titleArgb)
			{
				toolbar.SetTitleTextColor(titleArgb);
				_titleTextColor = titleArgb;
			}

			bool isDefaultBackground = Brush.IsNullOrEmpty(background);
			var newBackground = isDefaultBackground ? ShellRenderer.DefaultBackground : background;

			AColor? defaultBackgroundColor = null;
			if (isDefaultBackground && newBackground is SolidColorBrush solidColorBrush)
				defaultBackgroundColor = solidColorBrush.Color.ToAndroid();

			if (!isDefaultBackground || (isDefaultBackground && (!(toolbar.Background is ColorDrawable bcd) || bcd.Color != defaultBackgroundColor)))
				toolbar.UpdateBackground(newBackground);

			var newColor = backgroundColor.ToAndroid(ShellRenderer.DefaultBackgroundColor);

			if (isDefaultBackground && (!(toolbar.Background is ColorDrawable bccd) || bccd.Color != newColor))
			{
				using (var colorDrawable = new ColorDrawable(backgroundColor.ToAndroid(ShellRenderer.DefaultBackgroundColor)))
					toolbar.SetBackground(colorDrawable);
			}

			var newTintColor = foreground.IsDefault ? ShellRenderer.DefaultForegroundColor : foreground;

			if (toolbarTracker.TintColor != newTintColor)
				toolbarTracker.TintColor = newTintColor;
		}

		#region IDisposable

		public void Dispose()
		{
			Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
				return;

			_disposed = true;

			if (disposing)
			{
				_shellContext = null;
			}
		}

		#endregion IDisposable
	}
}
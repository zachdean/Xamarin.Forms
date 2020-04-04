using System;

namespace Xamarin.Forms
{
    public class AppThemeChangedEventArgs : EventArgs
    {
        public AppThemeChangedEventArgs(Essentials.AppTheme appTheme) =>
            RequestedTheme = appTheme;

        public Essentials.AppTheme RequestedTheme { get; }
    }
}
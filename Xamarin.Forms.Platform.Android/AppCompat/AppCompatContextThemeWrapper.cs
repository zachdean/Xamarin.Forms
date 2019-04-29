
using Android.Content;
using Android.Views;
using Xamarin.Forms.Platform.Android;
using AndroidAppCompat = Android.Support.V7.Content.Res.AppCompatResources;

namespace Xamarin.Forms.Platform.Android
{
	public class AppCompatContextThemeWrapper : ContextThemeWrapper
	{
		AppCompatContextThemeWrapper(Context context) : this(context, Resource.Style.XamarinFormsAppCompatTheme)
		{
		}

		AppCompatContextThemeWrapper(Context context, int themeResId) : base(context, themeResId)
		{

		}

		public static AppCompatContextThemeWrapper Create(Context context)
		{
			if (context is AppCompatContextThemeWrapper materialContext)
				return materialContext;

			return new AppCompatContextThemeWrapper(context);
		}
	}
}

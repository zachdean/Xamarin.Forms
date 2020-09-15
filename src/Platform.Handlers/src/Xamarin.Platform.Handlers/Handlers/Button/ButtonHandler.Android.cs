using System;

#if __ANDROID_29__
using AndroidX.Core.View;
using AndroidX.AppCompat.Widget;
#else
using Android.Support.V4.View;
using Android.Support.V7.Widget;
# endif

namespace Xamarin.Platform.Handlers
{
	public partial class ButtonHandler : AbstractViewHandler<IButton, AppCompatButton>
	{
		protected override AppCompatButton CreateView() => throw new NotImplementedException();

		public static void MapPropertyText(IViewHandler handler, IButton view) { }
	}
}
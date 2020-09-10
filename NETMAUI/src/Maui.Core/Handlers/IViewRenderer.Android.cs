using Android.Content;
using AView = Android.Views.View;

namespace System.Maui
{
	public interface IAndroidViewHandler : IViewHandler
	{
		void SetContext(Context context);

		AView View { get; }
	}
}
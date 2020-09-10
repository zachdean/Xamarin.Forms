using System;
using AView = Android.Views.View;
using Android.Content;

namespace Xamarin.Platform {
	public interface IAndroidViewRenderer : IViewRenderer
	{

		void SetContext(Context context);

		AView View { get; }
	}
}

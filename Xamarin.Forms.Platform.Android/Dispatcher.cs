using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Xamarin.Forms.Platform.Android
{
	public class Dispatcher : IDispatcher
	{
		public void BeginInvokeOnMainThread(Action action)
		{
			Device.BeginInvokeOnMainThread(action);
		}

		bool IDispatcher.IsInvokeRequired => Device.IsInvokeRequired;
	}
}

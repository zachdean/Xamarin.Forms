using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WPF;

namespace Xamarin.Forms.Platform.WPF
{
	internal class Dispatcher : IDispatcher
	{
		public void BeginInvokeOnMainThread(Action action)
		{
			Device.BeginInvokeOnMainThread(action);
		}

		bool IDispatcher.IsInvokeRequired => Device.IsInvokeRequired;
	}
}

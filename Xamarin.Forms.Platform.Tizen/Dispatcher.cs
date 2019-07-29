using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Tizen;

namespace Xamarin.Forms.Platform.Tizen
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

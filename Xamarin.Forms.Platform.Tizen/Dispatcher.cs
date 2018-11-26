using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xamarin.Forms.Platform.Tizen
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

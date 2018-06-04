using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Forms.Platform.GTK
{
	public class GtkDispatcher : IDispatcher
	{
		public void BeginInvokeOnMainThread(Action action)
		{
			Device.BeginInvokeOnMainThread(action);
		}

		public bool IsInvokeRequired()
		{
			return Device.IsInvokeRequired;
		}
	}
}

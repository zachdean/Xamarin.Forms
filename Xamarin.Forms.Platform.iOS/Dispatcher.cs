using System;
using System.Collections.Generic;
using System.Text;

#if __MOBILE__
namespace Xamarin.Forms.Platform.iOS
#else

namespace Xamarin.Forms.Platform.MacOS
#endif
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

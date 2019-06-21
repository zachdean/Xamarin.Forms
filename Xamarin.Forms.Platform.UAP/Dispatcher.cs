using System;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace Xamarin.Forms.Platform.UWP
{
	public class Dispatcher : IDispatcher
	{

		readonly CoreDispatcher _coreDispatcher;

		public void BeginInvokeOnMainThread(Action action)
		{
			_coreDispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action()).WatchForError();
		}

		public Dispatcher()
		{
			_coreDispatcher = CoreApplication.GetCurrentView().Dispatcher;
		}

		bool IDispatcher.IsInvokeRequired => Device.IsInvokeRequired;
	}
}

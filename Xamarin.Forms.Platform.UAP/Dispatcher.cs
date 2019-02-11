using System;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace Xamarin.Forms.Platform.UWP
{
	public class Dispatcher : IDispatcher
	{

		CoreDispatcher _coreDispatcher;
		CoreDispatcher CoreDispatcher
		{
			get
			{
				if (_coreDispatcher == null)
				{
					_coreDispatcher = CoreApplication.GetCurrentView().Dispatcher;
				}

				return _coreDispatcher;
			}
		}

		public void BeginInvokeOnMainThread(Action action)
		{
			CoreDispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action()).WatchForError();
		}

		bool IDispatcher.IsInvokeRequired => !CoreDispatcher.HasThreadAccess;
	}
}

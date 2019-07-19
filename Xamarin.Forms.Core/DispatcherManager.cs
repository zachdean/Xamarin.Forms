using System;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms
{
	public class DispatcherManager
	{
		[ThreadStatic]
		static DispatcherManager s_current;

		IDispatcher _dispatcher;
		public IDispatcher Dispatcher
		{
			get
			{
				return _dispatcher;
			}
		}

		public static DispatcherManager Current
		{
			get
			{
				if (s_current == null)
				{
					s_current = new DispatcherManager();
					s_current.Init();
				}

				return s_current;
			}
		}

		private void Init()
		{
			if (_dispatcher == null)
			{
				try
				{
					_dispatcher = DependencyService.Get<IDispatcherProvider>().GetDispatcher();
				}
				catch
				{
					_dispatcher = null;
					Log.Warning("DispatcherManager.Init", "The Dispacther could not be recovered correctly, ensure that the Xamarin.Forms.Init(); was called.");
				}
			}
		}
	}
}

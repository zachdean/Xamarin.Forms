using System;

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
				if (_dispatcher == null)
					throw new Exception("The Init() method must be called before getting this property");

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

		internal void Init()
		{
			if (_dispatcher == null)
			{
				_dispatcher = DependencyService.Get<IDispatcherProvider>().GetDispatcher();
			}
		}
	}
}

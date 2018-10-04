using System;
using System.Collections.Generic;
using System.Text;

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
					_dispatcher = Device.GetDispatcher();

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
				}

				return s_current;
			}
		}
	}
}

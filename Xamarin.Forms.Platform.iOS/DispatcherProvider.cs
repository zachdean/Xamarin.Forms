using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
#if __MOBILE__
using Xamarin.Forms.Platform.iOS;
#else

using Xamarin.Forms.Platform.MacOS;
#endif

[assembly: Dependency(typeof(DispatcherProvider))]
#if __MOBILE__
namespace Xamarin.Forms.Platform.iOS
#else

namespace Xamarin.Forms.Platform.MacOS
#endif
{
	internal class DispatcherProvider : IDispatcherProvider
	{
		IDispatcher _dispatcher = new Dispatcher();

		public IDispatcher GetDispatcher(object context)
		{
			return _dispatcher;
		}
	}
}

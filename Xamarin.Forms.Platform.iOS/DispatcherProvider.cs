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
	public class DispatcherProvider : IDispatcherProvider
	{
		public IDispatcher GetDispatcher()
		{
			return new Dispatcher();
		}
	}
}

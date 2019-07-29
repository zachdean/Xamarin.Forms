using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Dependency(typeof(DispatcherProvider))]
namespace Xamarin.Forms.Platform.Android
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

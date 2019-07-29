using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.GTK;

[assembly: Dependency(typeof(GtkDispatcherProvider))]
namespace Xamarin.Forms.Platform.GTK
{
	internal class GtkDispatcherProvider : IDispatcherProvider
	{
		IDispatcher _dispatcher = new GtkDispatcher();

		public IDispatcher GetDispatcher(object context)
		{
			return _dispatcher;
		}
	}
}

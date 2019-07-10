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
	public class GtkDispatcherProvider : IDispatcherProvider
	{
		public IDispatcher GetDispatcher()
		{
			return new GtkDispatcher();
		}
	}
}

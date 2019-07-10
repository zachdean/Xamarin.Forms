using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: Dependency(typeof(DispatcherProvider))]
namespace Xamarin.Forms.Platform.UWP
{
	public class DispatcherProvider : IDispatcherProvider
	{
		public IDispatcher GetDispatcher()
		{
			return new Dispatcher();
		}
	}
}

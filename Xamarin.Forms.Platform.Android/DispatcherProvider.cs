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
	public class DispatcherProvider : IDispatcherProvider
	{
		public IDispatcher GetDispatcher()
		{
			return new Dispatcher();
		}
	}
}

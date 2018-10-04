using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Forms
{
	public interface IDispatcher
	{ 
		void BeginInvokeOnMainThread(Action action);
		bool IsInvokeRequired();
	}
}

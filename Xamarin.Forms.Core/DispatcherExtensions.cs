using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Forms
{
	internal static class DispatcherExtensions
	{
		static IDispatcherProvider s_current;

		public static IDispatcher GetDispatcher(this BindableObject bindableObject)
		{
			s_current = s_current ?? DependencyService.Get<IDispatcherProvider>();
			return s_current.GetDispatcher(bindableObject);
		}
	}
}

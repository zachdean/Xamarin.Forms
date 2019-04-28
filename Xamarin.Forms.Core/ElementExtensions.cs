using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin.Forms
{
	internal static class ElementExtensions
	{
		public static Application FindApplication(this Element element)
		{
			if (element == null)
				return null;

			return (element.Parent is Application app) ? app : FindApplication(element.Parent);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Xamarin.Forms.Internals
{
	class KnownAssemblies
	{
		internal static bool IsKnownAssembly(Assembly assembly)
		{
			var fullName = assembly.FullName;
			if (fullName.StartsWith("mscorlib"))
				return true;
			if (fullName.StartsWith("System"))
				return true;
			if (fullName.StartsWith("Mono"))
				return true;
			if (fullName.StartsWith("Java"))
				return true;
			if (fullName.StartsWith("FormsViewGroup"))
				return true;
			if (fullName.StartsWith("Xamarin") && !fullName.StartsWith("Xamarin.Forms.Platform.Android"))
				return true;
			return false;
		}
	}
}

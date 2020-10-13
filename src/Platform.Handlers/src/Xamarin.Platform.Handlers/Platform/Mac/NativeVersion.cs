using Foundation;

namespace Xamarin.Platform
{
	public static partial class NativeVersion
	{
		public static bool IsAtLeast(int version)
		{
			return NSProcessInfo.ProcessInfo.IsOperatingSystemAtLeastVersion(new NSOperatingSystemVersion(10, version, 0));
		}

		public static bool Supports(int capability)
		{
			return IsAtLeast(capability);
		}
	}
}
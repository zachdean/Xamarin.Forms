using UIKit;

namespace System.Maui.Platform
{
    public static class ReturnTypeExtensions
    {
		internal static UIReturnKeyType ToNative(this ReturnType returnType)
		{
			switch (returnType)
			{
				case ReturnType.Go:
					return UIReturnKeyType.Go;
				case ReturnType.Next:
					return UIReturnKeyType.Next;
				case ReturnType.Send:
					return UIReturnKeyType.Send;
				case ReturnType.Search:
					return UIReturnKeyType.Search;
				case ReturnType.Done:
					return UIReturnKeyType.Done;
				case ReturnType.Default:
					return UIReturnKeyType.Default;
				default:
					throw new System.NotImplementedException($"ReturnType {returnType} not supported");
			}
		}
	}
}
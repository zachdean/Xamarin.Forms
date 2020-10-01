using System;
using UIKit;
using Xamarin.Forms;

namespace Xamarin.Platform
{
	public static class ReturnKeyType
	{
		public static UIReturnKeyType ToUIReturnKeyType(this ReturnType returnType)
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
					throw new NotImplementedException($"ReturnType {returnType} not supported");
			}
		}
	}
}

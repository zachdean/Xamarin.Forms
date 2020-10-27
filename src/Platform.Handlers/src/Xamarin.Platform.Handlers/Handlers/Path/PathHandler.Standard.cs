using System;

namespace Xamarin.Platform.Handlers
{
    public partial class PathHandler : AbstractViewHandler<IPath, object>
	{
		protected override object CreateNativeView() => throw new NotImplementedException();
	}
}
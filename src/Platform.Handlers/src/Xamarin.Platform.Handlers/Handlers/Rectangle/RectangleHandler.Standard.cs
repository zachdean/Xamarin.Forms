using System;

namespace Xamarin.Platform.Handlers
{
	public partial class RectangleHandler : AbstractViewHandler<IRectangle, object>
	{
		protected override object CreateNativeView() => throw new NotImplementedException();
	}
}
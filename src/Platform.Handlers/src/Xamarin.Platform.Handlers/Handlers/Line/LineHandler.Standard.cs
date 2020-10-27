using System;

namespace Xamarin.Platform.Handlers
{
	public partial class LineHandler : AbstractViewHandler<ILine, object>
	{
		protected override object CreateNativeView() => throw new NotImplementedException();
	}
}
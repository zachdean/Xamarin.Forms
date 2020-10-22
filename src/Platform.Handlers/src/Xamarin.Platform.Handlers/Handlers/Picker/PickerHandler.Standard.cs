using System;

namespace Xamarin.Platform.Handlers
{
	public partial class PickerHandler : AbstractViewHandler<IPicker, object>
	{
		protected override object CreateNativeView() => throw new NotImplementedException();
	}
}
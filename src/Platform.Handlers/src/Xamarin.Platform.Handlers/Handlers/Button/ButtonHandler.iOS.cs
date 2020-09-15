using System;
using UIKit;

namespace Xamarin.Platform.Handlers
{
	public partial class ButtonHandler : AbstractViewHandler<IButton, UIButton>
	{
		protected override UIButton CreateView() => throw new NotImplementedException();

		public static void MapText(IViewHandler handler, IButton view) { }
	}
}
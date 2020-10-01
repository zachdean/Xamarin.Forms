using Android.Widget;

namespace Xamarin.Platform.Handlers
{
	public partial class LabelHandler : AbstractViewHandler<ILabel, TextView>
	{
		protected override TextView CreateView() => new TextView(Context);
	}
}
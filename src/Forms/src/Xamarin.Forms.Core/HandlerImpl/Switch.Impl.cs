using Xamarin.Platform;

namespace Xamarin.Forms
{
	public partial class Switch : ISwitch
	{
		void ISwitch.Toggled()
		{
			Toggled?.Invoke(this, new ToggledEventArgs(IsToggled));
			ChangeVisualState();
		}
	}
}
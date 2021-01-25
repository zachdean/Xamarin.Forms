using Xamarin.Forms;

namespace Xamarin.Platform
{
	public interface IBorder : IView
	{
		Color BorderColor { get; }
		int CornerRadius { get; }
		double BorderWidth { get; }
	}
}
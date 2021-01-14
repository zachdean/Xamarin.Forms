using Xamarin.Forms;

namespace Xamarin.Platform
{
	public interface IGridColumnDefinition
	{
		GridLength Width { get; }
		double ActualWidth { get; }
	}
}
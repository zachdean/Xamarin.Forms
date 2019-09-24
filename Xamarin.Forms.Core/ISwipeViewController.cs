namespace Xamarin.Forms
{
	public interface ISwipeViewController : IViewController
	{
		bool HandleTouchInteractions(GestureStatus status, Point point);
	}
}
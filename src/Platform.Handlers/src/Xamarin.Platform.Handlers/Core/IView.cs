namespace Xamarin.Platform
{
	public interface IView : IFrameworkElement
	{
		Alignment GetVerticalAlignment(ILayout layout);
		Alignment GetHorizontalAlignment(ILayout layout);
	}
}
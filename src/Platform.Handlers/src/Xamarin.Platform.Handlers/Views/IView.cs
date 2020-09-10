namespace Xamarin.Platform
{
	public interface IView : IFrameworkElement
	{

		Alignment GetVerticalAlignment(ILayout layout) => Alignment.Fill;
		Alignment GetHorizontalAlignment(ILayout layout) => Alignment.Fill;
	}
}

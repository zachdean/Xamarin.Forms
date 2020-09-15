namespace Xamarin.Platform
{
	public interface IPage : IFrameworkElement
	{
		string Title { get; }

		object Content { get; }
	}
}

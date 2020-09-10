namespace Xamarin.Platform
{
	public interface IText : IView
	{
		string Text { get; }

		TextType TextType { get; }
		//TODO: Add fonts and Colors
		Color Color { get; }
	}
}

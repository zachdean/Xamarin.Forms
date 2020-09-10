namespace System.Maui
{
	public interface IFont : IView
	{
		FontAttributes FontAttributes { get; }
		string FontFamily { get; }
		double FontSize { get; }
	}
}
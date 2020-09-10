namespace System.Maui
{
	public interface IBorder : IView
	{
		int CornerRadius { get; }
		Color BorderColor { get; }
		double BorderWidth { get; }
	}
}
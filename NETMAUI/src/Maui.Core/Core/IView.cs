namespace System.Maui
{
	public interface IView
	{
		bool IsEnabled { get; }
		bool IsFocused { get; set; }
		Color BackgroundColor { get; }
		Rect Frame { get; }
		IViewHandler Handler { get; set; }
		IView Parent { get; }
	}
}
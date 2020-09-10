namespace System.Maui
{
	public interface ISwitch : IView
	{
		bool IsToggled { get; set; }
		Color OnColor { get; }
		Color ThumbColor { get; }
	}
}
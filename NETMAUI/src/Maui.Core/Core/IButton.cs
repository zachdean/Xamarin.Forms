namespace System.Maui
{
	public interface IButton : IText, IBorder
	{
		void Pressed();
		void Released();
		void Clicked();
	}
}
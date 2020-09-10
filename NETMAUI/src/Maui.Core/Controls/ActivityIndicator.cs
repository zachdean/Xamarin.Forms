namespace System.Maui.Controls
{
	public class ActivityIndicator : View, IActivityIndicator
	{
		public bool IsRunning { get; set; } = true;

		public Color Color { get; set; }
	}
}
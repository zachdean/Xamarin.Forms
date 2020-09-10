namespace System.Maui.Controls
{
	public class Switch : View, ISwitch
	{
		public Switch()
		{

		}

		public bool IsToggled { get; set; }

		public Color OnColor { get; set; }

		public Color ThumbColor { get; set; }
	}
}

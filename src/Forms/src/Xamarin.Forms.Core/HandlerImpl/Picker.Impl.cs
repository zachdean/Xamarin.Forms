using Xamarin.Platform;

namespace Xamarin.Forms
{
	public partial class Picker : IPicker
	{
		public string Text { get; set; }

		public Color Color { get; set; }

		public Font Font { get; set; }

		void IPicker.SelectedIndexChanged()
		{
			throw new System.NotImplementedException();
		}
	}
}
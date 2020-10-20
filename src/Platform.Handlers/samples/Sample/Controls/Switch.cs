using System;
using Xamarin.Forms;
using Xamarin.Platform;

namespace Sample
{
	public class Switch : Xamarin.Forms.View, ISwitch
	{
		public Switch()
		{

		}

		public bool IsToggled { get; set; }

		public Color OnColor { get; set; }

		public Color ThumbColor { get; set; }

		public Action Toggled { get; set; }

		void ISwitch.Toggled() => Toggled?.Invoke();
	}
}
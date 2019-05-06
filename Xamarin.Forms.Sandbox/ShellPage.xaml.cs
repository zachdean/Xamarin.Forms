using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xamarin.Forms.Sandbox
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ShellPage : Shell
	{
		public ShellPage()
		{
			InitializeComponent();
		}

		private void Button_Clicked(object sender, EventArgs e)
		{
			var page = (sender as Element).Parent.Parent as Page;

			if(Shell.GetTabBarIsVisible(this) != false)
				Shell.SetTabBarIsVisible(this, false);
			else
				Shell.SetTabBarIsVisible(this, true);
		}
	}
}
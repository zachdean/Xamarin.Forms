using System;
using System.Collections.Generic;
using System.Maui;
using System.Maui.Platform;
using System.Text;
using Android.Views;
using AndroidX.AppCompat.App;

namespace Sample
{
	public partial class CustomButton
	{
		public CustomButton()
		{
			this.Clicked += CustomButton_Clicked;
		}

		private void CustomButton_Clicked(object sender, EventArgs e)
		{
			var dialog = new AlertDialog.Builder(this.GetContext());
			AlertDialog alert = dialog.Create();
			alert.SetTitle("Title");
			alert.SetMessage("Hello From Android Alert Dialog");
			alert.Show();
		}
	}
}

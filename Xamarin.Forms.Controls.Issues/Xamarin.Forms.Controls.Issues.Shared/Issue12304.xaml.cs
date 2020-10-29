using System;
using System.Diagnostics;
using Xamarin.Forms.CustomAttributes;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

#if UITEST
using Xamarin.UITest;
using Xamarin.UITest.Queries;
using NUnit.Framework;
using Xamarin.Forms.Core.UITests;
#endif

namespace Xamarin.Forms.Controls.Issues
{
#if UITEST
	[Category(UITestCategories.Button)]
#endif
#if APP
	[XamlCompilation(XamlCompilationOptions.Compile)]
#endif
	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Github, 12303, "[Bug] Button color is wrong on UWP",
		PlatformAffected.UWP)]
	public partial class Issue12303 : TestContentPage
	{
		public Issue12303()
		{
#if APP
			InitializeComponent();
#endif
		}

		protected override void Init()
		{

		}
#if APP
		void DropGestureRecognizer_DragOver(System.Object sender, Xamarin.Forms.DragEventArgs e)
		{
			StackLayout sl = ((StackLayout)(sender as DropGestureRecognizer).Parent);
			sl.BackgroundColor = Color.Green;
		}

		void DropGestureRecognizer_DragLeave(System.Object sender, Xamarin.Forms.DragEventArgs e)
		{
			StackLayout sl = ((StackLayout)(sender as DropGestureRecognizer).Parent);
			sl.BackgroundColor = Color.LightGray;
		}

		void DropGestureRecognizer_Drop(System.Object sender, Xamarin.Forms.DropEventArgs e)
		{
			StackLayout sl = ((StackLayout)(sender as DropGestureRecognizer).Parent);
			sl.BackgroundColor = Color.LightGray;

			// add new box to the stack
			var btn = new Button();
			btn.BackgroundColor = (Color)e.Data.Properties["Color"];
			sl.Children.Add(btn);

			btn.Clicked += ButtonRemove_Clicked;
		}

		void Button_Clicked(System.Object sender, System.EventArgs e)
		{
			Button btn = (Button)sender;
			var i = Int32.Parse(btn.Text);
			btn.Text = (++i).ToString();
		}

		void DragGestureRecognizer_DragStarting(System.Object sender, Xamarin.Forms.DragStartingEventArgs e)
		{
			Debug.WriteLine("started drag");
			Button btn = (sender as Element).Parent as Button;
			e.Data.Properties.Add("Color", btn.BackgroundColor);
		}

		void ButtonRemove_Clicked(System.Object sender, System.EventArgs e)
		{
			Button btn = (Button)sender;

			StackLayout sl = btn.Parent as StackLayout;
			sl.Children.Remove(btn);
		}
#endif
	}
}
using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Maui;
using AndroidX.AppCompat.App;
using Google.Android.Material.Snackbar;
using AndroidX.CoordinatorLayout.Widget;
using AndroidX.Core.Widget;

namespace Sample.Droid {
	[Activity (Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
	public class MainActivity : AppCompatActivity {
		//NestedScrollView page;
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			Xamarin.Essentials.Platform.Init (this, savedInstanceState);
			SetContentView (Resource.Layout.activity_main);

			AndroidX.AppCompat.Widget.Toolbar toolbar = FindViewById<AndroidX.AppCompat.Widget.Toolbar> (Resource.Id.toolbar);
			SetSupportActionBar (toolbar);

			var rootView = FindViewById<ViewGroup>(Resource.Id.rootLayout);

			//page = FindViewById<NestedScrollView> (Resource.Id.Page);
			//FloatingActionButton fab = FindViewById<FloatingActionButton> (Resource.Id.fab);
			//fab.Click += FabOnClick;
			var app = new MyApp ();

			var nativeView = (app.MainPage as IPage).ToNative(this);
			rootView.AddView(nativeView);
			//Add ((app.MainPage as ContentPage).Content);
		}

		//void Add(params IView[] views)
		//{
		//	foreach (var v in views)
		//	{
		//		var view = v.ToNative(this);
		//		page.AddView(view, new ViewGroup.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent));
		//	}
		//}

		/*public override bool OnCreateOptionsMenu (IMenu menu)
		{
			MenuInflater.Inflate (Resource.Menu.menu_main, menu);
			return true;
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			int id = item.ItemId;
			if (id == Resource.Id.action_settings) {
				return true;
			}

			return base.OnOptionsItemSelected (item);
		}*/

		public override void OnRequestPermissionsResult (int requestCode, string [] permissions, [GeneratedEnum] Android.Content.PM.Permission [] grantResults)
		{
			Xamarin.Essentials.Platform.OnRequestPermissionsResult (requestCode, permissions, grantResults);

			base.OnRequestPermissionsResult (requestCode, permissions, grantResults);
		}
	}
}


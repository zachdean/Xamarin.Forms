using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using System.Maui;
using System.Maui.Platform;

namespace Sample.Droid
{
	[Activity(
		Label = "@string/app_name",
		Theme = "@style/AppTheme.NoActionBar",
		MainLauncher = true)]
	public class MainActivity : AppCompatActivity
	{
		LinearLayout _stack;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			Xamarin.Essentials.Platform.Init(this, savedInstanceState);

			SetContentView(Resource.Layout.activity_main);

			Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
			SetSupportActionBar(toolbar);

			_stack = FindViewById<LinearLayout>(Resource.Id.Stack);

			var app = new MyApp();

			Add(app.TestActivityIndicator);
			Add(app.TestButton);
			Add(app.TestEllipse);
			Add(app.TestEntry);
			Add(app.TestLabel);
			Add(app.TestLine);
			Add(app.TestPath);
			Add(app.TestPolygon);
			Add(app.TestPolyline);
			Add(app.TestProgressBar);
			Add(app.TestRectangle);
			Add(app.TestSlider);
			Add(app.TestStepper);
			Add(app.TestSwitch);
		}

		void Add(params IView[] views)
		{
			foreach (var view in views)
			{
				var nativeView = view.ToNative(this);
				_stack.AddView(nativeView, new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent));
			}
		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
		{
			Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}
	}
}
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Core.Widget;
using Xamarin.Platform;

namespace Sample.Droid
{
	[Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
	public class MainActivity : AppCompatActivity
	{
		ViewGroup _page;


		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			Xamarin.Essentials.Platform.Init(this, savedInstanceState);

			SetContentView(Resource.Layout.activity_main);

			AndroidX.AppCompat.Widget.Toolbar toolbar = FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.toolbar);
			SetSupportActionBar(toolbar);

			_page = FindViewById<ViewGroup>(Resource.Id.pageLayout);

#pragma warning disable CS0618 // Type or member is obsolete
			Xamarin.Forms.Forms.Context = this;
#pragma warning restore CS0618 // Type or member is obsolete
			var app = new MyApp();
			Add(
				new Xamarin.Forms.Label() { Text = "Forms Button" },
				new Entry(),
				new Entry(),
				new Entry(),
				new Entry(),
				new Entry(),
				new Xamarin.Forms.Button() { Text = "Forms Button" }
			);

			//Randomize();
		}
				
		void Add(params IView[] views)
		{
			int y = 100;
			var size = new Android.Graphics.Point();
			WindowManager.DefaultDisplay.GetSize(size);

			foreach (var view in views)
			{
				var nativeView = view.ToNative(this);
				var data = view.Handler.GetDesiredSize(this.FromPixels(size.X), 100).Request;

				var layoutParams = new FrameLayout.LayoutParams((int)this.ToPixels(data.Width), (int)this.ToPixels(data.Height));

				layoutParams.TopMargin = (int)this.ToPixels(y);

				_page.AddView(nativeView, layoutParams);
				int viewWidth = (int)data.Width;
				if (viewWidth < 100)
					viewWidth = 100;

				var rect = new Xamarin.Forms.Rectangle(50, y, viewWidth, data.Height);
				view.Arrange(rect);
				view.Handler.SetFrame(rect);
				y += (int)(data.Height + 5);
			}
		}

		static Java.Util.Timer timer = new Java.Util.Timer();
		async void Randomize()
		{
			int changesProcessed = 1;
			int previousPRocesseed = 1;
			long previousTick = 1;

			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();

			timer.ScheduleAtFixedRate(new UpdateClass(ProcessTextChanges), 0, 1000);


			var views = Enumerable.Range(0, 100)
				.Select(_ => new Xamarin.Forms.Label() { Text = "Message" })
				.ToList();

			var random = new Random();
			var size = new Android.Graphics.Point();
			WindowManager.DefaultDisplay.GetSize(size);
			var x = (int)this.FromPixels(size.X);
			var y = (int)this.FromPixels(size.Y);

			foreach (IView view in views)
			{
				var rect = new Xamarin.Forms.Rectangle(random.Next(0, x), random.Next(0, y), 60, 25);

				var layoutParams = new FrameLayout.LayoutParams(50, 50);

				layoutParams.LeftMargin = (int)this.ToPixels(rect.X);
				layoutParams.TopMargin = (int)this.ToPixels(rect.Y);
				_page.AddView(view.ToNative(this), layoutParams);
				view.Arrange(rect);
				view.Handler.SetFrame(rect);
			}


			while (true)
			{
				await Task.Delay(1);

				foreach (IView view in views)
				{
					var rect = new Xamarin.Forms.Rectangle(random.Next(0, x), random.Next(0, y), 60, 25);

					var layoutParams = (view.Handler.NativeView as View).LayoutParameters as FrameLayout.LayoutParams;

					layoutParams.LeftMargin = (int)this.ToPixels(rect.X);
					layoutParams.TopMargin = (int)this.ToPixels(rect.Y);

					view.InvalidateArrange();
					view.Arrange(rect);
					view.Handler.SetFrame(rect);
					changesProcessed++;
				}
			}

			void ProcessTextChanges()
			{
				this.RunOnUiThread(() =>
				{
					double changes = changesProcessed - previousPRocesseed;
					double timePassed = (stopwatch.ElapsedMilliseconds - previousTick) / 1000d;
					var changesPerSecond = changes / timePassed;
					SupportActionBar.Title = $"{changesPerSecond}/sec";
					changesProcessed = 1;
					previousPRocesseed = changesProcessed;
					previousTick = stopwatch.ElapsedMilliseconds;
				});
			}
		}


		public class UpdateClass : Java.Util.TimerTask
		{
			readonly Action _action;

			public UpdateClass(Action action)
			{
				_action = action;
			}

			public override void Run()
			{
				_action?.Invoke();
			}
		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
		{
			Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}
	}
}
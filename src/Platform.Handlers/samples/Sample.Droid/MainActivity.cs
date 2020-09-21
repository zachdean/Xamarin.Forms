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
			//Add(app.CreateViews());
			Randomize();


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
				.Select(_ => new Xamarin.Forms.Label() { Text = "MAUI" })
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

			int max = views.Count;
			int i = 0;
			while (true)
			{
				var now = stopwatch.ElapsedMilliseconds;
				while (stopwatch.ElapsedMilliseconds - now < 16)
				{
					var caputeredi = i;

					this.RunOnUiThread(() =>
					{
						var view = (IView)views[i];
						if (view is Xamarin.Forms.Label label)
							label.TextColor = new Xamarin.Forms.Color(random.Next(128, 255) / 255d, random.Next(128, 255) / 255d, random.Next(128, 255) / 255d);
						var rect = new Xamarin.Forms.Rectangle(random.Next(0, x), random.Next(0, y), 60, 25);

						var layoutParams = (view.Handler.NativeView as View).LayoutParameters as FrameLayout.LayoutParams;

						layoutParams.LeftMargin = (int)this.ToPixels(rect.X);
						layoutParams.TopMargin = (int)this.ToPixels(rect.Y);

						(view.Handler.NativeView as Android.Views.View).Rotation = random.Next(0, 360);
						view.InvalidateArrange();
						view.Arrange(rect);
						view.Handler.SetFrame(rect);
						changesProcessed++;

					});
					i++;
					if (i >= max)
						i = 0;
				}

				await Task.Delay(1);
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
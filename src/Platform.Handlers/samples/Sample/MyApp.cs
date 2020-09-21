using Xamarin.Platform;
using Xamarin.Platform.Handlers;
using Sample.ReactiveControl;
using RegistrarHandlers = Xamarin.Platform.Registrar;
using System;
#if MONOANDROID
using Android.Widget;
#endif

namespace Sample
{
	public class MyApp : IApp
	{
		public MyApp()
		{
#if MONOANDROID
			RegistrarHandlers.Handlers.Register<ReactiveLabel, LabelHandler>();
			RegistrarHandlers.Handlers.Register<Xamarin.Forms.Label, LabelHandler>();
			RegistrarHandlers.Handlers.Register<CustomLabel, LabelHandler>();
			//RegistrarHandlers.Handlers.Register<Xamarin.Forms.Label, Xamarin.Forms.Platform.Android.LabelRenderer>();
			RegistrarHandlers.Handlers.Register<Xamarin.Forms.Button, Xamarin.Forms.Platform.Android.AppCompat.ButtonRenderer>();
#else
			RegistrarHandlers.Handlers.Register<Button, ButtonHandler>();
#endif
			RegistrarHandlers.Handlers.Register<Entry, EntryHandler>();

			RegisterMappers();
		}

		public IView[] CreateViews()
		{
			return new IView[] {
				new Xamarin.Forms.Label() { Text = "Forms Label", TextColor = Xamarin.Forms.Color.Purple  },
				new ReactiveLabel() { Text = "Reactive Label", TextColor = Xamarin.Forms.Color.Purple },
				new Entry(){ Text = "Entry 1", TextColor = Xamarin.Forms.Color.Purple  },
				new Entry(){ Text = "Entry 2", TextColor = Xamarin.Forms.Color.Purple  },
				new Entry(){ Text = "Entry 3", TextColor = Xamarin.Forms.Color.Purple  },
				new Entry(){ Text = "Entry 4", TextColor = Xamarin.Forms.Color.Purple  },
				new Entry(){ Text = "Entry 5", TextColor = Xamarin.Forms.Color.Purple  },
				new CustomLabel(){ Text = "Custom Label", TextColor = Xamarin.Forms.Color.Green},
				new Xamarin.Forms.Button() { Text = "Forms Button", TextColor = Xamarin.Forms.Color.Purple  }
			};
		}

		public IView CreateView()
		{
			return new Entry { Placeholder = "Placeholder" };
		}

		private void RegisterMappers()
		{
			ViewHandler.ViewMapper[nameof(IView.BackgroundColor)] = (handler, view) =>
			{

#if MONOANDROID
				if (view is CustomLabel)
					(handler.NativeView as Android.Views.View).SetBackgroundColor(Xamarin.Forms.Color.Purple.ToNative());
				else
#endif
					ViewHandler.MapBackgroundColor(handler, view);
			};
		}


		public class CustomLabel : Xamarin.Forms.Label
		{

		}

		//		public IView[] CreateViews()
		//		{
		//#if MONOANDROID

		//			ViewHandler.ViewMapper[nameof(IView.BackgroundColor)] = (handler, view) =>
		//			{
		//				(handler.NativeView as Android.Views.View).SetBackgroundColor(Xamarin.Forms.Color.Purple.ToNative());
		//			};

		//			var textView = crossPlatformLabel.Handler.NativeView as TextView;

		//#endif
		//			return new IView[] {
		//						new Xamarin.Forms.Label() { Text = "Forms Label" },
		//						new Entry(),
		//						new Entry(),
		//						new Entry(),
		//						new Entry(),
		//						new Entry(),
		//						new Xamarin.Forms.Button() { Text = "Forms Button" }
		//					};
		//		}

	}
}
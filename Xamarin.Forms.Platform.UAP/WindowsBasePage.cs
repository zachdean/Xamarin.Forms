using System;
using System.ComponentModel;
using Windows.ApplicationModel;

namespace Xamarin.Forms.Platform.UWP
{
	public abstract class WindowsBasePage : Windows.UI.Xaml.Controls.Page
	{

		Application _application;

		public WindowsBasePage()
		{
			if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
			{
				Windows.UI.Xaml.Application.Current.Suspending += OnApplicationSuspending;
				Windows.UI.Xaml.Application.Current.Resuming += OnApplicationResuming;
			}
		}

		protected Platform Platform { get; private set; }

		protected abstract Platform CreatePlatform();

		protected void LoadApplication(Application application)
		{
			if (application == null)
				throw new ArgumentNullException("application");

			_application = application;
			RegisterDispatcher(_application);
			Application.SetCurrentApplication(application);
			Platform = CreatePlatform();
			Platform.SetPage(_application.MainPage);
			application.PropertyChanged += OnApplicationPropertyChanged;

			_application.SendStart();
		}

		void RegisterDispatcher(Application application)
		{
			Forms.Dispatchers.AddOrUpdate(application.WindowId, this.Dispatcher, (key, oldValue) => this.Dispatcher);
		}

		void OnApplicationPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "MainPage")
				Platform.SetPage(_application.MainPage);
		}

		void OnApplicationResuming(object sender, object e)
		{
			_application.SendResume();
		}

		async void OnApplicationSuspending(object sender, SuspendingEventArgs e)
		{
			SuspendingDeferral deferral = e.SuspendingOperation.GetDeferral();
			try
			{
				await _application.SendSleepAsync();
			}
			finally
			{
				deferral.Complete();
			}
		}
	}
}
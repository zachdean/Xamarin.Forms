using Xamarin.Platform.Handlers;
using RegistrarHandlers = Xamarin.Platform.Registrar;

namespace Sample
{
	public class Platform
	{
		static bool HasInit;

		public static void Init()
		{
			if (HasInit)
				return;

			HasInit = true;
#if MONOANDROID

			RegistrarHandlers.Handlers.Register<Xamarin.Forms.Label, LabelHandler>();
			//RegistrarHandlers.Handlers.Register<Xamarin.Forms.Label, Xamarin.Forms.Platform.Android.LabelRenderer>();
			RegistrarHandlers.Handlers.Register<Xamarin.Forms.Button, Xamarin.Forms.Platform.Android.AppCompat.ButtonRenderer>();
#else
			RegistrarHandlers.Handlers.Register<Button, ButtonHandler>();
#endif
			RegistrarHandlers.Handlers.Register<Entry, EntryHandler>();
		}
	}
}
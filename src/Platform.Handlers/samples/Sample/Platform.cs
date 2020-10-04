using Xamarin.Platform.Handlers;
using Xamarin.Platform.Handlers.Image;
using RegistrarHandlers = Xamarin.Platform.Registrar;

namespace Sample
{
	public class Platform
	{
		static bool HasInit;

		public static void Init(bool useForms = true)
		{
			if (HasInit)
				return;

			HasInit = true;

			if (useForms)
				RegisterForms();
			else
				RegisterPoco();
		}

		static void RegisterForms()
		{
			RegistrarHandlers.Handlers.Register<Xamarin.Forms.Button, ButtonHandler>();
			RegistrarHandlers.Handlers.Register<Xamarin.Forms.Image, ImageHandler>();
		}

		static void RegisterPoco()
		{
			RegistrarHandlers.Handlers.Register<Button, ButtonHandler>();
			RegistrarHandlers.Handlers.Register<Image, ImageHandler>();
		}

	}
}
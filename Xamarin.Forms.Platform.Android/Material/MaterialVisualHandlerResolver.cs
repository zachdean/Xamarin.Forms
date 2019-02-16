using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Platform.Android.Material
{
	public class MaterialVisualHandlerResolver : IVisualHandlerResolver
	{
		public static MaterialVisualHandlerResolver Instance { get; private set; } = new MaterialVisualHandlerResolver();

		private MaterialVisualHandlerResolver() { }

		public Type VisualType => typeof(VisualRendererMarker.Material);

		public object GetHandler(VisualRendererFactoryArgs args)
		{
			if(args.Source is Button button)
			{
				var style = Xamarin.Forms.Material.Button.GetStyle(button);
				int themeId = Resource.Style.XamarinFormsMaterialTheme;
				switch (style)
				{
					case Xamarin.Forms.Material.Style.Outline:
						themeId = Resource.Style.XamarinFormsMaterialOutlinedButtonTheme;
						break;
					case Xamarin.Forms.Material.Style.Text:
						themeId = Resource.Style.XamarinFormsMaterialTextButtonTheme;
						break;
				}

				if (args.HandlerType.GetTypeInfo().DeclaredConstructors.Any(info => info.GetParameters().Length == 2))
				{
					return Activator.CreateInstance(args.HandlerType, args.Args[0], themeId);
				}
			}

			return null;
		}
	}
}
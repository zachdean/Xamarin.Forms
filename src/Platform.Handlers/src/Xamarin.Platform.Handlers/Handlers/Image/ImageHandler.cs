using System;
using System.Collections.Generic;
using System.Text;
#if __IOS__
using NativeView = UIKit.UIImageView;
#elif __MACOS__
using NativeView = AppKit.NSImageView;
#elif MONOANDROID
using NativeView = Android.Widget.ImageView;
#elif NETCOREAPP
using NativeView = System.Windows.Controls.TextBlock;
#elif NETSTANDARD
using NativeView = System.Object;
#endif

namespace Xamarin.Platform.Handlers.Image
{
	public class ImageHandler : AbstractViewHandler<IImage, NativeView>
	{

		public static PropertyMapper<IImage, ImageHandler> ImageMapper = new PropertyMapper<IImage, ImageHandler>(ViewHandler.ViewMapper)
		{
			[nameof(IImage.Aspect)] = MapAspect,
			[nameof(IImage.Source)] = MapSource,
			[nameof(IImage.IsOpaque)] = MapIsOpaque
		};

		public static void MapSource(ImageHandler handler, IImage image)
		{
			ViewHandler.CheckParameters(handler, image);
			handler.TypedNativeView?.UpdateSource(image);
		}

		public static void MapAspect(ImageHandler handler, IImage image)
		{
			ViewHandler.CheckParameters(handler, image);
			handler.TypedNativeView?.UpdateAspect(image);
		}

		public static void MapIsOpaque(ImageHandler handler, IImage image)
		{
			ViewHandler.CheckParameters(handler, image);
			handler.TypedNativeView?.UpdateOpaque(image);
		}


#if MONOANDROID
		protected override NativeView CreateView() => new NativeView(this.Context);
#else
		protected override NativeView CreateView() => new NativeView();
#endif
		public ImageHandler() : base(ImageMapper)
		{

		}

		public ImageHandler(PropertyMapper mapper) : base(mapper ?? ImageMapper)
		{

		}
	}
}

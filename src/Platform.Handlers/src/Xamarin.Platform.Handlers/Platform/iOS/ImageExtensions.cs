using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using Xamarin.Forms;

namespace Xamarin.Platform
{
	public static class ImageExtensions
	{
		public static async void UpdateSource(this UIImageView imageView, IImage image)
		{
			try
			{
				//image.IsLoading = true;

				if (imageView.Image?.Images != null && imageView.Image.Images.Length > 1)
				{
					//maybe clear old images
					//renderer.SetImage(null);
				}

				float scale = (float)UIScreen.MainScreen.Scale;

				var uiImage = await LoadImageAsync(image.Source, scale: scale);

				imageView.Image = uiImage;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(nameof(IImage), "Error loading image: {0}", ex);
			}
			finally
			{
				//image.IsLoading = false;
			}
		}

		public static void UpdateAspect(this UIImageView imageView, IImage image)
		{
			imageView.ContentMode = image.Aspect.ToUIViewContentMode();
		}

		public static void UpdateOpaque(this UIImageView imageView, IImage image)
		{
			imageView.Opaque = image.IsOpaque;
		}

		public static UIViewContentMode ToUIViewContentMode(this Aspect aspect)
		{
			switch (aspect)
			{
				case Aspect.AspectFill:
					return UIViewContentMode.ScaleAspectFill;
				case Aspect.Fill:
					return UIViewContentMode.ScaleToFill;
				case Aspect.AspectFit:
				default:
					return UIViewContentMode.ScaleAspectFit;
			}
		}

		static async Task<UIImage?> LoadImageAsync(IImageSource imagesource, CancellationToken cancelationToken = default, float scale = 1f)
		{
			UIImage? uiImage = null;

			if (imagesource is IStreamImageSource streamImageSource)
			{
				using var streamImage = await streamImageSource.GetStreamSourceAsync(cancelationToken).ConfigureAwait(false);
				if (streamImage != null)
					uiImage = UIImage.LoadFromData(NSData.FromStream(streamImage), scale);
			}

			if (imagesource is IFileImageSource fileImageSource)
			{
				var file = fileImageSource.File;
				uiImage = File.Exists(file) ? new UIImage(file) : UIImage.FromBundle(file);
			}

			if (uiImage == null)
				System.Diagnostics.Debug.WriteLine(nameof(IImageSource), "Could not load image: {0}", imagesource);

			return uiImage;
		}
	}
}

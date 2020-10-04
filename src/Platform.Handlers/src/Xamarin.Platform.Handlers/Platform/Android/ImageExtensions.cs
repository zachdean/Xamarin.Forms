using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Android.Widget;
using Xamarin.Forms;


namespace Xamarin.Platform
{
	public static class ImageExtensions
	{
		static ImageView.ScaleType? s_fill;
		static ImageView.ScaleType? s_aspectFill;
		static ImageView.ScaleType? s_aspectFit;

		public static async void UpdateSource(this ImageView imageView, IImage image)
		{
			imageView.SetImageResource(global::Android.Resource.Color.Transparent);

			try
			{
				//image.IsLoading = true;
				if (imageView.Context == null)
				{
					throw new InvalidOperationException($"Context can't be null");
				}

				Bitmap? bitmap = await LoadImageAsync(imageView.Context, image.Source);

				imageView.SetImageBitmap(bitmap);
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

		public static void UpdateAspect(this ImageView imageView, IImage image)
		{
			var type = image.Aspect.ToScaleType();
			imageView.SetScaleType(type);
		}

		public static void UpdateOpaque(this ImageView imageView, IImage image)
		{
			//imageView.Opaque = image.IsOpaque;
		}

		public static ImageView.ScaleType ToScaleType(this Aspect aspect)
		{
#pragma warning disable CS8603 // Possible null reference return.
			switch (aspect)
			{
				case Aspect.Fill:
					return s_fill ??= ImageView.ScaleType.FitXy;
				case Aspect.AspectFill:
					return s_aspectFill ??= ImageView.ScaleType.CenterCrop;
				default:
				case Aspect.AspectFit:
					return s_aspectFit ??= ImageView.ScaleType.FitCenter;
			}
#pragma warning restore CS8603
		}

		static async Task<Bitmap?> LoadImageAsync(Context context, IImageSource imagesource, CancellationToken cancelationToken = default)
		{
			Bitmap? bitmap = null;
			if (imagesource == null)
				return bitmap;

			if (imagesource is IStreamImageSource streamImageSource)
			{
				using Stream imageStream = await streamImageSource.GetStreamSourceAsync(cancelationToken).ConfigureAwait(false);
				bitmap = await BitmapFactory.DecodeStreamAsync(imageStream).ConfigureAwait(false);
			}

			if (imagesource is IFileImageSource fileImageSource)
			{
				if (context == null)
					return bitmap;
				string file = fileImageSource.File;
				if (File.Exists(file))
				{
					bitmap = await BitmapFactory.DecodeFileAsync(file).ConfigureAwait(false);
				}
				else
				{
					bitmap = context?.Resources?.GetBitmap(file, context);
				}
				if (bitmap == null)
				{
					System.Diagnostics.Debug.WriteLine(nameof(IFileImageSource), "Could not find image or image file was invalid: {0}", imagesource);
				}
			}

			if (bitmap == null)
			{
				System.Diagnostics.Debug.WriteLine(nameof(bitmap), "Could not retrieve image or image data was invalid: {0}", imagesource);
			}

			return bitmap;
		}
	}
}

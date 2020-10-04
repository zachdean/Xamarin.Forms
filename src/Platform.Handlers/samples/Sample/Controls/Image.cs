using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Platform;
namespace Sample
{
	public class Image : View, IImage
	{
		ImageSource _internalSource;
		public ImageSource Source
		{
			get { return _internalSource; }
			set { _internalSource = value; }
		}

		public Aspect Aspect { get; set; }

		public bool IsLoading { get; set; }

		public bool IsOpaque { get; set; }

		IImageSource IImage.Source => _internalSource;
	}

	public abstract class ImageSource : IImageSource
	{
		CancellationTokenSource _cancellationTokenSource;

		protected ImageSource()
		{
			CancellationTokenSource = new CancellationTokenSource();
		}

		public virtual bool IsEmpty => false;

		protected CancellationTokenSource CancellationTokenSource
		{
			get { return _cancellationTokenSource; }
			private set
			{
				if (_cancellationTokenSource == value)
					return;
				if (_cancellationTokenSource != null)
					_cancellationTokenSource.Cancel();
				_cancellationTokenSource = value;
			}
		}

		public bool IsLoading
		{
			get { return _cancellationTokenSource != null; }
		}

		public static implicit operator ImageSource(string source)
		{
			Uri uri;
			return Uri.TryCreate(source, UriKind.Absolute, out uri) && uri.Scheme != "file" ? FromUri(uri) as ImageSource : FromFile(source);
		}

		public static ImageSource FromResource(string resource, Type resolvingType)
		{
			return FromResource(resource, resolvingType.GetTypeInfo().Assembly);
		}

		public static ImageSource FromResource(string resource, Assembly sourceAssembly = null)
		{
#if NETSTANDARD2_0
			sourceAssembly = sourceAssembly ?? Assembly.GetCallingAssembly();
#else
			if (sourceAssembly == null)
			{
				MethodInfo callingAssemblyMethod = typeof(Assembly).GetTypeInfo().GetDeclaredMethod("GetCallingAssembly");
				if (callingAssemblyMethod != null)
				{
					sourceAssembly = (Assembly)callingAssemblyMethod.Invoke(null, new object[0]);
				}
				else
				{
					System.Diagnostics.Debug.WriteLine("Warning", "Can not find CallingAssembly, pass resolvingType to FromResource to ensure proper resolution");
					return null;
				}
			}
#endif
			return FromStream(() => sourceAssembly.GetManifestResourceStream(resource));
		}

		public static ImageSource FromStream(Func<Stream> stream)
		{
			return new StreamImageSource { Stream = token => Task.Run(stream, token) };
		}

		public static ImageSource FromStream(Func<CancellationToken, Task<Stream>> stream)
		{
			return new StreamImageSource { Stream = stream };
		}

		public static ImageSource FromFile(string file)
		{
			return new FileImageSource { File = file };
		}

		public static implicit operator ImageSource(Uri uri)
		{
			if (uri == null)
				return null;

			if (!uri.IsAbsoluteUri)
				throw new ArgumentException("uri is relative");
			return FromUri(uri) as ImageSource;
		}

		public static IImageSource FromUri(Uri uri)
		{
			if (!uri.IsAbsoluteUri)
				throw new ArgumentException("uri is relative");
			return new UriImageSource { Uri = uri };
		}

		public abstract Task<Stream> GetStreamSourceAsync(CancellationToken userToken = default);


		public Task<bool> Cancel()
		{
			throw new NotImplementedException();
		}
	}

	public sealed class UriImageSource : ImageSource
	{
		public Uri Uri { get; set; }

		public override async Task<Stream> GetStreamSourceAsync(CancellationToken userToken = default)
		{
			Stream stream;
			try
			{
				stream = await StreamExtensions.GetStreamAsync(Uri, userToken);
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex)
			{
				Xamarin.Forms.Internals.Log.Warning("Image Loading", $"Error getting stream for {Uri}: {ex}");
				throw;
			}

			return stream;
		}
	}

	public sealed class StreamImageSource : ImageSource
	{
		public Func<CancellationToken, Task<Stream>> Stream { get; set; }

		public override Task<Stream> GetStreamSourceAsync(CancellationToken userToken = default)
		{
			throw new NotImplementedException();
		}
	}

	public sealed class FileImageSource : ImageSource
	{
		public string File
		{
			get => throw new NotImplementedException(); set => throw new NotImplementedException();
		}

		public override Task<Stream> GetStreamSourceAsync(CancellationToken userToken = default)
		{
			throw new NotImplementedException();
		}
	}
}




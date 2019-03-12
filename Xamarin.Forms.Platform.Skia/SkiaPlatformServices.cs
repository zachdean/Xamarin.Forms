using SkiaSharp;
using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;
using System.Linq;

namespace Xamarin.Forms.Platform.Skia
{
	public class SkiaPlatformServices : IPlatformServices
	{
		public bool IsInvokeRequired => false;

		public string RuntimePlatform => "Android";

		public void BeginInvokeOnMainThread(Action action)
		{
			action();
		}

		public Ticker CreateTicker()
		{
			throw new NotImplementedException();
		}

		public Assembly[] GetAssemblies()
		{
			return AppDomain.CurrentDomain.GetAssemblies();
		}

		static readonly MD5CryptoServiceProvider s_checksum = new MD5CryptoServiceProvider();

		public string GetMD5Hash(string input)
		{
			var bytes = s_checksum.ComputeHash(Encoding.UTF8.GetBytes(input));
			var ret = new char[32];
			for (var i = 0; i < 16; i++)
			{
				ret[i * 2] = (char)Hex(bytes[i] >> 4);
				ret[i * 2 + 1] = (char)Hex(bytes[i] & 0xf);
			}
			return new string(ret);
		}

		static int Hex(int v)
		{
			if (v < 10)
				return '0' + v;
			return 'a' + v - 10;
		}

		public double GetNamedSize(NamedSize size, Type targetElementType, bool useOldSizes)
		{
			switch (size)
			{
				case NamedSize.Default:
					return 17;
				case NamedSize.Micro:
					return 12;
				case NamedSize.Small:
					return 14;
				case NamedSize.Medium:
					return 17;
				case NamedSize.Large:
					return 22;
				default:
					throw new ArgumentOutOfRangeException("size");
			}
		}

		HttpClient GetHttpClient()
		{
			var handler = new HttpClientHandler();
			return new HttpClient(handler);
		}

		public async Task<Stream> GetStreamAsync(Uri uri, CancellationToken cancellationToken)
		{
			using (var client = GetHttpClient())
			using (var response = await client.GetAsync(uri, cancellationToken))
			{
				if (!response.IsSuccessStatusCode)
				{
					Log.Warning("HTTP Request", $"Could not retrieve {uri}, status code {response.StatusCode}");
					return null;
				}
				return await response.Content.ReadAsStreamAsync();
			}
		}

		public IIsolatedStorageFile GetUserStoreForApplication()
		{
			return new _IsolatedStorageFile(IsolatedStorageFile.GetUserStoreForAssembly());
		}

		public void OpenUriAction(Uri uri)
		{
			
		}

		public void QuitApplication()
		{
			
		}

		public void StartTimer(TimeSpan interval, Func<bool> callback)
		{
			throw new NotImplementedException();
		}

		public SizeRequest GetNativeSize(VisualElement view, double widthConstraint, double heightConstraint)
		{
			SizeRequest? result = null;

			if (view is Button || view is Label || view is Entry || view is Editor || view is DatePicker || view is TimePicker || view is Picker)
			{
				string text = null;
				TextDrawingData drawingData = null;
				if (view is Button button)
				{
					text = button.Text;

					drawingData = new TextDrawingData(button);
				}
				else if (view is Label label)
				{
					text = label.Text;
					drawingData = new TextDrawingData(label);
				}
				else if (view is Entry entry)
				{
					text = entry.Text;
					drawingData = new TextDrawingData(entry);
				}
				else if (view is Editor editor)
				{
					text = editor.Text;
					drawingData = new TextDrawingData(editor);
				}
				else if (view is DatePicker datePicker)
				{
					text = datePicker.Date.ToString(datePicker.Format);
					drawingData = new TextDrawingData(datePicker);
				}
				else if (view is TimePicker timePicker)
				{
					text = timePicker.Time.ToString(timePicker.Format);
					drawingData = new TextDrawingData(timePicker);
				}
				else if (view is Picker picker)
				{
					text = picker.SelectedItem?.ToString();
					drawingData = new TextDrawingData(picker);
				}

				if (string.IsNullOrEmpty(text))
					text = " ";

				drawingData.Rect = new Rectangle(0, 0,
					double.IsPositiveInfinity(widthConstraint) ? float.MaxValue : widthConstraint,
					double.IsPositiveInfinity(heightConstraint) ? float.MaxValue : heightConstraint);

				if (view is Editor)
					drawingData.Rect = drawingData.Rect.Inflate(-8, 0);

				Forms.GetTextLayout(text, drawingData, true, out var lines);

				Size size;
				if (lines.Count > 0)
					size = new Size(lines.Max (l => l.Width), lines.Sum(l => l.Height));
				else
					size = new Size();

				if (view is Button)
					size += new Size(28, 20);

				if (view is Entry || view is Editor)
					size += new Size(16, 16);

				return new SizeRequest(size);
			}
			else if (view is Image image)
			{
				if (image.Source is UriImageSource uri)
				{
					var url = uri.Uri.AbsoluteUri;
					var localPath = uri.Uri.LocalPath;
					int multiplier = 1;
					if (localPath.EndsWith("@2x.png"))
					{
						multiplier = 2;
					}
					if (localPath.EndsWith("@3x.png"))
					{
						multiplier = 3;
					}
					var bitmap = ImageCache.TryGetValue(url);
					if (bitmap != null)
					{
						return new SizeRequest(new Size(bitmap.Width / multiplier, bitmap.Height / multiplier));
					}
				}
				if (image.Source is FileImageSource fileSource)
				{
					var s = ImageCache.FromFile(fileSource.File);
					var bitmap = s.bitmap;
					int multiplier = s.scale;
					if (bitmap != null)
					{
						return new SizeRequest(new Size(bitmap.Width / multiplier, bitmap.Height / multiplier));
					}
				}
				return new SizeRequest(new Size(10, 10));
			}
			else if (view is Stepper)
			{
				return new SizeRequest(new Size(81, 28));
			}
			else if (view is Switch)
			{
				return new SizeRequest(new Size(81, 28));
			}
			else if (view is ProgressBar)
			{
				return new SizeRequest(new Size(12, 6));
			}
			else if (view is Slider slider)
			{
				return new SizeRequest(new Size(18, 30));
			}
			else if (view is ActivityIndicator)
			{
				return new SizeRequest(new Size(81, 81));
			}
			else
				result = new SizeRequest(new Size(100, 100));

			if (result == null)
				throw new NotImplementedException();

			return result.Value;
		}

		public class _IsolatedStorageFile : IIsolatedStorageFile
		{
			readonly IsolatedStorageFile _isolatedStorageFile;

			public _IsolatedStorageFile(IsolatedStorageFile isolatedStorageFile)
			{
				_isolatedStorageFile = isolatedStorageFile;
			}

			public Task CreateDirectoryAsync(string path)
			{
				_isolatedStorageFile.CreateDirectory(path);
				return Task.FromResult(true);
			}

			public Task<bool> GetDirectoryExistsAsync(string path)
			{
				return Task.FromResult(_isolatedStorageFile.DirectoryExists(path));
			}

			public Task<bool> GetFileExistsAsync(string path)
			{
				return Task.FromResult(_isolatedStorageFile.FileExists(path));
			}

			public Task<DateTimeOffset> GetLastWriteTimeAsync(string path)
			{
				return Task.FromResult(_isolatedStorageFile.GetLastWriteTime(path));
			}

			public Task<Stream> OpenFileAsync(string path, FileMode mode, FileAccess access)
			{
				Stream stream = _isolatedStorageFile.OpenFile(path, mode, access);
				return Task.FromResult(stream);
			}

			public Task<Stream> OpenFileAsync(string path, FileMode mode, FileAccess access, FileShare share)
			{
				Stream stream = _isolatedStorageFile.OpenFile(path, mode, access, share);
				return Task.FromResult(stream);
			}
		}
	}
}

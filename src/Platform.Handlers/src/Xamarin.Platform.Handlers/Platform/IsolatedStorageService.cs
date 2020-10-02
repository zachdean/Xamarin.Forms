using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Platform
{
	public class IsolatedStorageService : IIsolatedStorageFile
	{
		readonly IsolatedStorageFile _isolatedStorageFile;

		public IsolatedStorageService()
		{
			_isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication();
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

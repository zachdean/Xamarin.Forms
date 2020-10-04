using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.Platform
{

	public interface IImageSource
	{
		bool IsEmpty { get; }

	}

	public interface IFileImageSource : IImageSource
	{
		string File { get; }
	}

	public interface IStreamImageSource : IImageSource
	{
		Task<Stream> GetStreamSourceAsync(CancellationToken userToken = default);
	}

	public interface IImage : IView
	{
		IImageSource Source { get; }

		Aspect Aspect { get; }

		bool IsLoading { get; }

		//What does this map on Android?
		bool IsOpaque { get; }
	}
}

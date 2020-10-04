using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Platform;

namespace Xamarin.Forms
{
	public partial class Image : IImage
	{
		IImageSource IImage.Source => Source;
	}
}

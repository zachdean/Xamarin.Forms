using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Forms
{
	[TypeConverter(typeof(FileImageSourceConverter))]
	public sealed class FileImageSource : ImageSource, Xamarin.Platform.IFileImageSource
	{
		public static readonly BindableProperty FileProperty = BindableProperty.Create("File", typeof(string), typeof(FileImageSource), default(string));

		public override bool IsEmpty => string.IsNullOrEmpty(File);

		public string File
		{
			get { return (string)GetValue(FileProperty); }
			set { SetValue(FileProperty, value); }
		}

		public override Task<bool> Cancel() => Task.FromResult(false);

		public override string ToString() => $"File: {File}";

		public static implicit operator FileImageSource(string file)
		{
			return (FileImageSource)FromFile(file);
		}

		public static implicit operator string(FileImageSource file)
		{
			return file?.File;
		}

		protected override void OnPropertyChanged(string propertyName = null)
		{
			if (propertyName == FileProperty.PropertyName)
				OnSourceChanged();
			base.OnPropertyChanged(propertyName);
		}
	}
}
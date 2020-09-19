using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Platform;

namespace Xamarin.Forms
{
	public partial class Label : ILabel
	{
		public string UpdateTransformedText(string source, TextTransform textTransform)
		{
			return TextTransformUtilites.GetTransformedText(source, textTransform);
		}
	}
}

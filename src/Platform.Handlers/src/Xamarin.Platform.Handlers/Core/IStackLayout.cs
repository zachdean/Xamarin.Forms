using System.Collections.Generic;
using Xamarin.Forms;

namespace Xamarin.Platform
{
	public interface IStackLayout : ILayout
	{
		int Spacing { get; }
	}
}
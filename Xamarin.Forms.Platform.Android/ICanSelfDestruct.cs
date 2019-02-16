using System;

namespace Xamarin.Forms.Platform.Android
{
	internal interface ICanSelfDestruct
	{
		event EventHandler SelfDestruct;
	}
}
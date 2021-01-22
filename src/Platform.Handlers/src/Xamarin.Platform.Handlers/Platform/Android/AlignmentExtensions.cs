using Xamarin.Forms;
using AGravityFlags = Android.Views.GravityFlags;
using ATextAlignment = Android.Views.TextAlignment;

namespace Xamarin.Platform
{
	public static class AlignmentExtensions
	{
		public static ATextAlignment ToTextAlignment(this TextAlignment alignment)
		{
			switch (alignment)
			{
				case TextAlignment.Center:
					return ATextAlignment.Center;
				case TextAlignment.End:
					return ATextAlignment.ViewEnd;
				default:
					return ATextAlignment.ViewStart;
			}
		}

		public static AGravityFlags ToHorizontalGravityFlags(this TextAlignment alignment)
		{
			switch (alignment)
			{
				case TextAlignment.Center:
					return AGravityFlags.CenterHorizontal;
				case TextAlignment.End:
					return AGravityFlags.End;
				default:
					return AGravityFlags.Start;
			}
		}

		public static AGravityFlags ToVerticalGravityFlags(this TextAlignment alignment)
		{
			switch (alignment)
			{
				case TextAlignment.Start:
					return AGravityFlags.Top;
				case TextAlignment.End:
					return AGravityFlags.Bottom;
				default:
					return AGravityFlags.CenterVertical;
			}
		}
	}
}
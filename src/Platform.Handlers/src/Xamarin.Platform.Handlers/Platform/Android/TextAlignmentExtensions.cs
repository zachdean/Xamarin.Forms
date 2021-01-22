using Android.OS;
using Android.Widget;
using Xamarin.Forms;
using AGravityFlags = Android.Views.GravityFlags;

namespace Xamarin.Platform
{
	public static class TextAlignmentExtensions
	{
		public static void UpdateHorizontalAlignment(this EditText view, TextAlignment alignment, bool hasRtlSupport, AGravityFlags orMask = AGravityFlags.NoGravity)
		{
			if ((int)Build.VERSION.SdkInt < 17 || !hasRtlSupport)
				view.Gravity = alignment.ToHorizontalGravityFlags() | orMask;
			else
				view.TextAlignment = alignment.ToTextAlignment();
		}

		public static void UpdateVerticalAlignment(this EditText view, TextAlignment alignment, AGravityFlags orMask = AGravityFlags.NoGravity)
		{
			view.Gravity = alignment.ToVerticalGravityFlags() | orMask;
		}

		public static void UpdateTextAlignment(this EditText view, TextAlignment horizontal, TextAlignment vertical)
		{
			if ((int)Build.VERSION.SdkInt < 17 || (view.Context != null && !view.Context.HasRtlSupport()))
			{
				view.Gravity = vertical.ToVerticalGravityFlags() | horizontal.ToHorizontalGravityFlags();
			}
			else
			{
				view.TextAlignment = horizontal.ToTextAlignment();
				view.Gravity = vertical.ToVerticalGravityFlags();
			}
		}
	}
}
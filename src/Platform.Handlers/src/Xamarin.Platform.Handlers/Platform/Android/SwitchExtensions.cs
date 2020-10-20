using Android.Graphics.Drawables;
using Xamarin.Forms;
using ASwitch = Android.Widget.Switch;

namespace Xamarin.Platform
{
	public static class SwitchExtensions
	{
		static bool ChangedThumbColor;

		public static void UpdateIsToggled(this ASwitch aSwitch, ISwitch view)
		{
			aSwitch.Checked = view.IsToggled;
		}

		public static void UpdateOnColor(this ASwitch aSwitch, ISwitch view)
		{
			aSwitch.UpdateOnColor(view, null);
		}

		public static void UpdateOnColor(this ASwitch aSwitch, ISwitch view, Drawable? defaultTrackDrawable)
		{
			if (defaultTrackDrawable == null)
				defaultTrackDrawable = aSwitch.TrackDrawable;

			if (aSwitch.Checked)
			{
				var onColor = view.OnColor;

				if (onColor.IsDefault)
				{
					aSwitch.TrackDrawable = defaultTrackDrawable;
				}
				else
				{
					aSwitch.TrackDrawable?.SetColorFilter(onColor.ToNative(), FilterMode.Multiply);
				}
			}
			else
			{
				aSwitch.TrackDrawable?.ClearColorFilter();
			}
		}

		public static void UpdateThumbColor(this ASwitch aSwitch, ISwitch view)
		{
			var thumbColor = view.ThumbColor;

			if (!thumbColor.IsDefault)
			{
				aSwitch.ThumbDrawable?.SetColorFilter(thumbColor, FilterMode.Multiply);
				ChangedThumbColor = true;
			}
			else
			{
				if (ChangedThumbColor)
				{
					aSwitch.ThumbDrawable?.ClearColorFilter();
					ChangedThumbColor = false;
				}
			}

			aSwitch.ThumbDrawable?.SetColorFilter(thumbColor, FilterMode.Multiply);
		}
	}
}

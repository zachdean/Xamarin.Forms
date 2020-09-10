using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.Content;
using Android.Util;
using Android.Views;

namespace System.Maui.Platform
{
	internal class BorderBackgroundManager : IDisposable
	{
		Drawable _defaultDrawable;
		BorderDrawable _backgroundDrawable;
		RippleDrawable _rippleDrawable;
		bool _drawableEnabled;

		IViewHandler _Handler;
		IBorder _border;
		readonly View _nativeView;

		readonly bool _drawOutlineWithBackground;

		public BorderBackgroundManager(IViewHandler Handler, IBorder border) : this(Handler, border, true)
		{
		}

		public BorderBackgroundManager(IViewHandler Handler, IBorder border, bool drawOutlineWithBackground)
		{
			_Handler = Handler;
			_nativeView = (View)_Handler.NativeView;
			_border = border;
			_drawOutlineWithBackground = drawOutlineWithBackground;
		}

		public static Color ColorButtonNormalOverride { get; set; }

		public void UpdateDrawable()
		{
			if (_border == null || _Handler == null)
				return;

			bool cornerRadiusIsDefault = _border.CornerRadius == BorderDrawable.DefaultCornerRadius;
			bool backgroundColorIsDefault = _border.BackgroundColor == Color.Default;
			bool borderColorIsDefault = _border.BorderColor == Color.Default;
			bool borderWidthIsDefault = _border.BorderWidth == 0.0d;

			if (backgroundColorIsDefault
				&& cornerRadiusIsDefault
				&& borderColorIsDefault
				&& borderWidthIsDefault)
			{
				if (!_drawableEnabled)
					return;

				if (_defaultDrawable != null)
					_nativeView.SetBackground(_defaultDrawable);

				_drawableEnabled = false;
				Reset();
			}
			else
			{
				if (_backgroundDrawable == null)
					_backgroundDrawable = new BorderDrawable(_nativeView.Context.ToPixels, GetButtonColor(_nativeView.Context), _drawOutlineWithBackground);

				_backgroundDrawable.Border = _border;

				if (_drawableEnabled)
					return;

				if (_defaultDrawable == null)
					_defaultDrawable = _nativeView.Background;

				if (!backgroundColorIsDefault || _drawOutlineWithBackground)
				{
					if (NativeVersion.IsAtLeast(21))
					{
						var rippleColor = _backgroundDrawable.PressedBackgroundColor.ToNative();
						_rippleDrawable = new RippleDrawable(ColorStateList.ValueOf(rippleColor), _backgroundDrawable, null);
						_nativeView.SetBackground(_rippleDrawable);
					}
					else
						_nativeView.SetBackground(_backgroundDrawable);
				}

				_drawableEnabled = true;
			}

			_nativeView.Invalidate();
		}

		public void Reset()
		{
			if (_drawableEnabled)
			{
				_drawableEnabled = false;
				_backgroundDrawable?.Reset();
				_backgroundDrawable = null;
				_rippleDrawable = null;
			}
		}

		public void Dispose()
		{
			_backgroundDrawable?.Dispose();
			_backgroundDrawable = null;
			_defaultDrawable?.Dispose();
			_defaultDrawable = null;
			_rippleDrawable?.Dispose();
			_rippleDrawable = null;

			_border = null;

			if (_Handler != null)
				_Handler = null;
		}

		Color GetButtonColor(Context context)
		{
			Color rc = ColorButtonNormalOverride;

			if (ColorButtonNormalOverride == Color.Default)
			{
				using (var value = new TypedValue())
				{
					if (context.Theme.ResolveAttribute(global::Android.Resource.Attribute.ColorButtonNormal, value, true) && NativeVersion.IsAtLeast(21)) // Android 5.0+
					{
						rc = Color.FromUint((uint)value.Data);
					}
					else if (context.Theme.ResolveAttribute(context.Resources.GetIdentifier("colorButtonNormal", "attr", context.PackageName), value, true))  // < Android 5.0
					{
						rc = Color.FromUint((uint)value.Data);
					}
				}
			}

			return rc;
		}
	}
}
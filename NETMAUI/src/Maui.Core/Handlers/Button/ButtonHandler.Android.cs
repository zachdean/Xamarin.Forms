using Android.Graphics;
using Android.Util;
using Android.Views;
using static Android.Views.View;

#if __ANDROID_29__
using AndroidX.Core.View;
using AndroidX.AppCompat.Widget;
#else
using Android.Support.V4.View;
using Android.Support.V7.Widget;
#endif

namespace System.Maui.Platform
{
	public partial class ButtonHandler : AbstractViewHandler<IButton, AppCompatButton>
	{
		float _defaultFontSize;
		Typeface _defaultTypeface;
		Lazy<TextColorSwitcher> _textColorSwitcher;
		BorderBackgroundManager _backgroundTracker;

		protected override AppCompatButton CreateView()
		{
            var button = new AppCompatButton(Context)
            {
                SoundEffectsEnabled = false
            };

            button.Click += OnButtonClick;

			return button;
		}

		protected override void SetupDefaults()
		{
			_textColorSwitcher = new Lazy<TextColorSwitcher>(
				() => new TextColorSwitcher(TypedNativeView));

			_backgroundTracker = new BorderBackgroundManager(this, VirtualView);

			TypedNativeView.SoundEffectsEnabled = false;

			base.SetupDefaults();
		}

		protected override void DisposeView(AppCompatButton nativeView)
		{
			_textColorSwitcher = null;

			_backgroundTracker?.Dispose();
			_backgroundTracker = null;

			nativeView.Click -= OnButtonClick;

			base.DisposeView(nativeView);
		}

		public override SizeRequest GetDesiredSize(double wConstraint, double hConstraint)
		{
			var hint = TypedNativeView.Hint;
			bool setHint = TypedNativeView.LayoutParameters != null;

			if (!string.IsNullOrWhiteSpace(hint) && setHint)
			{
				TypedNativeView.Hint = string.Empty;
			}

			int widthConstraint = wConstraint == double.MaxValue ? int.MaxValue : (int)wConstraint;
			int heightConstraint = hConstraint == double.MaxValue ? int.MaxValue : (int)hConstraint;

			TypedNativeView.Measure(widthConstraint, heightConstraint);

			var previousHeight = TypedNativeView.MeasuredHeight;
			var previousWidth = TypedNativeView.MeasuredWidth;

			// If the measure of the view has changed then trigger a request for layout
			// If the measure hasn't changed then force a layout of the button
			if (previousHeight != View.MeasuredHeight || previousWidth != View.MeasuredWidth)
				View.RequestLayout();
			else
				View.ForceLayout();

			var result = new SizeRequest(new Size(View.MeasuredWidth, View.MeasuredHeight), Size.Zero);

			if (setHint)
				TypedNativeView.Hint = hint;

			return result;
		}

		protected static void MapPropertyText(IViewHandler Handler, IButton view)
		{
			if (!(Handler.NativeView is AppCompatButton nativeButton))
				return;

			nativeButton.Text = view.Text;
		}

		protected static void MapPropertyTextColor(IViewHandler Handler, IButton view)
		{
			(Handler as ButtonHandler)?.UpdateTextColor();
		}

		protected static void MapPropertyCornerRadius(IViewHandler Handler, IButton view)
		{
			(Handler as ButtonHandler)?.UpdateBorder();
		}

		protected static void MapPropertyBorderColor(IViewHandler Handler, IButton view)
		{
			(Handler as ButtonHandler)?.UpdateBorder();
		}

		protected static void MapPropertyBorderWidth(IViewHandler Handler, IButton view)
		{
			(Handler as ButtonHandler)?.UpdateBorder();
		}

		protected static void MapPropertyFont(IViewHandler Handler, IButton view)
		{
			(Handler as ButtonHandler)?.UpdateFont();
		}

		protected static void MapPropertyFontSize(IViewHandler Handler, IButton view)
		{
			(Handler as ButtonHandler)?.UpdateFont();
		}

		protected static void MapPropertyFontAttributes(IViewHandler Handler, IButton view)
		{
			(Handler as ButtonHandler)?.UpdateFont();
		}

		protected static void MapPropertyCharacterSpacing(IViewHandler Handler, IButton view)
		{
			(Handler as ButtonHandler)?.UpdateCharacterSpacing();
		}

		public virtual void UpdateTextColor()
		{
			if (TypedNativeView == null || VirtualView == null || _textColorSwitcher == null)
				return;

			_textColorSwitcher.Value.UpdateTextColor(VirtualView.TextColor);
		}

		public virtual void UpdateBorder()
		{
			_backgroundTracker?.UpdateDrawable();
		}

		public virtual void UpdateFont()
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			double fontSize = VirtualView.FontSize;
			FontAttributes fontAttributes = VirtualView.FontAttributes;
			string fontFamily = VirtualView.FontFamily;

			Font font;

			if (fontFamily != null)
				font = Font.OfSize(fontFamily, fontSize).WithAttributes(fontAttributes);
			else
				font = Font.SystemFontOfSize(fontSize, fontAttributes);

			if (font == Font.Default && _defaultFontSize == 0f)
			{
				return;
			}

			if (_defaultFontSize == 0f)
			{
				_defaultTypeface = TypedNativeView.Typeface;
				_defaultFontSize = TypedNativeView.TextSize;
			}

			if (font == Font.Default)
			{
				TypedNativeView.Typeface = _defaultTypeface;
				TypedNativeView.SetTextSize(ComplexUnitType.Px, _defaultFontSize);
			}
			else
			{
				TypedNativeView.Typeface = font.ToTypeface();
				TypedNativeView.SetTextSize(ComplexUnitType.Sp, font.ToScaledPixel());
			}
		}

		public virtual void UpdateCharacterSpacing()
		{
			if (TypedNativeView == null || VirtualView == null)
				return;

			if (NativeVersion.IsAtLeast(21))
			{
				TypedNativeView.LetterSpacing = VirtualView.CharacterSpacing.ToEm();
			}
		}

		void OnButtonClick(object sender, EventArgs e)
		{
			VirtualView.Clicked();
		}
	}
}
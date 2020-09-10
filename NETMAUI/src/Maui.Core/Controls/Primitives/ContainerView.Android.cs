using Android.Content;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using AView = Android.Views.View;

namespace System.Maui.Controls.Primitives
{
	public partial class ContainerView : FrameLayout
	{
		AView _mainView;

		public ContainerView(Context context) : base(context)
		{
			SetWillNotDraw(false);
			SetLayerType(LayerType.Hardware, null);
			LayoutParameters = new LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
		}

		public AView MainView
		{
			get => _mainView;
			set
			{
				if (_mainView == value)
					return;
				if (_mainView != null)
				{
					RemoveView(_mainView);
				}

				_mainView = value;
				var parent = _mainView?.Parent as ViewGroup;

				if (_mainView != null)
				{
					_mainView.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
					AddView(_mainView);
				}
			}
		}
		protected override void OnLayout(bool changed, int l, int t, int r, int b)
		{
			base.OnLayout(changed, l, t, r, b);

			if (changed)
			{
				MainView?.Layout(l, t, r, b);
				PostInvalidate();
			}
		}

		protected override void DispatchDraw(Canvas canvas)
		{
			base.DispatchDraw(canvas);
			MainView?.Draw(canvas);
		}
		protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
		{
			_mainView?.Measure(widthMeasureSpec, heightMeasureSpec);
			base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
			SetMeasuredDimension(_mainView.MeasuredWidth, _mainView.MeasuredHeight);
		}
	}
}
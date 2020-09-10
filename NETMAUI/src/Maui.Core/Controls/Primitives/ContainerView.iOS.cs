using UIKit;

namespace System.Maui.Controls.Primitives
{
	public partial class ContainerView : UIView
	{
		UIView _mainView;

		public ContainerView()
		{
			AutosizesSubviews = true;
		}

		public UIView MainView
		{
			get => _mainView;
			set
			{
				if (_mainView == value)
					return;

				if (_mainView != null)
				{
					//Cleanup!
					_mainView.RemoveFromSuperview();
				}

				_mainView = value;

				if (value == null)
					return;

				Frame = _mainView.Frame;
				var oldParent = value.Superview;

				if (oldParent != null)
					oldParent.InsertSubviewAbove(this, _mainView);

				_mainView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
				_mainView.Frame = Bounds;

				AddSubview(_mainView);
			}
		}

		public override void SizeToFit()
		{
			MainView.SizeToFit();
			Bounds = MainView.Bounds;
			base.SizeToFit();
		}
	}
}
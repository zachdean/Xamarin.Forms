using UIKit;
using RectangleF = CoreGraphics.CGRect;
using SizeF = CoreGraphics.CGSize;

namespace Xamarin.Platform
{
	public class NativeLabel : UILabel
	{
		public NativeLabel(RectangleF frame) : base(frame)
		{

		}

		public UIEdgeInsets TextInsets { get; set; }

		public override void DrawText(RectangleF rect) => base.DrawText(TextInsets.InsetRect(rect));

		public override SizeF SizeThatFits(SizeF size) => AddInsets(base.SizeThatFits(size));

		SizeF AddInsets(SizeF size) => new SizeF(
			width: size.Width + TextInsets.Left + TextInsets.Right,
			height: size.Height + TextInsets.Top + TextInsets.Bottom);
	}
}
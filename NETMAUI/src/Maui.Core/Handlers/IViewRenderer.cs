using System.Maui.Controls.Primitives;

namespace System.Maui
{
	public interface IViewHandler
	{
		void SetView(IView view);
		void UpdateValue(string property);
		void Remove(IView view);
		object NativeView { get; }
		bool HasContainer { get; set; }
		ContainerView ContainerView { get; }
		SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint);
		void SetFrame(Rect frame);
	}
}
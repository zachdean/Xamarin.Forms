namespace System.Maui.Platform
{
	public abstract partial class AbstractViewHandler<TVirtualView, TNativeView>
	{
		public void SetFrame(Rect rect)
		{

		}
		public virtual SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
			=> new SizeRequest();

		void SetupContainer() { }
		void RemoveContainer() { }
	}
}
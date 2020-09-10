namespace System.Maui.Controls
{
	public class View : IView, IPropertyMapperView
	{
		public View()
		{

		}

		public bool IsEnabled { get; set; } = true;

		public bool IsFocused { get; set; }

		public Color BackgroundColor { get; set; }

		public Rect Frame { get; set; }

		public IViewHandler Handler { get; set; }

		public IView Parent { get; set; }

		protected PropertyMapper propertyMapper;
		protected PropertyMapper<T> GetHandlerOverides<T>() where T : IView => (PropertyMapper<T>)(propertyMapper as PropertyMapper<T> ?? (propertyMapper = new PropertyMapper<T>()));
		PropertyMapper IPropertyMapperView.GetPropertyMapperOverrides() => propertyMapper;
	}
}
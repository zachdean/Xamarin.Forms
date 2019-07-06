using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;


using System.Reflection;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms
{
	[ContentProperty(nameof(Content))]
	public class ShellContent : BaseShellItem, IShellContentController
	{
		static readonly BindablePropertyKey MenuItemsPropertyKey =
			BindableProperty.CreateReadOnly(nameof(MenuItems), typeof(MenuItemCollection), typeof(ShellContent), null,
				defaultValueCreator: bo => new MenuItemCollection());

		public static readonly BindableProperty MenuItemsProperty = MenuItemsPropertyKey.BindableProperty;

		public static readonly BindableProperty ContentProperty =
			BindableProperty.Create(nameof(Content), typeof(object), typeof(ShellContent), null, BindingMode.OneTime, propertyChanged: OnContentChanged);

		public static readonly BindableProperty ContentTemplateProperty =
			BindableProperty.Create(nameof(ContentTemplate), typeof(DataTemplate), typeof(ShellContent), null, BindingMode.OneTime);

		internal static readonly BindableProperty QueryAttributesProperty =
			BindableProperty.CreateAttached("QueryAttributes", typeof(IDictionary<string, string>), typeof(ShellContent), defaultValue: null, propertyChanged: OnQueryAttributesPropertyChanged);

		public MenuItemCollection MenuItems => (MenuItemCollection)GetValue(MenuItemsProperty);

		public object Content {
			get => GetValue(ContentProperty);
			set => SetValue(ContentProperty, value);
		}

		public DataTemplate ContentTemplate {
			get => (DataTemplate)GetValue(ContentTemplateProperty);
			set => SetValue(ContentTemplateProperty, value);
		}

		Page IShellContentController.Page => ContentCache;

		Page IShellContentController.GetOrCreateContent()
		{
			if (ContentCache != null)
			{
				if (GetValue(QueryAttributesProperty) is IDictionary<string, string> delayedQueryParams)
					ContentCache.SetValue(QueryAttributesProperty, delayedQueryParams);
			}
			else
			{
				var newPage = ContentCache ?? ShellContentCreator.Create(new ShellContentCreateArgs(this));

				if (newPage == null)
					throw new InvalidOperationException($"No Content found for {nameof(ShellContent)}, Title:{Title}, Route {Route}");

				if (GetValue(QueryAttributesProperty) is IDictionary<string, string> delayedQueryParams)
					newPage.SetValue(QueryAttributesProperty, delayedQueryParams);

				newPage.Appearing += OnPageAppearing;

				if (newPage != null && newPage.Parent != this)
					OnChildAdded(newPage);

				ContentCache = newPage;
			}

			return ContentCache;
		}

		#region Navigation Interfaces
		IShellContentCreator ShellContentCreator => (Parent.Parent.Parent as Shell).ShellContentCreator;
		IShellPartAppearing ShellPartAppearing => (Parent.Parent.Parent as Shell).ShellPartAppearing;
		#endregion

		void IShellContentController.RecyclePage(Page page)
		{
		}

		Page _contentCache;
		IList<Element> _logicalChildren = new List<Element>();
		ReadOnlyCollection<Element> _logicalChildrenReadOnly;

		public ShellContent() => ((INotifyCollectionChanged)MenuItems).CollectionChanged += MenuItemsCollectionChanged;

		internal bool IsVisibleContent => Parent is ShellSection shellSection && shellSection.IsVisibleSection;
		internal override ReadOnlyCollection<Element> LogicalChildrenInternal => _logicalChildrenReadOnly ?? (_logicalChildrenReadOnly = new ReadOnlyCollection<Element>(_logicalChildren));

		internal override void SendDisappearing()
		{
			base.SendDisappearing();
			((ContentCache ?? Content) as Page)?.SendDisappearing();
		}

		internal override void SendAppearing()
		{
			// only fire Appearing when the Content Page exists on the ShellContent
			var content = ContentCache ?? Content;
			if (content == null)
				return;

			base.SendAppearing();
			((ContentCache ?? Content) as Page)?.SendAppearing();
		}

		protected override void OnChildAdded(Element child)
		{
			base.OnChildAdded(child);
		}

		async void OnPageAppearing(object sender, EventArgs e)
		{			
			var routePath = (this.Parent.Parent.Parent as Shell).RouteState;
			await ShellPartAppearing.AppearingAsync(new ShellLifecycleArgs(this, routePath.CurrentRoute.PathParts.FirstOrDefault(x => x.ShellPart == this), routePath.CurrentRoute));
		}

		Page ContentCache
		{
			get => _contentCache;
			set
			{
				_contentCache = value;
				if (Parent != null)
					((ShellSection)Parent).UpdateDisplayedPage();
			}
		}

		public static implicit operator ShellContent(TemplatedPage page)
		{
			var shellContent = new ShellContent();

			var pageRoute = Routing.GetRoute(page);

			shellContent.Route = Routing.GenerateImplicitRoute(pageRoute);

			shellContent.Content = page;
			shellContent.SetBinding(TitleProperty, new Binding(nameof(Title), BindingMode.OneWay, source: page));
			shellContent.SetBinding(IconProperty, new Binding(nameof(Icon), BindingMode.OneWay, source: page));
			shellContent.SetBinding(FlyoutIconProperty, new Binding(nameof(FlyoutIcon), BindingMode.OneWay, source: page));

			return shellContent;
		}

		static void OnContentChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var shellContent = (ShellContent)bindable;
			// This check is wrong but will work for testing
			if (shellContent.ContentTemplate == null)
			{
				// deparent old item
				if (oldValue is Page oldElement)
				{
					shellContent.OnChildRemoved(oldElement);
					shellContent.ContentCache = null;
				}

				// make sure LogicalChildren collection stays consisten
				shellContent._logicalChildren.Clear();
				if (newValue is Page newElement)
				{
					shellContent._logicalChildren.Add((Element)newValue);
					shellContent.ContentCache = newElement;
					// parent new item
					shellContent.OnChildAdded(newElement);
				}
				else if(newValue != null)
				{
					throw new InvalidOperationException($"{nameof(ShellContent)} {nameof(Content)} should be of type {nameof(Page)}. Title {shellContent?.Title}, Route {shellContent?.Route} ");
				}
			}

			if (shellContent.Parent?.Parent is ShellItem shellItem)
				shellItem?.SendStructureChanged();
		}

		void MenuItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.NewItems != null)
				foreach (Element el in e.NewItems)
					OnChildAdded(el);

			if (e.OldItems != null)
				foreach (Element el in e.OldItems)
					OnChildRemoved(el);
		}

		internal override void ApplyQueryAttributes(IDictionary<string, string> query)
		{
			base.ApplyQueryAttributes(query);
			SetValue(QueryAttributesProperty, query);

			if (Content is BindableObject bindable)
				bindable.SetValue(QueryAttributesProperty, query);

			if (ContentCache != Content && ContentCache is BindableObject contentCacheBo)
				contentCacheBo.SetValue(QueryAttributesProperty, query);
		}

		static void OnQueryAttributesPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (newValue is IDictionary<string, string> query)
				ApplyQueryAttributes(bindable, query);
		}

		static void ApplyQueryAttributes(object content, IDictionary<string, string> query)
		{
			if (content is IQueryAttributable attributable)
				attributable.ApplyQueryAttributes(query);

			if (content is BindableObject bindable && bindable.BindingContext != null && content != bindable.BindingContext)
				ApplyQueryAttributes(bindable.BindingContext, query);

			var type = content.GetType();
			var typeInfo = type.GetTypeInfo();
#if NETSTANDARD1_0
			var queryPropertyAttributes = typeInfo.GetCustomAttributes(typeof(QueryPropertyAttribute), true).ToArray();
#else
			var queryPropertyAttributes = typeInfo.GetCustomAttributes(typeof(QueryPropertyAttribute), true);
#endif
			
			foreach (QueryPropertyAttribute attrib in queryPropertyAttributes) {
				if (query.TryGetValue(attrib.QueryId, out var value)) {
					PropertyInfo prop = type.GetRuntimeProperty(attrib.Name);

					if (prop != null && prop.CanWrite && prop.SetMethod.IsPublic)
						prop.SetValue(content, value);
				}
			}
		}
	}
}

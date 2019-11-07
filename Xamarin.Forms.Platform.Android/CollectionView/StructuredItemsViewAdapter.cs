using System;
using System.ComponentModel;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;
using ViewGroup = Android.Views.ViewGroup;

namespace Xamarin.Forms.Platform.Android
{
	public class StructuredItemsViewAdapter<TItemsView, TItemsViewSource> : ItemsViewAdapter<TItemsView, TItemsViewSource>
		where TItemsView : StructuredItemsView
		where TItemsViewSource : IItemsViewSource
	{
		internal StructuredItemsViewAdapter(TItemsView itemsView, 
			Func<View, Context, ItemContentView> createItemContentView = null) : base(itemsView, createItemContentView)
		{
			UpdateHasHeader();
			UpdateHasFooter();
			UpdateHasEmpty();
		}

		protected override void ItemsViewPropertyChanged(object sender, PropertyChangedEventArgs property)
		{
			base.ItemsViewPropertyChanged(sender, property);

			if (property.Is(StructuredItemsView.HeaderProperty))
			{
				UpdateHasHeader();
				NotifyDataSetChanged();
			}
			else if (property.Is(StructuredItemsView.FooterProperty))
			{
				UpdateHasFooter();
				NotifyDataSetChanged();
			}
			else if (property.IsOneOf(Xamarin.Forms.ItemsView.EmptyViewProperty, Xamarin.Forms.ItemsView.EmptyViewTemplateProperty))
			{
				UpdateHasEmpty();
				NotifyDataSetChanged();
			}
		}
  
		public override int GetItemViewType(int position)
		{
			if (IsHeader(position))
			{
				return ItemViewType.Header;
			}

			if (IsFooter(position))
			{
				return ItemViewType.Footer;
			}

			if (IsEmpty(position))
			{
				return ItemViewType.Empty;
			}

			return base.GetItemViewType(position);
		}

		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
		{
			var context = parent.Context;

			if (viewType == ItemViewType.Header)
			{
				return CreateHeaderFooterViewHolder(ItemsView.Header, ItemsView.HeaderTemplate, context);
			}

			if (viewType == ItemViewType.Footer)
			{
				return CreateHeaderFooterViewHolder(ItemsView.Footer, ItemsView.FooterTemplate, context);
			}

			if (viewType == ItemViewType.Empty)
			{
				return CreateEmptyViewHolder(ItemsView.EmptyView, ItemsView.EmptyViewTemplate, parent);
			}

			return base.OnCreateViewHolder(parent, viewType);
		}

		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
		{
			if (IsHeader(position))
			{
				if (holder is TemplatedItemViewHolder templatedItemViewHolder)
				{
					BindTemplatedItemViewHolder(templatedItemViewHolder, ItemsView.Header);
				}

				return;
			}

			if (IsFooter(position))
			{
				if (holder is TemplatedItemViewHolder templatedItemViewHolder)
				{
					BindTemplatedItemViewHolder(templatedItemViewHolder, ItemsView.Footer);
				}

				return;
			}

			if(IsEmpty(position))
			{
				if (holder is SimpleViewHolder emptyViewHolder && emptyViewHolder.View != null)
				{
					// For templated empty views, this will happen on bind. But if we just have a plain-old View,
					// we need to add it as a "child" of the ItemsView here so that stuff like Visual and FlowDirection
					// propagate to the controls in the EmptyView
					ItemsView.AddLogicalChild(emptyViewHolder.View);
				}
				else if (holder is TemplatedItemViewHolder templatedItemViewHolder && ItemsView.EmptyViewTemplate != null)
				{
					// Use EmptyView as the binding context for the template
					templatedItemViewHolder.Bind(ItemsView.EmptyView, ItemsView);
				}
			}

			base.OnBindViewHolder(holder, position);
		}

		public override void UpdateHasEmpty()
		{
			ItemsSource.HasEmpty = ItemsSource.ItemsCount == 0 && (ItemsView.EmptyView != null || ItemsView.EmptyViewTemplate != null);
		}

		void UpdateHasHeader()
		{
			ItemsSource.HasHeader = ItemsView.Header != null;
		}

		void UpdateHasFooter()
		{
			ItemsSource.HasFooter = ItemsView.Footer != null;
		}

		bool IsHeader(int position)
		{
			return ItemsSource.IsHeader(position);
		}

		bool IsFooter(int position)
		{
			return ItemsSource.IsFooter(position);
		}

		bool IsEmpty(int position)
		{
			return ItemsSource.IsEmpty(position);
		}
  
		protected RecyclerView.ViewHolder CreateHeaderFooterViewHolder(object content, DataTemplate template, Context context)
		{
			if (template != null)
			{
				var footerContentView = new ItemContentView(context);
				return new TemplatedItemViewHolder(footerContentView, template, isSelectionEnabled: false);
			}

			if (content is View formsView)
			{
				return SimpleViewHolder.FromFormsView(formsView, context);
			}

			// No template, Footer is not a Forms View, so just display Footer.ToString
			return SimpleViewHolder.FromText(content?.ToString(), context, false);
		}

		protected RecyclerView.ViewHolder CreateEmptyViewHolder(object content, DataTemplate template, ViewGroup parent)
		{
			var context = parent.Context;

			if (template == null)
			{
				if (!(content is View formsView))
				{
					// No template, EmptyView is not a Forms View, so just display EmptyView.ToString
					return SimpleViewHolder.FromText(content?.ToString(), context, () => parent.Width, () => parent.Height);
				}

				// EmptyView is a Forms View; display that
				return SimpleViewHolder.FromFormsView(formsView, context, () => parent.Width, () => parent.Height);
			}

			// Use EmptyViewTemplate
			var itemContentView = new SizedItemContentView(context, () => parent.Width, () => parent.Height);
			return new TemplatedItemViewHolder(itemContentView, template, isSelectionEnabled: false);
		}
	}
}
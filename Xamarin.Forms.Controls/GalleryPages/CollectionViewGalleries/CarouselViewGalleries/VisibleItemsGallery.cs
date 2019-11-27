using System;
using Xamarin.Forms.Controls.GalleryPages.CollectionViewGalleries.SpacingGalleries;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace Xamarin.Forms.Controls.GalleryPages.CollectionViewGalleries.CarouselViewGalleries
{
	[Preserve(AllMembers = true)]
	public class VisibleItemsGallery : ContentPage
	{
		public VisibleItemsGallery()
		{
			On<iOS>().SetLargeTitleDisplay(LargeTitleDisplayMode.Never);

			Title = "VisibleItems";

			var nItems = 5;
			var layout = new Grid
			{
				RowDefinitions = new RowDefinitionCollection
				{
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Star }
				}
			};
			var itemsLayout =
			new LinearItemsLayout(ItemsLayoutOrientation.Horizontal)
			{
				SnapPointsType = SnapPointsType.MandatorySingle,
				SnapPointsAlignment = SnapPointsAlignment.Center
			};

			var itemTemplate = ExampleTemplates.CarouselTemplate();

			var carouselView = new CarouselView
			{
				ItemsLayout = itemsLayout,
				ItemTemplate = itemTemplate,
				Position = 2,
				HeightRequest = 150,
				Margin = new Thickness(0, 10, 0, 40),
				BackgroundColor = Color.LightGray,
				PeekAreaInsets = new Thickness(30, 0, 30, 0),
				AutomationId = "TheCarouselView"
			};

			layout.Children.Add(carouselView);

			var generator = new ItemsSourceGenerator(carouselView, initialItems: nItems, itemsSourceType: ItemsSourceType.ObservableCollection);
			layout.Children.Add(generator);

			var spacingModifier = new SpacingModifier(carouselView.ItemsLayout, "Update Spacing");
			layout.Children.Add(spacingModifier);

			var stckPeek = new StackLayout { Orientation = StackOrientation.Horizontal };
			stckPeek.Children.Add(new Label { Text = "Peek" });
			var padi = new Slider
			{
				Maximum = 100,
				Minimum = 0,
				Value = 30,
				WidthRequest = 100,
				BackgroundColor = Color.Pink
			};

			layout.Children.Add(stckPeek);
	
			stckPeek.Children.Add(padi);

			var stckVisibleItems = new StackLayout();
			var visibleItemsText = new Label();
			stckVisibleItems.Children.Add(visibleItemsText);

			layout.Children.Add(stckVisibleItems);

			Grid.SetRow(stckPeek, 1);
			Grid.SetRow(spacingModifier, 2);
			Grid.SetRow(carouselView, 3);
			Grid.SetRow(stckVisibleItems, 4);

			Content = layout;

			generator.GenerateItems();

			UpdateVisibleItems();

			padi.ValueChanged += (s, e) =>
			{
				var peek = padi.Value;
				carouselView.PeekAreaInsets = new Thickness(peek, 0, peek, 0);
				UpdateVisibleItems();
			};

			generator.CollectionChanged += (s, e) =>
			{
				UpdateVisibleItems();
			};

			carouselView.Scrolled += (s, e) =>
			{
				UpdateVisibleItems();
			};

			void UpdateVisibleItems()
			{ 
				visibleItemsText.Text = string.Empty;

				System.Diagnostics.Debug.WriteLine("UpdateVisibleItems");
				var visibleItems = carouselView.VisibleItems;

				if (visibleItems == null)
					return;

				int count = 0;
				foreach(var visibleItem in visibleItems)
				{
					visibleItemsText.Text += visibleItem.ToString() + Environment.NewLine;
					count++;
				}

				visibleItemsText.Text += Environment.NewLine + $"VisibleItems: {count}";
			}
		}
	}
}
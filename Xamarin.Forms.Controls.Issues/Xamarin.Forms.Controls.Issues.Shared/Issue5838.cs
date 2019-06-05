using Xamarin.Forms.CustomAttributes;
using Xamarin.Forms.Internals;
using System.Collections.Generic;
using System.Security.Cryptography;
using System;

#if UITEST
using Xamarin.Forms.Core.UITests;
using Xamarin.UITest;
using NUnit.Framework;
#endif

namespace Xamarin.Forms.Controls.Issues
{
#if UITEST
	[Category(UITestCategories.ManualReview)]
#endif
	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Github, 5838, "[Android] 4.0.1.305340-nightly breaks ListView recylcing", PlatformAffected.Android)]
	public class Issue5838 : TestContentPage // or TestMasterDetailPage, etc ...
	{
		protected override void Init()
		{
			var grd = new Grid();
			grd.RowDefinitions.Add(new RowDefinition { Height = 100 });
			grd.RowDefinitions.Add(new RowDefinition { });
			var lbl = new Label
			{
				BackgroundColor = Color.LightBlue,
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Center,
				LineBreakMode = LineBreakMode.WordWrap,
				Text = "Scroll up and down several times. Observe that cell heights are no longer correct per position."
			};
			grd.Children.Add(lbl);
			var lst = new ListView(ListViewCachingStrategy.RecycleElement)
			{
				HasUnevenRows = true,
				ItemsSource = new ViewModel().Items,
				ItemTemplate = new DataTemplate(typeof(ItemCell))

			};
			grd.Children.Add(lst, 0, 1);
			Content = grd;
		}

		[Preserve(AllMembers = true)]
		class ItemCell : ViewCell
		{
			public ItemCell()
			{
				var grd = new Grid
				{
					Margin = new Thickness(0, 0, 0, 10),
					BackgroundColor = Color.LightPink
				};
				grd.SetBinding(Grid.HeightRequestProperty, nameof(Model.HeightRequest));
				grd.RowDefinitions.Add(new RowDefinition { });
				grd.RowDefinitions.Add(new RowDefinition { });
				var lbl = new Label
				{
					HorizontalTextAlignment = TextAlignment.Center,
					VerticalTextAlignment = TextAlignment.Center,
				};
				var lbl2 = new Label
				{
					HorizontalTextAlignment = TextAlignment.Center,
					VerticalTextAlignment = TextAlignment.Center,
				};
				lbl.SetBinding(Label.TextProperty, nameof(Model.HeightRequest));
				lbl2.SetBinding(Label.TextProperty, nameof(Model.Position));
				grd.Children.Add(lbl);
				grd.Children.Add(lbl2, 0, 1);
				View = grd;
			}
		}

		[Preserve(AllMembers = true)]
		class ViewModel
		{
			public List<Model> Items = new List<Model>();

			public ViewModel()
			{
				var rng = new RNGCryptoServiceProvider();
				var data = new byte[4];

				for (var i = 0; i < 100; i++)
				{
					rng.GetBytes(data);
					var heightRequest = 50 + SmartMod(BitConverter.ToInt32(data, 0), 300);

					Items.Add(new Model
					{
						Position = i,
						HeightRequest = heightRequest
					});
				}
			}

			int SmartMod(int x, int m)
			{
				return (x % m + m) % m;
			}
		}

		[Preserve(AllMembers = true)]
		class Model
		{
			public int Position { get; set; }

			public int HeightRequest { get; set; }
		}
	}
}
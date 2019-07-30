using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.CustomAttributes;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace Xamarin.Forms.Controls.Issues
{
	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Github, 3769, "[iOS] ListView Group Header size incorrect on iOS", PlatformAffected.iOS)]
	public partial class Issue3769 : TestContentPage
	{
		#region Items

		public static readonly BindableProperty ItemsProperty = BindableProperty.Create(
			nameof(Items),
			typeof(List<List<string>>),
			typeof(Issue3769),
			default(List<List<string>>));

		public List<List<string>> Items
		{
			get => (List<List<string>>)GetValue(ItemsProperty);
			set => SetValue(ItemsProperty, value);
		}

		#endregion

		public Issue3769()
		{
#if APP
			Items = new List<List<string>>
			{
				new List<string>
				{
					"Item 1",
					"Item 2",
					"Item 3",
				},
				new List<string>
				{
					"Item 4",
					"Item 5",
					"Item 6",
				},
				new List<string>
				{
					"Item 7",
					"Item 8",
					"Item 9",
				},
				new List<string>
				{
					"Item 10",
					"Item 11",
					"Item 12",
				},
				new List<string>
				{
					"Item 13",
					"Item 14",
					"Item 15",
				},
				new List<string>
				{
					"Item 16",
					"Item 17",
					"Item 18",
				},
				new List<string>
				{
					"Item 19",
					"Item 20",
					"Item 21",
				},
				new List<string>
				{
					"Item 22",
					"Item 23",
					"Item 24",
				},
			};
			BindingContext = this;

			InitializeComponent();
#endif
		}

		protected override void Init()
		{

		}
	}
}
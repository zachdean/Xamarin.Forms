using Xamarin.Forms.CustomAttributes;
using System;

#if UITEST
using Xamarin.UITest;
using NUnit.Framework;
using Xamarin.Forms.Core.UITests;
#endif

namespace Xamarin.Forms.Controls.Issues
{
	[Issue(IssueTracker.Github, 11405,
		"[UWP] Cannot unselect SelectedItem for CollectionView",
		PlatformAffected.UWP)]
	public partial class Issue11405 : TestContentPage
	{
		public Issue11405()
		{
#if APP
			InitializeComponent();
#endif
		}

		protected override void Init()
		{
		}
#if APP
		void OnUnselectClicked(object sender, EventArgs e)
		{
			MonkeyCollectionView.SelectedItem = null;
		}
#endif
	}
}
using Xamarin.Forms.CustomAttributes;
using System;

#if UITEST
using Xamarin.Forms.Core.UITests;
using Xamarin.UITest;
using NUnit.Framework;
#endif

namespace Xamarin.Forms.Controls.Issues
{
	[Issue(IssueTracker.Github, 10014, "[Bug] CollectionView ScrollTo not scrolling on ContentView.LayoutChanged iOS",
		PlatformAffected.iOS)]
	public sealed partial class Issue10014 : TestContentPage
    {
        public Issue10014()
        {
            this.InitializeComponent();
        }

		protected override void Init()
		{

		}

		void OnLayoutChanged(object sender, EventArgs e)
		{
			Collection.ScrollTo(3, position: ScrollToPosition.Start, animate: false);
		}
	}
}

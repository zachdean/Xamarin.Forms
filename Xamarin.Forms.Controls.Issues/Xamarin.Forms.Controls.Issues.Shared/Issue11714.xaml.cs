using Xamarin.Forms.CustomAttributes;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;

#if UITEST
using Xamarin.UITest;
using Xamarin.UITest.Queries;
using NUnit.Framework;
using Xamarin.Forms.Core.UITests;
#endif

namespace Xamarin.Forms.Controls.Issues
{
#if UITEST
	[NUnit.Framework.Category(UITestCategories.Brush)]
#endif
#if APP
	[XamlCompilation(XamlCompilationOptions.Compile)]
#endif
	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Github, 11714, "[Bug] Brushes API - z draw order on Android is incorrect in certain cases", PlatformAffected.Android)]
	public partial class Issue11714 : TestContentPage
	{
		public Issue11714()
		{
#if APP
			Title = "Issue 11714";
			Device.SetFlags(new List<string> { ExperimentalFlags.BrushExperimental });
			InitializeComponent();
#endif
        }

        protected override void Init()
		{

		}
	}
}
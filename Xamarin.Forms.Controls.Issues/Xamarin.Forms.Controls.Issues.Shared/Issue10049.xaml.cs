using Xamarin.Forms.CustomAttributes;
using Xamarin.Forms.Internals;
using System.Collections.Generic;

#if UITEST
using Xamarin.UITest;
using NUnit.Framework;
using Xamarin.UITest.iOS;
#endif

namespace Xamarin.Forms.Controls.Issues
{
	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Github, 10049, "[Bug] AdaptiveTrigger crashes when paired with a Setter with TargetName", PlatformAffected.UWP)]
	public partial class Issue10049 : TestContentPage
    {
        public Issue10049()
		{
#if APP
			Device.SetFlags(new List<string> { ExperimentalFlags.StateTriggersExperimental });
			Title = "Issue 10049";
            InitializeComponent();
#endif
		}

		protected override void Init()
		{
		
		}
	}
}
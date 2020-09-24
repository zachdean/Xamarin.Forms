using Xamarin.Forms.CustomAttributes;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Controls.Issues
{
	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Github, 8617, "[Bug] RefreshView - Not enabled when using ScrollView unless content fills space [UWP/iOS]", PlatformAffected.UWP)]
	public partial class Issue8617 : TestContentPage
	{
		public Issue8617()
		{
#if APP
			InitializeComponent();
#endif
		}

		protected override void Init()
		{

		}
	}
}
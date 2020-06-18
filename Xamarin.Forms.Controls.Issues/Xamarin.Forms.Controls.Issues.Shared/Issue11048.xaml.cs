using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.CustomAttributes;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Controls.Issues
{

	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Github, 11048, "[Bug][Android] Shapes: flower not drawing correctly on Android", PlatformAffected.Android)]
	public partial class Issue11048 : ContentPage
	{
		public Issue11048()
		{
#if APP
			InitializeComponent();
#endif

		}
	}

}
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace Xamarin.Forms.Controls
{
	[Preserve(AllMembers = true)]
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HanselmanShell : TestShell
	{
		public HanselmanShell()
		{
			InitializeComponent();
		}

		protected override void Init()
		{
			
		}
	}
}

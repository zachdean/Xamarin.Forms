using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms.CustomAttributes;
using Xamarin.Forms.Internals;


#if UITEST
using Xamarin.UITest;
using NUnit.Framework;
#endif

namespace Xamarin.Forms.Controls.Issues
{
	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Github, 8724, "BindableLayout loses sync with source if RaiseChild is used",
		PlatformAffected.Android)]
	public class Issue8724 : TestContentPage
	{
		protected override void Init()
		{
			StackLayout layout = new StackLayout();
			ObservableCollection<int> items = new ObservableCollection<int>();

			items.Add(1);
			items.Add(2);
			items.Add(3);

			BindableLayout.SetItemsSource(layout, items);
			BindableLayout.SetItemTemplate(layout,
				new DataTemplate
				(() =>
				{
					Label label = new Label();
					label.SetBinding(Label.TextProperty, ".");
					label.SetBinding(Label.AutomationIdProperty, ".");
					return label;
				}
				));

			this.Appearing += (_, __) =>
			{
				layout.RaiseChild(layout.Children[1]);
				items.RemoveAt(1);
			};

			Content = layout;
		}


#if UITEST
		[Test]
		public void RemovingItemFromBindableLayoutSourceRemovesCorrectRaisedChild()
		{
			RunningApp.WaitForElement("1");
			RunningApp.WaitForNoElement("2");
		}
#endif
	}
}

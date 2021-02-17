using Xamarin.Forms.CustomAttributes;
using Xamarin.Forms.Internals;

#if UITEST
using Xamarin.Forms.Core.UITests;
using Xamarin.UITest;
using NUnit.Framework;
#endif

namespace Xamarin.Forms.Controls.Issues
{

#if UITEST
	[Category(UITestCategories.Label)]
	[Category(UITestCategories.Github10000)]
#endif

	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Github, 11499, "[Bug] TextTransform not implemented on several controls", PlatformAffected.All)]
	public class Issue11499 : TestContentPage
	{
		protected override void Init()
		{
			// Initialize ui here instead of ctor
			Content = new Label
			{
				AutomationId = "IssuePageLabel",
				FormattedText = new FormattedString
				{
					Spans =
					{
						new Span
						{
							Text = "transform spans "
						},
						new Span
						{
							Text = "to uppercase ",
							TextTransform = TextTransform.Uppercase
						},
						new Span
						{
							Text = "AND LOWERCASE",
							TextTransform = TextTransform.Lowercase
						}
					}
				}
			};
		}

#if UITEST
		[Test]
		public void Issue11499SpanTest()
		{
			var label = RunningApp.WaitForElement("IssuePageLabel");
			RunningApp.Screenshot ("I am at Issue 11499, Span Text Transform");			
			Assert.AreEqual("transform spans TO UPPERCASE and lowercase", label[0].Text);
		}
#endif
	}
}
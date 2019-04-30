using Xamarin.Forms.CustomAttributes;
using Xamarin.Forms.Internals;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

#if UITEST
using Xamarin.UITest;
using NUnit.Framework;
#endif

namespace Xamarin.Forms.Controls.Issues
{
	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Github, 2210, "[Enhancement] Add better life cycle events", PlatformAffected.All)]
	public class Issue2210 : TestMasterDetailPage
	{
		const string startTestLabel = "GoTest";
		const string switchButtonLabel = "Switch Legacy Events";
		const string successlText = "Success";
		const string resultId = "Result";

		public enum State
		{
			PageAppearing,
			PageAppeared,
			PageDisappearing,
			PageDisappeared,
			ElementAppearing,
			ElementAppeared,
			ElementDisappearing,
			ElementDisappeared,
		}

		protected override void Init()
		{
			Application.Current.UseLegacyPageEvents = false;
			MasterBehavior = MasterBehavior.Split;

			IsPresented = true;

			var states = new List<State>();
			var checkLabel = new Label();
			var resultLabel = new Label { AutomationId = resultId };

			var goTestButton = new Button
			{
				Text = startTestLabel,
				Command = new Command(async () =>
				{
					states.Clear();
					resultLabel.Text = "Let's assume that this text is not here.";
					checkLabel.Text = "But soon everything will change.";

					var button = new Button
					{
						Text = "Button"
					};
					button.Appeared += (_, __) => states.Add(State.ElementAppeared);
					button.Disappeared += (_, __) => states.Add(State.ElementDisappeared);
					button.Disappearing += (_, __) => states.Add(State.ElementDisappearing);
					button.Appearing += (_, __) => states.Add(State.ElementAppearing);

					var page = new ContentPage
					{
						Content = button
					};
					page.Appearing += (_, __) => states.Add(State.PageAppearing);
					page.Appeared += (_, __) => states.Add(State.PageAppeared);
					page.Disappearing += (_, __) => states.Add(State.PageDisappearing);
					page.Disappeared += (_, __) => states.Add(State.PageDisappeared);

					await Detail.Navigation.PushAsync(page);
					// UWP not waiting for page creation
					await Task.Delay(800);
					Detail.Navigation.RemovePage(page);

					var eraserPage = new ContentPage();
					await Detail.Navigation.PushAsync(eraserPage);
					Detail.Navigation.RemovePage(eraserPage);
					await Task.Delay(200);

					var sb = new StringBuilder();
					if (Application.Current.UseLegacyPageEvents)
						sb.AppendLine($"Legacy events");

					for (int i = 0; i < states.Count; i++)
						sb.AppendLine($"{i + 1}. {states[i]}");
					resultLabel.Text = sb.ToString();

					bool success = false;

					if (Application.Current.UseLegacyPageEvents)
					{
						success =
							states.Count == 4 &&
							states.Count == new HashSet<State>(states).Count && // all values in a list are unique
							!states.Contains(State.PageAppeared) &&
							!states.Contains(State.PageDisappeared) &&
							!states.Contains(State.ElementAppeared) &&
							!states.Contains(State.ElementDisappeared) &&
							states.IndexOf(State.PageAppearing) < states.IndexOf(State.ElementDisappearing) &&
							states.IndexOf(State.ElementAppearing) < states.IndexOf(State.PageDisappearing) &&
							states.IndexOf(State.ElementAppearing) < states.IndexOf(State.ElementDisappearing);
					}
					else
					{
						success =
							states.Count == 8 &&
							states.Count == new HashSet<State>(states).Count &&
							states.IndexOf(State.PageAppearing) < states.IndexOf(State.PageAppeared) &&
							states.IndexOf(State.PageAppearing) < states.IndexOf(State.ElementAppearing) &&
							states.IndexOf(State.PageAppeared) < states.IndexOf(State.PageDisappearing) &&
							states.IndexOf(State.PageDisappearing) < states.IndexOf(State.PageDisappeared) &&
							states.IndexOf(State.ElementAppearing) < states.IndexOf(State.ElementAppeared) &&
							states.IndexOf(State.ElementAppeared) < states.IndexOf(State.ElementDisappearing) &&
							states.IndexOf(State.ElementDisappearing) < states.IndexOf(State.ElementDisappeared);
					}

					checkLabel.Text = success ? successlText : "Failed";
				})
			};

			Master = new ContentPage
			{
				Title = "menu",
				// on iOS the goTestButton overlaps by notch. Until this is corrected added padding-top.
				Padding = new Thickness(5, 40, 5, 0), 
				Content = new StackLayout
				{
					Children =
					{
						goTestButton,
						resultLabel,
						new Button
						{
							Text = switchButtonLabel,
							Command = new Command(() => Application.Current.UseLegacyPageEvents = !Application.Current.UseLegacyPageEvents)
						},
						checkLabel
					}
				}
			};

			Detail = new NavigationPage(
				new ContentPage
				{
					Content = new StackLayout
					{
						Children =
						{
							new Button
							{
								Text = "Open menu",
								Command = new Command(() => IsPresented = true)
							}
						}
					}
				}
			);
		}

#if UITEST
		[Test]
		public void Issue2210_CycleEvents()
		{
			RunningApp.Tap(startTestLabel);
			RunningApp.WaitForElement(successlText);

			// check legacy events
			RunningApp.Tap(switchButtonLabel);
			RunningApp.Tap(startTestLabel);
			RunningApp.WaitForElement(successlText);
		}
#endif
	}
}
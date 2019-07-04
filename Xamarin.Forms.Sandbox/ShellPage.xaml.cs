using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xamarin.Forms.Sandbox
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ShellPage : Shell
	{
		public ShellPage()
		{
			InitializeComponent();
			shellContent.Content = new BackButtonPage();
		}

		public class BackButtonPage : ContentPage
		{
			BackButtonBehavior behavior = new BackButtonBehavior();
			Entry _commandParameter;
			Label _commandResult = new Label() { AutomationId = "CommandResult" };

			public BackButtonPage()
			{
				_commandParameter = new Entry()
				{
					Placeholder = "Command Parameter"
				};

				_commandParameter.TextChanged += (_, __) =>
				{
					if (String.IsNullOrWhiteSpace(_commandParameter.Text))
						behavior.ClearValue(BackButtonBehavior.CommandParameterProperty);
					else
						behavior.CommandParameter = _commandParameter.Text;
				};

				StackLayout layout = new StackLayout();

				layout.Children.Add(new Button()
				{
					Text = "Toggle Behavior",
					Command = new Command(ToggleBehavior)
				});
				layout.Children.Add(new Button()
				{
					Text = "Toggle Command",
					Command = new Command(ToggleCommand)
				});
				layout.Children.Add(_commandParameter);
				layout.Children.Add(_commandResult);
				layout.Children.Add(new Button()
				{
					Text = "Toggle Text",
					Command = new Command(ToggleBackButtonText)
				});
				layout.Children.Add(new Button()
				{
					Text = "Toggle Icon",
					Command = new Command(ToggleIcon)
				});
				layout.Children.Add(new Button()
				{
					Text = "Toggle Is Enabled",
					Command = new Command(ToggleIsEnabled)
				});

				Content = layout;
				ToggleBehavior();
			}

			public void ToggleBehavior()
			{
				if (this.IsSet(Shell.BackButtonBehaviorProperty))
					this.ClearValue(Shell.BackButtonBehaviorProperty);
				else
					this.SetValue(Shell.BackButtonBehaviorProperty, behavior);
			}

			public void ToggleCommand()
			{
				if (behavior.Command == null)
					behavior.Command = new Command<string>(result =>
					{
						_commandResult.Text = result;
					});
				else
					behavior.ClearValue(BackButtonBehavior.CommandProperty);
			}

			public void ToggleBackButtonText()
			{
				if (!String.IsNullOrWhiteSpace(behavior.TextOverride))
					behavior.ClearValue(BackButtonBehavior.TextOverrideProperty);
				else
					behavior.TextOverride = "Text";
			}

			public void ToggleIcon()
			{
				if (behavior.IsSet(BackButtonBehavior.IconOverrideProperty))
					behavior.ClearValue(BackButtonBehavior.IconOverrideProperty);
				else
					behavior.IconOverride = "coffee.png";

			}

			public void ToggleIsEnabled()
			{
				behavior.IsEnabled = !behavior.IsEnabled;
			}
		}
	}
}
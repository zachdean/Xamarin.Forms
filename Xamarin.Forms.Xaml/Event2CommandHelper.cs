using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Xamarin.Forms.Xaml.Internals
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class Event2CommandHelper
	{
		public static EventHandler GetHandlerAndSetBinding(BindableObject owner, string eventName, BindingBase commandBinding)
		{
			var eventCommand = BindableProperty.CreateAttached("eventCommand", typeof(ICommand), typeof(BindableObject), null);

			void OnEventFired(object sender, EventArgs e)
			{
				var parameter = GetParameterFor(owner, eventName);
				if (parameter is BindableProperty bp)
					parameter = owner.GetValue(bp);

				var command = owner.GetValue(eventCommand) as ICommand;
				if (command?.CanExecute(parameter) ?? false)
					command.Execute(parameter);
			}

			owner.SetBinding(eventCommand, commandBinding);
			return OnEventFired;
		}

		public static EventHandler GetHandlerFor(object owner, string eventName, ICommand command)
		{
			void OnEventFired(object sender, EventArgs e)
			{
				var parameter = GetParameterFor(owner, eventName);
				if (parameter is BindableProperty bp && owner is BindableObject bindableObject)
					parameter = bindableObject.GetValue(bp);

				if (command.CanExecute(parameter))
					command.Execute(parameter);
			}

			return OnEventFired;
		}

		public static void SetParameterFor(object owner, string eventName, BindingBase parameterBinding)
		{
			var eventCommandParameter = BindableProperty.CreateAttached("eventCommandParameter", typeof(object), typeof(BindableObject), null);
			(owner as BindableObject)?.SetBinding(eventCommandParameter, parameterBinding);

			SetParameterFor(owner, eventName, eventCommandParameter);
		}

		public static void SetParameterFor(object owner, string eventName, object parameter)
		{
			if (!cache.TryGetValue(owner, out var dict)) {
				dict = new Dictionary<string, object>();
				cache.Add(owner, dict);
			}
			dict[eventName] = parameter;
		}

		static readonly ConditionalWeakTable<object, IDictionary<string, object>> cache = new ConditionalWeakTable<object, IDictionary<string, object>>();

		static object GetParameterFor(object owner, string eventName) => cache.TryGetValue(owner, out var dict) && dict.TryGetValue(eventName, out var param) ? param : null;
	}
}

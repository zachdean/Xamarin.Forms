using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Xamarin.Platform
{
	public class PropertyMapper {
		internal Dictionary<string, (Action<IViewRenderer, IFrameworkElement> Action, bool RunOnUpdateAll)> _mapper = new Dictionary<string, (Action<IViewRenderer, IFrameworkElement> action, bool runOnUpdateAll)> ();

		protected virtual void UpdatePropertyCore (string key, IViewRenderer viewRenderer, IFrameworkElement virtualView)
		{
			if (_mapper.TryGetValue(key, out var action))
			{
				action.Action?.Invoke(viewRenderer, virtualView);
			}
		}

		public void UpdateProperty (IViewRenderer viewRenderer, IFrameworkElement virtualView, string property)
		{
			if (virtualView == null)
				return;
			UpdatePropertyCore (property, viewRenderer, virtualView);
		}

		public void UpdateProperties (IViewRenderer viewRenderer, IFrameworkElement virtualView)
		{
			if (virtualView == null)
				return;
			foreach (var key in Keys) {
				UpdatePropertyCore (key, viewRenderer, virtualView);
			}
		}

		public virtual ICollection<string> Keys => _mapper.Keys;
	}

	public class PropertyMapper<TVirtualView> : PropertyMapper, IEnumerable
		where TVirtualView : IFrameworkElement {
		private PropertyMapper chained;
		public PropertyMapper Chained {
			get => chained;
			set {
				chained = value;
				cachedKeys = null;
			}
		}

		ICollection<string> cachedKeys;
		public override ICollection<string> Keys => cachedKeys ??= (Chained?.Keys.Union (keysForStartup) as ICollection<string> ?? _mapper.Keys);
		ICollection<string> keysForStartup => _mapper.Where(x => x.Value.RunOnUpdateAll).Select(x => x.Key).ToList();

		public int Count => Keys.Count;

		public bool IsReadOnly => false;

		public Action<IViewRenderer, TVirtualView> this [string key] {
			set => Add(key, value, true);
		}

		public PropertyMapper ()
		{
		}

		public PropertyMapper (PropertyMapper chained)
		{
			Chained = chained;
		}

		ActionMapper<TVirtualView> actions;
		public ActionMapper<TVirtualView> Actions {
			get => actions ??= new ActionMapper<TVirtualView>(this);
		}

		protected override void UpdatePropertyCore (string key, IViewRenderer viewRenderer, IFrameworkElement virtualView)
		{
			if (_mapper.TryGetValue (key, out var action))
				action.Action?.Invoke (viewRenderer, virtualView);
			else
				Chained?.UpdateProperty (viewRenderer, virtualView, key);
		}

		public void Add (string key, Action<IViewRenderer, TVirtualView> action)
			=> this [key] = action;

		public void Add(string key, Action<IViewRenderer, TVirtualView> action, bool ignoreOnStartup)
			=> _mapper[key] = ((r, v) => action?.Invoke(r, (TVirtualView)v), ignoreOnStartup);

		IEnumerator IEnumerable.GetEnumerator () => _mapper.GetEnumerator ();

		public class ActionMapper<TView>
			where TView : TVirtualView, IFrameworkElement
		{
			public ActionMapper(PropertyMapper<TView> propertyMapper)
			{
				PropertyMapper = propertyMapper;
			}

			public PropertyMapper<TView> PropertyMapper { get; }

			public Action<IViewRenderer, TView> this[string key]
			{
				set => PropertyMapper._mapper[key] = ((r, v) => value?.Invoke(r, (TView)v), false);
			}
		}

	}

}

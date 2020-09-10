using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace System.Maui
{
	public class PropertyMapper
	{
		List<string> _actionKeys;
		List<string> _updateKeys;

		internal Dictionary<string, (Action<IViewHandler, IView> Action, bool RunOnUpdateAll)> _genericMap = new Dictionary<string, (Action<IViewHandler, IView> action, bool runOnUpdateAll)>();

		protected virtual void UpdateProperty(string key, IViewHandler viewHandler, IView virtualView)
		{
			var action = Get(key);
			action.Action?.Invoke(viewHandler, virtualView);
		}

		public void UpdateProperty(IViewHandler viewHandler, IView virtualView, string property)
		{
			if (virtualView == null)
				return;

			UpdateProperty(property, viewHandler, virtualView);
		}

		public void UpdateProperties(IViewHandler viewHandler, IView virtualView)
		{
			if (virtualView == null)
				return;

			foreach (var key in UpdateKeys)
			{
				UpdateProperty(key, viewHandler, virtualView);
			}
		}

		public virtual ICollection<string> Keys => _genericMap.Keys;

		protected List<string> PopulateKeys(ref List<string> returnList)
		{
			_updateKeys = new List<string>();
			_actionKeys = new List<string>();

			foreach (var key in Keys)
			{
				var result = Get(key);
				if (result.RunOnUpdateAll)
					_updateKeys.Add(key);
				else
					_actionKeys.Add(key);

			}

			return returnList;
		}

		protected virtual void ClearKeyCache()
		{
			_updateKeys = null;
			_actionKeys = null;
		}

		public virtual (Action<IViewHandler, IView> Action, bool RunOnUpdateAll) Get(string key)
		{
			_genericMap.TryGetValue(key, out var action);
			return action;
		}

		public virtual IReadOnlyList<string> ActionKeys => _actionKeys ?? PopulateKeys(ref _actionKeys);
		public virtual IReadOnlyList<string> UpdateKeys => _updateKeys ?? PopulateKeys(ref _updateKeys);
	}

	public class ActionMapper<TVirtualView>
	where TVirtualView : IView
	{
		public ActionMapper(PropertyMapper<TVirtualView> propertyMapper)
		{
			PropertyMapper = propertyMapper;
		}

		public PropertyMapper<TVirtualView> PropertyMapper { get; }

		public Action<IViewHandler, TVirtualView> this[string key]
		{
			set => PropertyMapper._genericMap[key] = ((r, v) => value?.Invoke(r, (TVirtualView)v), false);
		}
	}

	public class PropertyMapper<TVirtualView> : PropertyMapper, IEnumerable
		where TVirtualView : IView
	{
		PropertyMapper _chained;
		ICollection<string> _cachedKeys;
		ActionMapper<TVirtualView> _actions;

		public PropertyMapper Chained
		{
			get => _chained;
			set
			{
				_chained = value;
				ClearKeyCache();
			}
		}

		public override ICollection<string> Keys => _cachedKeys ??= (Chained?.Keys.Union(_genericMap.Keys).ToList() as ICollection<string> ?? _genericMap.Keys);

		public int Count => Keys.Count;

		public bool IsReadOnly => false;

		public Action<IViewHandler, TVirtualView> this[string key]
		{
			set => _genericMap[key] = ((r, v) => value?.Invoke(r, (TVirtualView)v), true);
		}

		public PropertyMapper()
		{
		}

		public PropertyMapper(PropertyMapper chained)
		{
			Chained = chained;
		}

		public ActionMapper<TVirtualView> Actions
		{
			get => _actions ??= new ActionMapper<TVirtualView>(this);
		}

		protected override void ClearKeyCache()
		{
			base.ClearKeyCache();
			_cachedKeys = null;
		}

		public override (Action<IViewHandler, IView> Action, bool RunOnUpdateAll) Get(string key)
		{
			if (_genericMap.TryGetValue(key, out var action))
				return action;
			else
				return Chained?.Get(key) ?? (null, false);
		}

		public void Add(string key, Action<IViewHandler, TVirtualView> action)
			=> this[key] = action;

		public void Add(string key, Action<IViewHandler, TVirtualView> action, bool ignoreOnStartup)
			=> _genericMap[key] = ((r, v) => action?.Invoke(r, (TVirtualView)v), ignoreOnStartup);

		IEnumerator IEnumerable.GetEnumerator() => _genericMap.GetEnumerator();
	}
}
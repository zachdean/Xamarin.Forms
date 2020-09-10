using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace System.Maui
{
	public static class Registrar
	{
		public static Registrar<IView, IViewHandler> Handlers { get; private set; }
		static Registrar()
		{
			Handlers = new Registrar<IView, IViewHandler> ();
		}
	}

	public class Registrar<TType, TTypeRender>
	{
		internal Dictionary<Type, Type> _handler = new Dictionary<Type, Type>();

		public void Register<TView, TRender>()
			where TView : TType
				where TRender : TTypeRender
		{
			Register(typeof(TView), typeof(TRender));
		}

		public void Register(Type view, Type handler)
		{
			_handler[view] = handler;
		}
		public TTypeRender GetHandler<T>()
		{
			return GetHandler(typeof(T));
		}

		internal List<KeyValuePair<Type,Type>> GetViewType(Type type) =>
			_handler.Where(x => isType(x.Value,type)).ToList();

		bool isType(Type type, Type type2)
		{
			if (type == type2)
				return true;
			if (!type.IsGenericType)
				return false;
			var paramerter = type.GetGenericArguments();
			return paramerter[0] == type2;
		}

		public TTypeRender GetHandler(Type type)
		{
			List<Type> types = new List<Type> { type };
			Type baseType = type.BaseType;
			while (baseType != null)
			{
				types.Add(baseType);
				baseType = baseType.BaseType;
			}

			foreach (var t in types)
			{
				var Handler = getHandler(t);
				if (Handler != null)
					return Handler;
			}
			return default;
		}

		public Type GetHandlerType(Type type)
		{
			List<Type> types = new List<Type> { type };
			Type baseType = type.BaseType;
			while (baseType != null)
			{
				types.Add(baseType);
				baseType = baseType.BaseType;
			}

			foreach (var t in types)
			{
				if (_handler.TryGetValue(t, out var returnType))
					return returnType;
			}
			return null;
		}

		TTypeRender getHandler(Type t)
		{
			if (!_handler.TryGetValue(t, out var Handler))
				return default;
			try
			{
				var newObject = Activator.CreateInstance(Handler);
				return (TTypeRender)newObject;
			}
			catch (Exception ex)
			{
				if (Debugger.IsAttached)
					throw ex;
			}

			return default;
		}
	}
}

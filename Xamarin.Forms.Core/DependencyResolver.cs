using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Xamarin.Forms.Internals
{
	public interface IVisualHandlerResolver
	{
		Type VisualType { get; }
		object GetHandler(VisualRendererFactoryArgs args);
	}

	public class VisualRendererFactoryArgs
	{
		public object[] Args;
		public object Source;
		public Type HandlerType;
	}

	public static class DependencyResolver
	{
		static Func<Type, object[], object> Resolver { get; set; }
		static Dictionary<Type, IVisualHandlerResolver[]> VisualHandlerResolvers { get; set; } = new Dictionary<Type, IVisualHandlerResolver[]>();

		public static void ResolveVisualUsing(IVisualHandlerResolver visualHandlerResolver)
		{
			ResolveVisualUsing(visualHandlerResolver.VisualType, new[] { visualHandlerResolver });
		}
		public static void ResolveVisualUsing(Type visualType, IVisualHandlerResolver[] visualHandlerResolver)
		{
			VisualHandlerResolvers[visualType] = visualHandlerResolver;
		}

		public static void ResolveUsing(Func<Type, object[], object> resolver)
		{
			Resolver = resolver;
		}

		public static void ResolveUsing(Func<Type, object> resolver)
		{
			Resolver = (type, objects) => resolver.Invoke(type);
		}

		internal static object Resolve(Type type, params object[] args)
		{
			var result = Resolver?.Invoke(type, args);

			if (result != null)
			{
				if (!type.IsInstanceOfType(result))
				{
					throw new InvalidCastException("Resolved instance is not of the correct type.");
				}
			}

			return result;
		}

		internal static object ResolveOrCreate(Type type) => ResolveOrCreate(type, null, null);

		internal static object ResolveOrCreate(Type type, Type visualType, object source, params object[] args)
		{
			var result = Resolve(type, args);

			if (result != null) return result;

			IVisualHandlerResolver[] handlers = null;
			if (VisualHandlerResolvers.TryGetValue(visualType, out handlers))
			{
				var factoryArgs = new VisualRendererFactoryArgs() { Args = args, Source = source, HandlerType = type };
				foreach(var handler in handlers)
					result = handler.GetHandler(factoryArgs);
			}

			if (result != null)
				return result;

			if (args.Length > 0)
			{
				// This is by no means a general solution to matching with the correct constructor, but it'll
				// do for finding Android renderers which need Context (vs older custom renderers which may still use
				// parameterless constructors)
				if (type.GetTypeInfo().DeclaredConstructors.Any(info => info.GetParameters().Length == args.Length))
				{
					return Activator.CreateInstance(type, args);
				}
			}
			
			return Activator.CreateInstance(type);
		}
	}
}
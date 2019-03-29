using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Xamarin.Forms
{
	public static class Routing
	{
		static int s_routeCount = 0;

#if NETSTANDARD1_0
		static Dictionary<string, RouteFactory> s_routes = new Dictionary<string, RouteFactory>();
#else
		static Dictionary<string, RouteFactory> s_routes = new Dictionary<string, RouteFactory>(StringComparer.InvariantCultureIgnoreCase);
#endif

		internal const string ImplicitPrefix = "IMPL_";

		static bool IsImplict(string route) => route.StartsWith(ImplicitPrefix, StringComparison.Ordinal);

		internal static string GenerateImplicitRoute (string source)
		{
			return IsImplict(source) ? source : ImplicitPrefix + source;
		}

		internal static bool CompareWithRegisteredRoutes(string compare) => s_routes.ContainsKey(compare);

		internal static bool CompareRoutes(string route, string compare, out bool isImplicit)
		{
			if (isImplicit = IsImplict(route))
				route = route.Substring(ImplicitPrefix.Length);

			if (IsImplict(compare))
				throw new Exception();

			return route == compare;
		}

		public static readonly BindableProperty RouteProperty =
			BindableProperty.CreateAttached("Route", typeof(string), typeof(Routing), null, 
				defaultValueCreator: CreateDefaultRoute);

		static object CreateDefaultRoute(BindableObject bindable)
		{
			return bindable.GetType().Name + ++s_routeCount;
		}

		public static Element GetOrCreateContent(string route)
		{
			Element result = null;

#if NETSTANDARD1_0
			if (s_routes.TryGetValue(route.ToLowerInvariant(), out var content))
				result = content.GetOrCreate();
#else
			if (s_routes.TryGetValue(route, out var content))
				result = content.GetOrCreate();
#endif

			if (result == null)
			{
				// okay maybe its a type, we'll try that just to be nice to the user
				var type = Type.GetType(route);
				if (type != null)
					result = Activator.CreateInstance(type) as Element;
			}

			if (result != null)
				SetRoute(result, route);

			return result;
		}

		public static string GetRoute(Element obj)
		{
			return (string)obj.GetValue(RouteProperty);
		}

		public static void RegisterRoute(string route, RouteFactory factory)
		{
			ValidateRoute(route);

#if NETSTANDARD1_0
			route = route.ToLowerInvariant();
#endif
			s_routes[route] = factory;
		}

		public static void RegisterRoute(string route, Type type)
		{
			ValidateRoute(route);

#if NETSTANDARD1_0
			route = route.ToLowerInvariant();
#endif
			s_routes[route] = new TypeRouteFactory(type);
		}

		public static void SetRoute(Element obj, string value)
		{
			obj.SetValue(RouteProperty, value);
		}

		static void ValidateRoute(string route)
		{
			if (string.IsNullOrWhiteSpace(route))
				throw new ArgumentNullException("Route cannot be an empty string");

			var uri = new Uri(route, UriKind.RelativeOrAbsolute);

			var parts = uri.OriginalString.Split(new [] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
			foreach (var part in parts)
			{
				if (IsImplict(part))
					throw new ArgumentException($"Route contains invalid characters in \"{part}\"");
			}
		}

		class TypeRouteFactory : RouteFactory
		{
			readonly Type _type;

			public TypeRouteFactory(Type type)
			{
				_type = type;
			}

			public override Element GetOrCreate()
			{
				return (Element)Activator.CreateInstance(_type);
			}
		}
	}
}
using System;
using System.Linq;
using System.Threading.Tasks;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using IOPath = System.IO.Path;

namespace Xamarin.Platform
{
	public static class ResourceManager
	{
		const string _drawableDefType = "drawable";

		static Type? _drawableClass;
		static Type? _resourceClass;
		static Type? _styleClass;
		static Type? _layoutClass;

		public static Bitmap? GetBitmap(this Resources resource, string name, Context context)
		{
			return BitmapFactory.DecodeResource(resource, IdFromTitle(name, DrawableClass, _drawableDefType, resource, context.PackageName));
		}

		public static Task<Bitmap?> GetBitmapAsync(this Resources resource, string name, Context context)
		{
			return BitmapFactory.DecodeResourceAsync(resource, IdFromTitle(name, DrawableClass, _drawableDefType, resource, context.PackageName));
		}

		public static Type DrawableClass
		{
			get
			{
				if (_drawableClass == null)
					_drawableClass = FindType("Drawable", "Resource_Drawable");
				return _drawableClass;
			}
			set
			{
				_drawableClass = value;
			}
		}

		public static Type ResourceClass
		{
			get
			{
				if (_resourceClass == null)
					_resourceClass = FindType("Id", "Resource_Id");
				return _resourceClass;
			}
			set
			{
				_resourceClass = value;
			}
		}

		public static Type StyleClass
		{
			get
			{
				if (_styleClass == null)
					_styleClass = FindType("Style", "Resource_Style");
				return _styleClass;
			}
			set
			{
				_styleClass = value;
			}
		}

		public static Type LayoutClass
		{
			get
			{
				if (_layoutClass == null)
					_layoutClass = FindType("Layout", "Resource_Layout");
				return _layoutClass;
			}
			set
			{
				_layoutClass = value;
			}
		}

		static int IdFromTitle(string title, Type type)
		{
			if (title == null)
				return 0;

			string name = IOPath.GetFileNameWithoutExtension(title);
			int id = GetId(type, name);
			return id;
		}

		static int IdFromTitle(string title, Type resourceType, string defType, Context context)
		{
#pragma warning disable CS8604 // Possible null reference argument.
			return IdFromTitle(title, resourceType, defType, context.Resources, context.PackageName);
#pragma warning restore CS8604 // Possible null reference argument.
		}
	
		static int IdFromTitle(string title, Type resourceType, string defType, global::Android.Content.Res.Resources resource, string? packageName)
		{
			int id = 0;
			if (title == null)
				return id;

			string name = IOPath.GetFileNameWithoutExtension(title);

			id = GetId(resourceType, name);

			if (id > 0)
				return id;

			if (packageName != null)
			{
				id = resource.GetIdentifier(name, defType, packageName);

				if (id > 0)
					return id;
			}

			id = resource.GetIdentifier(name, defType, null);

			return id;
		}

		static int GetId(Type type, string memberName)
		{
			// This may legitimately be null in designer scenarios
			if (type == null)
				return 0;

			object? value = null;
			var fields = type.GetFields();
			for (int i = 0; i < fields.Length; i++)
			{
				var field = fields[i];
				if (field.Name == memberName)
				{
					value = field.GetValue(type);
					break;
				}
			}

			if (value == null)
			{
				var properties = type.GetProperties();
				for (int i = 0; i < properties.Length; i++)
				{
					var prop = properties[i];
					if (prop.Name == memberName)
					{
						value = prop.GetValue(type);
						break;
					}
				}
			}

			if (value is int result)
				return result;
			return 0;
		}

		static Type FindType(string name, string altName)
		{
			//the app assembly
			var assembly = Registrar.CallingAssembly;
			return assembly.GetTypes().FirstOrDefault(x => x.Name == name || x.Name == altName);
		}
	}
}

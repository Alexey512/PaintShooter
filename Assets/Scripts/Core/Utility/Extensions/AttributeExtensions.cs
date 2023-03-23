using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Utility.Extensions
{
	public static class AttributeExtensions
	{
		public static T GetAttribute<T>(this Type type) where T : Attribute
		{
			object[] attributes = type.GetCustomAttributes(true);

			foreach (object attribute in attributes)
				if (attribute is T targetAttribute)
					return targetAttribute;

			return null;
		}

		public static List<Type> GetInheritedTypes(this Type type)
		{
			return Assembly.GetAssembly(type).GetTypes().Where(t => t.IsSubclassOf(type)).ToList();
		}

		public static TAttribute GetAttribute<TAttribute>(this Enum value) where TAttribute : Attribute
		{
			var enumType = value.GetType();
			var name = Enum.GetName(enumType, value);
			return enumType.GetField(name).GetCustomAttributes(false).OfType<TAttribute>().SingleOrDefault();
		}
	}
}

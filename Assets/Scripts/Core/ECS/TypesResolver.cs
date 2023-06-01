using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ECS
{
    public struct TypeInfo
    {
        public int Id;

        public Type Type;
    }

    public static class TypesResolver
    {
        private static int _typesCounter;

        private static Dictionary<Type, TypeInfo> _types = new Dictionary<Type, TypeInfo>();

        public static TypeInfo GetTypeInfo(Type type)
        {
            if (_types.TryGetValue(type, out var info))
                return info;
            var typeInfo = new TypeInfo { Type = type, Id = _typesCounter++ };
            _types[type] = typeInfo;
            return typeInfo;
        }

        public static TypeInfo GetTypeInfo<T>()
        {
            return GetTypeInfo(typeof(T));
        }
    }

    public static class ComponentType<T> where T : class, IComponent
    {
        public static TypeInfo Info { get; private set; }

        static ComponentType()
        {
            Info = TypesResolver.GetTypeInfo<T>();
        }
    }

    public static class SystemType<T> where T : class, ISystem
    {
        public static TypeInfo Info { get; private set; }

        static SystemType()
        {
            Info = TypesResolver.GetTypeInfo<T>();
        }
    }
}

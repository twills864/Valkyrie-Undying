using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Assets.Util
{
    public static class ReflectionUtil
    {
        private static BindingFlags PublicFlags => BindingFlags.Public| BindingFlags.Instance;
        private static BindingFlags PrivateFlags => BindingFlags.NonPublic | BindingFlags.Instance;
        private static BindingFlags PublicOrPrivateFlags => BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        /// <summary>
        /// Gets each private field of a given object that is both a subclass of PooledObject
        /// and has the [SerializeField] attribute.
        /// </summary>
        /// <param name="source">The object to inspect</param>
        /// <returns>Each private poolable prefab field of the given object.</returns>
        public static IEnumerable<PooledObject> GetPrivatePoolablePrefabFields(object source)
        {
            var sourceType = source.GetType();
            var allSourceFields = sourceType.GetFields(PrivateFlags);
            var prefabFields = allSourceFields.Where(x => IsPoolablePrefab(x));

            var ret = prefabFields.Select(x => (PooledObject)x.GetValue(source));
            return ret.ToList();
        }

        /// <summary>
        /// Gets the type of each private field of a given object that is both a subclass of PooledObject
        /// and has the [SerializeField] attribute.
        /// </summary>
        /// <param name="source">The object to inspect</param>
        /// <returns>Each private poolable prefab field type of the given object.</returns>

        public static IEnumerable<Type> GetPrivatePoolablePrefabFieldTypes(object source)
        {
            var sourceType = source.GetType();
            var allSourceFields = sourceType.GetFields(PrivateFlags);
            var prefabFields = allSourceFields.Where(x => IsPoolablePrefab(x));

            var ret = prefabFields.Select(x => x.FieldType);
            return ret;
        }

        /// <summary>
        /// Returns true if a FieldInfo has the SerializeField tag,
        /// and is also derived from the PooledObject class.
        /// Returns false otherwise.
        /// </summary>
        /// <param name="fieldInfo">The FieldInfo to inspect</param>
        /// <returns>True if the <paramref name="fieldInfo"/> is a poolable prefab; false otherwise.</returns>
        public static bool IsPoolablePrefab(FieldInfo fieldInfo)
        {
            var pooledObjectType = typeof(PooledObject);
            var serializeType = typeof(SerializeField);

            bool ret = fieldInfo.GetCustomAttributes(serializeType) != null
                && pooledObjectType.IsAssignableFrom(fieldInfo.FieldType);
            return ret;
        }

        /// <summary>
        /// Returns a list of each type in the current assembly that is a
        /// subclass of <typeparamref name="TType"/>.
        /// </summary>
        /// <typeparam name="TType">The type to retrieve a list of subclasses for.</typeparam>
        /// <returns>A list of each type that is a subclass of <typeparamref name="TType"/>.</returns>
        public static List<Type> GetTypesSubclassableFrom<TType>()
        {
            return GetTypesSubclassableFrom(typeof(TType));
        }

        /// <summary>
        /// Returns a list of each type in the current assembly that is a
        /// subclass of the given <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to retrieve a list of subclasses for.</param>
        /// <returns>A list of each type that is a subclass of <paramref name="type"/>.</returns>
        public static List<Type> GetTypesSubclassableFrom(Type type)
        {
            var ret = type.Assembly.GetTypes().Where(t => t.IsSubclassOf(type)).ToList();
            return ret;
        }
    }
}

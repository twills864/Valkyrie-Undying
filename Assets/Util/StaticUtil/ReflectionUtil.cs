using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;

namespace Assets.Util
{
    public static class ReflectionUtil
    {
        private const string StrictlyDebugging = "This method should only be used for debugging. "
            + "Do not use in release build under any circumstances.";

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

        /// <summary>
        /// Leverages the Activator class to create a new instance of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of object to create.</typeparam>
        /// <returns>The newly-created object.</returns>
        public static T CreateNew<T>()
        {
            var ret = Activator.CreateInstance<T>();
            return ret;
        }

        /// <summary>
        /// Leverages the Activator class to create a new instance of the specified type.
        /// </summary>
        /// <param name="type">The type of object to create.</param>
        /// <returns>The newly-created object.</returns>
        public static object CreateNew(Type type)
        {
            var ret = Activator.CreateInstance(type);
            return ret;
        }

#if UNITY_EDITOR

        [Obsolete(StrictlyDebugging)]
        public static bool TryGetProperty<T>(object target, string propertyName, out T value)
        {
            Type type = target.GetType();
            PropertyInfo property = type.GetProperty(propertyName, PublicOrPrivateFlags);

            bool ret;

            if(property != null)
            {
                value = (T)property.GetValue(target);
                ret = true;
            }
            else
            {
                value = default;
                ret = false;
            }

            return ret;
        }

        [Obsolete(StrictlyDebugging)]
        public static bool TryGetField<T>(object target, string fieldName, out T value)
        {
            Type type = target.GetType();
            FieldInfo field = type.GetField(fieldName, PublicOrPrivateFlags);

            bool ret;

            if (field != null)
            {
                value = (T)field.GetValue(target);
                ret = false;
            }
            else
            {
                value = default;
                ret = false;
            }

            return ret;
        }

        [Obsolete(StrictlyDebugging)]
        public static T GetMember<T>(object target, string member)
        {
            bool propertyFound = TryGetProperty(target, member, out T value);
            if (!propertyFound)
                TryGetField(target, member, out value);

            return value;
        }

#endif
    }
}

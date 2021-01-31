using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Util
{
    public static class ReflectionUtil
    {
        private static BindingFlags PrivateFlags => BindingFlags.NonPublic | BindingFlags.Instance;

        /// <summary>
        /// Gets each private field of a given object that is both a subclass of PooledObject
        /// and has the [SerializeField] attribute.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<PooledObject> GetPrivatePoolablePrefabFields<TSource>(TSource source)
        {
            var sourceType = typeof(TSource);
            var allSourceFields = sourceType.GetFields(PrivateFlags);
            var prefabFields = allSourceFields.Where(x => IsPoolablePrefab(x));
            //var prefabFields = allSourceFields.Where(x => x.GetCustomAttributes(serializeType) != null
            //    && pooledObjectType.IsAssignableFrom(x.FieldType));

            var ret = prefabFields.Select(x => (PooledObject)x.GetValue(source));
            return ret;
        }

        public static bool IsPoolablePrefab(FieldInfo fieldInfo)
        {
            var pooledObjectType = typeof(PooledObject);
            var serializeType = typeof(SerializeField);

            bool ret = fieldInfo.GetCustomAttributes(serializeType) != null
                && pooledObjectType.IsAssignableFrom(fieldInfo.FieldType);
            return ret;
        }
    }
}

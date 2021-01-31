using Assets.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util.ObjectPooling
{
    // This class was created for ease and speed of development.
    // It should be replaced with a hard-coded version upon release.
    public static class PoolManager
    {
        public static Dictionary<Type, ObjectPool<PooledObject>> AllPools
             = new Dictionary<Type, ObjectPool<PooledObject>>();

        public static TReturn Get<TReturn>() where TReturn : PooledObject
        {
            var pool = AllPools[typeof(TReturn)];
            var ret = pool.Get<TReturn>();
            ret.ActivateSelf();
            return ret;
        }

        public static void SendHome<T>(T item) where T : PooledObject
        {
            var type = item.GetType();

            var pool = AllPools[type];
            pool.Add(item);
        }

        public static void InitPool(IEnumerable<PooledObject> prefabs)
        {
            foreach(var prefab in prefabs)
            {
                var type = prefab.GetType();

                // AllPools.Add() will throw an exception if we've already added such a prefab
                AllPools.Add(type, new ObjectPool<PooledObject>(prefab));
            }
        }

        public static void DebugInfo()
        {
            foreach (var kvp in AllPools)
            {
                var pool = kvp.Value;
                if (pool.HasHadActivity)
                {
                    var type = kvp.Key;
                    DebugUI.SetDebugLabel(type.Name, pool.DebugInfo);
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Util.ObjectPooling
{
    /// <summary>
    /// A list of Object Pools that are grouped together by a common base class.
    /// </summary>
    public abstract class PoolList : MonoBehaviour
    {
        public virtual void Init() { }
    }

    /// <summary>
    /// A list of Object Pools that are grouped together by a common base class <typeparamref name="T"/>.
    /// </summary>
    public abstract class PoolList<T> : PoolList where T : PooledObject
    {
        /// <summary>
        /// Returns each pool represented by this PoolList.
        /// </summary>
        protected IEnumerable<ObjectPool<T>> Pools => PoolMap.Values;

        /// <summary>
        /// A dictionary in which each pool represented by this PoolList
        /// is indexed by the type of the pool.
        /// </summary>
        protected Dictionary<Type, ObjectPool<T>> PoolMap { get; private set; }

        public override void Init()
        {
            LoadPoolMap();
        }

        /// <summary>
        /// Uses reflection to initialize a new ObjectPool&lt;<typeparamref name="T"/>&gt;
        /// for each prefab contained in this PoolList.
        /// </summary>
        protected void LoadPoolMap()
        {
            var prefabTypes = ReflectionUtil.GetPrivatePoolablePrefabFields(this);
            PoolMap = prefabTypes.ToDictionary(x => x.GetType(), x => new ObjectPool<T>((T)x));
        }

        /// <summary>
        /// Accesses the Object Pool indexed by<typeparamref name="T"/>
        /// and returns a fresh instance from that Pool.
        /// </summary>
        /// <typeparam name="TGet">The type of object to return.</typeparam>
        /// <returns>A fresh instance of <typeparamref name="TGet"/> from the appropriate Object Pool.</returns>
        public TGet Get<TGet>() where TGet : T
        {
            var type = typeof(TGet);
            var pool = PoolMap[type];
            var ret = (TGet)pool.Get();
            return ret;
        }

        /// <summary>
        /// Calls RunFrames() on each Object Pool in this list
        /// using a given delta time.
        /// </summary>
        /// <param name="deltaTime">The represented amount of time that has passed
        /// since the last frame.</param>
        public virtual void RunFrames(float deltaTime)
        {
            foreach (var pool in Pools)
                pool.RunFrames(deltaTime);
        }

        /// <summary>
        /// Calls RecolorObjects() on each Object Pool in this list
        /// using a given color.
        /// </summary>
        /// <param name="color">The color to give to each object in each Object Pool.</param>
        public virtual void RecolorElements(Color color)
        {
            foreach (var pool in Pools)
                pool.RecolorObjects(color);
        }
    }
}

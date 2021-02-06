using System;
using System.Collections.Generic;
using System.Linq;
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
    /// /// <inheritdoc/>
    public abstract class PoolList<T> : PoolList where T : PooledObject
    {
        /// <summary>
        /// An array representation of each pool represented by this PoolList.
        /// </summary>
        protected ObjectPool<T>[] Pools { get; private set; }

        /// <summary>
        /// A dictionary in which each pool represented by this PoolList
        /// is indexed by the type of the pool.
        /// </summary>
        protected Dictionary<Type, ObjectPool<T>> PoolMap
        {
            get => _poolMap;
            private set
            {
                _poolMap = value;
                Pools = _poolMap.Values.ToArray();
            }
        }
        private Dictionary<Type, ObjectPool<T>> _poolMap;

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
            try
            {
                var prefabTypes = ReflectionUtil.GetPrivatePoolablePrefabFields(this);
                PoolMap = prefabTypes.ToDictionary(x => x.GetType(), x => new ObjectPool<T>((T)x));
            }
            catch (Exception ex)
            {
                var prefabTypes = ReflectionUtil.GetPrivatePoolablePrefabFields(this);
                var prefabs = ReflectionUtil.GetPrivatePoolablePrefabFields(this);
                throw;
            }
        }

        /// <summary>
        /// Accesses the Object Pool indexed by<typeparamref name="T"/>
        /// and returns a fresh instance from that Pool.
        /// </summary>
        /// <typeparam name="TGet">The type of object to return.</typeparam>
        /// <returns>A fresh instance of <typeparamref name="TGet"/> from the appropriate Object Pool.</returns>
        public TGet Get<TGet>() where TGet : T
        {
            try
            {
                var type = typeof(TGet);
                var pool = PoolMap[type];
                var ret = (TGet)pool.Get();
                return ret;
            } catch (Exception ex)
            {
                var type = typeof(TGet);
                throw;
            }
        }

        /// <summary>
        /// Accesses the object pool indexed by<typeparamref name="TGet"/>,
        /// returns a fresh instance from that pool,
        /// and sets its initial position to the specified position.
        /// </summary>
        /// <typeparam name="TGet">The type of object to return.</typeparam>
        /// <param name="position">The position to give to the fresh instance.</param>
        /// <returns></returns>
        public TGet Get<TGet>(Vector2 position) where TGet : T
        {
            var ret = Get<TGet>();
            ret.transform.position = position;
            return ret;
        }

        /// <summary>
        /// Accesses the Object Pool indexed by <typeparamref name="T"/>
        /// and returns a given number of fresh instances from that Pool.
        /// </summary>
        /// <typeparam name="TGet">The type of object to return.</typeparam>
        /// <param name="amountToGet">The number of fresh instances to get.</param>
        /// <returns>A fresh instance of <typeparamref name="TGet"/> from the appropriate Object Pool.</returns>
        public TGet[] Get<TGet>(int amountToGet) where TGet : T
        {
            TGet[] ret = LinqUtil.Array(amountToGet, () => Get<TGet>());
            return ret;
        }

        /// <summary>
        /// Accesses the Object Pool indexed by <typeparamref name="T"/>
        /// and returns the associated prefab from that Pool.
        /// </summary>
        /// <typeparam name="TGet">The type of object to return.</typeparam>
        /// <returns>The associated prefab.</returns>
        public TGet GetPrefab<TGet>() where TGet : T
        {
            var dbType = typeof(TGet);
            var map = PoolMap[typeof(TGet)];
            var ret = (TGet) map.ObjectPrefab;
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

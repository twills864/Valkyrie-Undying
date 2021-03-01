using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Util;
using UnityEngine;

namespace Assets.ObjectPooling
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

        protected virtual void OnPoolMapSet() { }
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
                OnPoolMapSet();
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
        /// and sets its initial position to a specified <paramref name="position"/>.
        /// </summary>
        /// <typeparam name="TGet">The type of object to return.</typeparam>
        /// <param name="position">The position to give to the fresh instance.</param>
        /// <returns>The initialized fresh instance of <typeparamref name="TGet"/> from the appropriate Object Pool.</returns>
        public TGet Get<TGet>(Vector2 position) where TGet : T
        {
            var ret = Get<TGet>();
            ret.transform.position = position;
            return ret;
        }

        /// <summary>
        /// Accesses the object pool indexed by<typeparamref name="TGet"/>,
        /// returns a fresh instance from that pool,
        /// sets its initial position to a specified <paramref name="position"/>,
        /// and sets its initial velocity to a specified <paramref name="velocity"/>.
        /// </summary>
        /// <typeparam name="TGet">The type of object to return.</typeparam>
        /// <param name="position">The position to give to the fresh instance.</param>
        /// <param name="velocity">The velocity to give to the fresh instance.</param>
        /// <returns>The initialized fresh instance of <typeparamref name="TGet"/> from the appropriate Object Pool.</returns>
        public TGet Get<TGet>(Vector2 position, Vector2 velocity) where TGet : T
        {
            var ret = Get<TGet>(position);
            ret.Velocity = velocity;
            return ret;
        }

        /// <summary>
        /// Accesses the Object Pool indexed by <typeparamref name="T"/>
        /// and returns a given number of fresh instances from that Pool.
        /// </summary>
        /// <typeparam name="TGet">The type of object to return.</typeparam>
        /// <param name="amountToGet">The number of fresh instances to get.</param>
        /// <returns>A fresh instance of <typeparamref name="TGet"/> from the appropriate Object Pool.</returns>
        public TGet[] GetMany<TGet>(int amountToGet) where TGet : T
        {
            var type = typeof(TGet);
            var pool = PoolMap[type];

            TGet[] ret = pool.GetMany<TGet>(amountToGet);
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
            var type = typeof(TGet);
            var pool = PoolMap[type];
            var ret = (TGet) pool.ObjectPrefab;
            return ret;
        }

        /// <summary>
        /// Returns the ObjectPool associated with the type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="TGet">The type of ObjectPool to return.</typeparam>
        /// <returns>The associated ObjectPool.</returns>
        public ObjectPool<T> GetPool<TGet>()
        {
            var type = typeof(TGet);
            var ret = PoolMap[type];
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

        /// <summary>
        /// Returns an IEnumerable of containing every active object
        /// from every Object Pool in this PoolList.
        /// </summary>
        /// <returns>Every active object within this PoolList.</returns>
        protected IEnumerable<T> GetAllActiveObjects()
        {
            var allActiveObjects = Pools.Select(x => x.ActiveObjects)
                .SelectMany(x => x);
            return allActiveObjects;
        }

        /// <summary>
        /// Attempts to retrieve a random active object from this PoolList.
        /// Returns false if there are no active objects to retrieve.
        /// Returns true otherwise.
        /// </summary>
        /// <param name="randomObject">The random object that was retrieved.</param>
        /// <returns>True if a random object was retrieved; false otherwise.</returns>
        public bool TryGetRandomObject(out T randomObject)
        {
            var potentials = GetAllActiveObjects();
            var ret = RandomUtil.TryGetRandomElement(potentials, out randomObject);
            return ret;
        }

        /// <summary>
        /// Attempts to retrieve a random active object from this PoolList
        /// that isn't the specified <paramref name="exclusion"/>.
        /// Returns false if there are no active objects to retrieve.
        /// Returns true otherwise.
        /// </summary>
        /// <param name="exclusion"></param>
        /// <param name="randomObject"></param>
        /// <returns></returns>
        public bool TryGetRandomObjectExcluding(T exclusion, out T randomObject)
        {
            var potentials = GetAllActiveObjects()
                .Where(x => x != exclusion);
            var ret = RandomUtil.TryGetRandomElement(potentials, out randomObject);
            return ret;
        }

        /// <summary>
        /// Attempts to retrieve up to a specified number of random elements from this PoolList.
        /// May retrieve fewer than the specified number if there are fewer active elements than requested.
        /// </summary>
        /// <param name="maxNumToGet">The upper limit of elements to get.</param>
        /// <returns></returns>
        public T[] GetUpToXRandomElements(int maxNumToGet)
        {
            var source = GetAllActiveObjects().ToArray();
            var ret = RandomUtil.GetUpToXRandomElements(source, maxNumToGet);
            return ret;
        }
    }
}

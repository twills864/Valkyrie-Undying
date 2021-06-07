using System;
using System.Collections.Generic;
using System.Linq;
using Assets.ColorManagers;
using Assets.Util;
using UnityEngine;

namespace Assets.ObjectPooling
{
    /// <summary>
    /// A list of Object Pools that are grouped together by a common base class.
    /// </summary>
    public abstract class PoolList : MonoBehaviour
    {
        public abstract void Init(in ColorManager colorManager);
    }

    /// <summary>
    /// A list of Object Pools that are grouped together by a common base class <typeparamref name="T"/>.
    /// </summary>
    /// /// <inheritdoc/>
    public abstract class PoolList<T> : PoolList where T : PooledObject
    {
        #region Property Fields

        private Dictionary<Type, ObjectPool<T>> _poolMap;
        private T[] _allPrefabs;

        #endregion Property Fields

        /// <summary>
        /// An array representation of each pool represented by this PoolList.
        /// </summary>
        protected ObjectPool<T>[] Pools { get; private set; }

        public ObjectPool<T>[] GetAllPools()
        {
            ObjectPool<T>[] allPools = new ObjectPool<T>[Pools.Length];
            Pools.CopyTo(allPools, 0);
            return allPools;
        }

        /// <summary>
        /// An array of all private prefabs represented by this PoolList.
        /// </summary>
        protected T[] AllPrefabs
        {
            get => _allPrefabs;
            private set
            {
                _allPrefabs = value;
                PrefabMap = _allPrefabs.ToDictionary(x => x.GetType(), x => x);
                PoolMap = _allPrefabs.ToDictionary(x => x.GetType(), x => new ObjectPool<T>(x));
            }
        }

        public T[] GetAllPrefabs()
        {
            T[] allPrefabs = new T[AllPrefabs.Length];
            AllPrefabs.CopyTo(allPrefabs, 0);
            return allPrefabs;
        }

        /// <summary>
        /// A dictionary in which each prefab represented by this PoolList
        /// is indexed by the type of the prefab.
        /// </summary>
        protected Dictionary<Type, T> PrefabMap { get; private set; }

        /// <summary>
        /// Returns the default color of objects in this pool from a given color manager.
        /// </summary>
        /// <param name="colorManager">This game's color manager.</param>
        /// <returns>The default color of objects in this pool</returns>
        protected abstract Color GetDefaultColor(in ColorManager colorManager);

        protected abstract void OnInitSprites(in ColorManager colorManager);


        /// <summary>
        /// Initializes the color of each object prefab withing this pool.
        /// </summary>
        /// <param name="colorManager">This game's color manager.</param>
        public void InitSprites(in ColorManager colorManager)
        {
            Color defaultColor = GetDefaultColor(in colorManager);

            foreach (var prefab in AllPrefabs)
            {
                prefab.IsOriginalPrefab = true;
                prefab.Init();
                prefab.SpriteColor = defaultColor;
            }

            OnInitSprites(in colorManager);
        }

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

        protected virtual void OnInit() { }
        public override void Init(in ColorManager colorManager)
        {
            LoadPoolMap();
            InitSprites(in colorManager);
            OnInit();
        }

        /// <summary>
        /// Uses reflection to initialize a new ObjectPool&lt;<typeparamref name="T"/>&gt;
        /// for each prefab contained in this PoolList.
        /// </summary>
        protected void LoadPoolMap()
        {
            try
            {
                AllPrefabs = ReflectionUtil.GetPrivatePoolablePrefabFields(this)
                    .Select(x => (T)x).ToArray();
            }
            catch (Exception ex)
            {
                var prefabs = ReflectionUtil.GetPrivatePoolablePrefabFields(this);
                throw ex;
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
            }
            catch (Exception ex)
            {
                var type = typeof(TGet);
                throw ex;
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
        public TGet Get<TGet>(Vector3 position) where TGet : T
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
        public TGet Get<TGet>(Vector3 position, Vector2 velocity) where TGet : T
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
            var ret = (TGet)pool.ObjectPrefab;
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
        public IEnumerable<T> GetAllActiveObjects()
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

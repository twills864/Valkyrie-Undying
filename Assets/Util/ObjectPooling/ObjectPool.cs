using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Util
{
    /// <summary>
    /// A subclass of List&lt;<typeparamref name="T"/>&gt; that represents
    /// a Unity implementation of an object pool.
    /// </summary>
    /// <typeparam name="T">The type of object represented in this Object Pool.</typeparam>
    public class ObjectPool<T> : List<T> where T : PooledObject
    {
        /// <summary>
        /// The prefab of the object contained in this pool.
        /// </summary>
        public T ObjectPrefab { get; }

        private int _nextSpawnId = 0;
        /// <summary>
        /// The next unique ID to assign to spawned objects.
        /// </summary>
        public int NextSpawnId { get => _nextSpawnId++; }

        /// <summary>
        /// Returns all objects in the pool that are active and enabled.
        /// </summary>
        public IEnumerable<T> ActiveObjects => this.Where(x => x.isActiveAndEnabled);

        /// <summary>
        /// Whether or not this Object Pool has been activated at any point.
        /// </summary>
        public bool HasHadActivity => this.Any();

        public ObjectPool(T objectPrefab)
        {
            ObjectPrefab = objectPrefab;
        }

        /// <summary>
        /// Gets a fresh instance of the represented object, either by
        /// activating an inactive object, or creating a new one
        /// if there are no inactive objects in the pool.
        /// </summary>
        /// <returns>A fresh instance of <typeparamref name="T"/>.</returns>
        public T Get()
        {
            T firstInactive = this.Where(x => !x.isActiveAndEnabled).FirstOrDefault();

            T ret = firstInactive ?? CreateNewPooledObject();
            ret.ActivateSelf();
            ret.SpawnId = NextSpawnId;
            ret.name = ret.SpawnName;

            return ret;
        }

        /// <summary>
        /// Instantiates a new instance of the represented prefab.
        /// </summary>
        /// <returns>The newly-instantiated prefab.</returns>
        private T CreateNewPooledObject()
        {
            var ret = UnityEngine.Object.Instantiate(ObjectPrefab);
            ret.Init();
            ret.InitialName = ret.name;
            Add(ret);
            return ret;
        }

        /// <summary>
        /// Calls RunFrame() on each active object in the pool
        /// using the specified <paramref name="deltaTime"/>.
        /// </summary>
        /// <param name="deltaTime">The represented amount of time that has passed
        /// since the last frame.</param>
        public void RunFrames(float deltaTime)
        {
            foreach(var element in ActiveObjects)
                element.RunFrame(deltaTime);
        }

        /// <summary>
        /// Recolors each object in the pool to a specified color.
        /// </summary>
        /// <param name="color">The color to give to each object.</param>
        public void RecolorObjects(Color color)
        {
            foreach(var obj in this)
                obj.GetComponent<SpriteRenderer>().color = color;
        }


        /// <summary>
        /// Counts the number of active objects, inactive objects,
        /// and total objects in this pool.
        /// </summary>
        /// <param name="active">The number of active objects in this pool.</param>
        /// <param name="inactive">The number of inactive objects in this pool.</param>
        /// <param name="total">The total number of objects in this pool.</param>
        public void CountObjects(out int active, out int inactive, out int total)
        {
            total = Count;

            active = 0;
            inactive = 0;

            for(int i = 0; i < Count; i++)
            {
                if (this[i].isActiveAndEnabled)
                    active++;
                else
                    inactive++;
            }
        }

        /// <summary>
        /// A debug representation of this object pool.
        /// </summary>
        public string DebugInfo
        {
            get
            {
                CountObjects(out int active, out int inactive, out int total);
                return $"[{active}/{total}]";
            }
        }

        public override string ToString()
        {
            var type = ObjectPrefab.GetType();
            var ret = $"{ObjectPrefab.GetType().Name}{DebugInfo} ({ObjectPrefab})";
            return ret;
        }
    }
}

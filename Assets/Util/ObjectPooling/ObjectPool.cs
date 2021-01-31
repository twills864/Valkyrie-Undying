using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util
{
    // This class was created for ease and speed of development.
    // It should be replaced with a hard-coded version upon release.
    public class ObjectPool<T> where T : PooledObject
    {
        private Stack<T> Pool { get; }
        private HighestValueTracker<int> Size { get; }

        private T Prefab { get; }

        public bool HasHadActivity => Size.HighestValue > 0;

        public ObjectPool(T prefab)
        {
            Pool = new Stack<T>();
            Size = new HighestValueTracker<int>();
            Prefab = prefab;
        }

        public void Add(T item)
        {
            Pool.Push(item);
            Size.Value = Pool.Count;
        }

        public TReturn Get<TReturn>() where TReturn : T
        {
            T ret;

            if (Pool.Any())
                ret = Pool.Pop();
            else
                ret = UnityEngine.Object.Instantiate(Prefab);

            return (TReturn) ret;
        }

        public override string ToString()
        {
            var type = Prefab.GetType();
            var ret = $"{Prefab.GetType().Name}{DebugInfo} ({Prefab})";
            return ret;
        }

        public string DebugInfo
        {
            get
            {
                return $"[{Pool.Count}/{Size.HighestValue}]";
            }
        }
    }
}

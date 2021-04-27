using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.ObjectPooling
{
    /// <summary>
    /// Tracks a set of unique PooledObjectTrackers, and automatically removes elements
    /// as they become inactive.
    /// </summary>
    /// <typeparam name="T">The type of PooledObject to track.</typeparam>
    public class TrackedPooledObjectSet<T> : IEnumerable<T> where T : PooledObject
    {
        private List<PooledObjectTracker<T>> List { get; set; }

        public TrackedPooledObjectSet()
        {
            List = new List<PooledObjectTracker<T>>();
        }

        public void Add(T item)
        {
            if (!this.Contains(item))
                List.Add(new PooledObjectTracker<T>(item));
        }

        // The number of elements in this list is not expected to be large enough
        // to justify using a HashSet to track existing items.
        public bool Contains(T item)
        {
            bool alreadyInList = List.Where(x => x.IsTarget(item)).Any();
            return alreadyInList;
        }

        public void Clear() => List.Clear();

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = List.Count - 1; i >= 0; i--)
            {
                PooledObjectTracker<T> ret = List[i];

                if (ret.IsActive)
                    yield return ret.Target;
                else
                    List.RemoveAt(i);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return List.Select(x => x.Target).GetEnumerator();
        }
    }
}

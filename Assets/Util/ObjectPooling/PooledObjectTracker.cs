using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util.ObjectPooling
{
    public class PooledObjectTracker<T> where T : PooledObject
    {
        private T Target { get; set; }
        private int TargetSpawnId { get; set; }

        public PooledObjectTracker(T target)
        {
            Target = target;
            TargetSpawnId = target.SpawnId;
        }

        public bool IsTarget(T toCompare)
        {
            bool ret = toCompare == Target
                && toCompare.SpawnId == TargetSpawnId;
            return ret;
        }
    }
}

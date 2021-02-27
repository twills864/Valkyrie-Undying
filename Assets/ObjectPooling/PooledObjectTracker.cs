using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.ObjectPooling
{
    public class PooledObjectTracker
    {
        private PooledObject Target { get; set; }
        private int TargetSpawnId { get; set; }

        public PooledObjectTracker(PooledObject target)
        {
            Target = target;
            TargetSpawnId = target.SpawnId;
        }

        public bool IsTarget(PooledObject toCompare)
        {
            bool ret = toCompare == Target
                && toCompare.SpawnId == TargetSpawnId;
            return ret;
        }

        public static implicit operator PooledObjectTracker(PooledObject target)
        {
            return new PooledObjectTracker(target);
        }
    }
}

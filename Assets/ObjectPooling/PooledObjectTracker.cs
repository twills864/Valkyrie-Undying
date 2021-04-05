using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.ObjectPooling
{
    public class PooledObjectTracker
    {
        private PooledObject _target;
        public PooledObject Target
        {
            get => _target;
            set
            {
                _target = value;
                TargetSpawnId = _target.SpawnId;
            }
        }

        private int TargetSpawnId { get; set; }

        public PooledObjectTracker()
        {
            _target = null;
            TargetSpawnId = -1;
        }

        public PooledObjectTracker(PooledObject target)
        {
            Target = target;
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

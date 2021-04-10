using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.ObjectPooling
{
    [DebuggerDisplay("({TargetSpawnId}) {Target}")]
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

        public bool IsActive => Target.isActiveAndEnabled
            && Target.SpawnId == TargetSpawnId;

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

        public void CloneFrom(PooledObjectTracker other)
        {
            _target = other._target;
            TargetSpawnId = other.TargetSpawnId;
        }

        public static implicit operator PooledObjectTracker(PooledObject target)
        {
            return new PooledObjectTracker(target);
        }
    }
}

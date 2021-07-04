using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.ObjectPooling
{
    /// <summary>
    /// Tracks a given PooledObject, and provides methods to determine
    /// whether or not a given PooledObject is both the same one assigned
    /// to the tracker, and that it hasn't been deactivated and reactivated
    /// since the original was assigned.
    /// </summary>
    /// <typeparam name="T">The type of PooledObject to track.</typeparam>
    /// <inheritdoc/>
    [DebuggerDisplay("({TargetSpawnId}) {Target}")]
    public class PooledObjectTracker<T> where T : PooledObject
    {
        private T _target;
        public T Target
        {
            get => _target;
            set
            {
                _target = value;
                TargetSpawnId = _target?.SpawnId ?? -1;
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

        public PooledObjectTracker(T target)
        {
            Target = target;
        }

        public bool IsTarget(PooledObject toCompare)
        {
            bool ret = toCompare == Target
                && toCompare.SpawnId == TargetSpawnId;
            return ret;
        }

        public void CloneFrom(PooledObjectTracker<T> other)
        {
            _target = other._target;
            TargetSpawnId = other.TargetSpawnId;
        }

        public static implicit operator PooledObjectTracker<T>(T target)
        {
            return new PooledObjectTracker<T>(target);
        }
    }
}

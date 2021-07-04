using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameTasks
{
    /// <summary>
    /// Changes the position of the target ValkyrieSprite
    /// by a specified amount over a specified period of time.
    /// </summary>
    /// <inheritdoc/>
    public class MoveBy : FiniteMovementGameTask
    {
        private Vector3 _distance;

        public Vector3 Distance
        {
            get => _distance;
            set
            {
                _distance = value;
                DistanceChanged();
            }
        }

        public float DistanceX
        {
            get => _distance.x;
            set
            {
                _distance.x = value;
                DistanceChanged();
            }
        }

        public float DistanceY
        {
            get => _distance.y;
            set
            {
                _distance.y = value;
                DistanceChanged();
            }
        }

        private void DistanceChanged()
        {
            Velocity = Distance / Duration;
        }

        public MoveBy(ValkyrieSprite target, Vector3 distance, float duration) : base(target, duration)
        {
            Distance = distance;
        }

        public static MoveBy Default(ValkyrieSprite target, float duration)
        {
            var ret = new MoveBy(target, Vector3.zero, duration);
            ret.FinishSelf();
            return ret;
        }
    }
}

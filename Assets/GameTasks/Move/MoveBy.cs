using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameTasks
{
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
    }
}

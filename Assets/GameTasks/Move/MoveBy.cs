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
                Velocity = Distance / Duration;
            }
        }

        public MoveBy(ValkyrieSprite target, Vector3 distance, float duration) : base(target, duration)
        {
            Distance = distance;
        }
    }
}

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
        private Vector2 _distance;

        public Vector2 Distance
        {
            get => _distance;
            set
            {
                _distance = value;
                Velocity = Distance / Duration;
            }
        }

        public MoveBy(ValkyrieSprite target, Vector2 distance, float duration) : base(target, duration)
        {
            Distance = distance;
        }
    }
}

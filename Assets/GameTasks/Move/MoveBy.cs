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
        private Vector2 Distance { get; set; }

        public MoveBy(GameTaskRunner target, Vector2 distance, float duration) : base(target, duration)
        {
            Distance = distance;

            Velocity = Distance / Duration;
        }
    }
}

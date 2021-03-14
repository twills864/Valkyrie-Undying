using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameTasks
{
    public class MoveSpeedForDuration : FiniteMovementGameTask
    {
        public MoveSpeedForDuration(ValkyrieSprite target, Vector2 velocity, float duration) : base(target, duration)
        {
            Velocity = velocity;
        }
    }
}

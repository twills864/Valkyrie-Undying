using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Constants;
using UnityEngine;

namespace Assets.GameTasks
{
    /// <summary>
    /// Causes the target ValkyrieSprite to move at a specified velocity
    /// over a specified period of time.
    /// </summary>
    /// <inheritdoc/>
    [Obsolete(ObsoleteConstants.CurrentlyUnused)]
    public class MoveSpeedForDuration : FiniteMovementGameTask
    {
        public MoveSpeedForDuration(ValkyrieSprite target, Vector2 velocity, float duration) : base(target, duration)
        {
            Velocity = velocity;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameTasks
{
    /// <inheritdoc/>
    public abstract class FiniteVelocityGameTask : FiniteMovementGameTask
    {
        public FiniteVelocityGameTask(GameTaskRunner target, float duration) : base(target, duration) { }

        /// <summary>
        /// Functionality that will occur after this Task's Timer is updated,
        /// but before applying the velocity of this Task.
        /// </summary>
        /// <param name="deltaTime">The represented amount of time that has passed
        /// since the last frame.</param>
        protected virtual void OnFiniteVelocityTaskFrameRun(float deltaTime) { }
        protected sealed override void OnFiniteMovementTaskFrameRun(float deltaTime)
        {
            OnFiniteVelocityTaskFrameRun(deltaTime);
        }

        /// <summary>
        /// Alters the Velocity of the Target itself, instead of applying transformations independently.
        /// </summary>
        /// <param name="deltaTime">The represented amount of time that has passed
        /// since the last frame (unused).</param>
        protected sealed override void ApplyVelocity(float deltaTime)
        {
            Target.Velocity = Velocity;
        }
    }
}

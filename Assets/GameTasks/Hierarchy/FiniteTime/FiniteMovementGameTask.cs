using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameTasks
{
    /// <inheritdoc/>
    public abstract class FiniteMovementGameTask : FiniteTimeGameTask
    {
        /// <summary>
        /// The velocity that will be applied for movement-related calculations.
        /// </summary>
        protected Vector2 Velocity { get; set; }

        public FiniteMovementGameTask(GameTaskRunner target, float duration) : base(target, duration) { }

        /// <summary>
        /// Functionality that will occur after this Task's Timer is updated,
        /// but before applying the velocity of this Task.
        /// </summary>
        /// <param name="deltaTime">The represented amount of time that has passed
        /// since the last frame.</param>
        protected virtual void OnFiniteMovementTaskFrameRun(float deltaTime) { }
        protected sealed override void OnFiniteTaskFrameRun(float deltaTime)
        {
            OnFiniteMovementTaskFrameRun(deltaTime);
            ApplyVelocity(deltaTime);
        }

        /// <summary>
        /// Applies this Task's Velocity to the Target GameTaskRunner.
        /// By default, the position of the Target is translated by Velocity * deltaTime.
        /// </summary>
        /// <param name="deltaTime">The represented amount of time that has passed
        /// since the last frame.</param>
        protected virtual void ApplyVelocity(float deltaTime)
        {
            Target.transform.position += new Vector3(Velocity.x * deltaTime, Velocity.y * deltaTime, 0);
        }
    }
}

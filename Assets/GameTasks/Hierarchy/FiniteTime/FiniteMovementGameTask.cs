using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameTasks
{
    public abstract class FiniteMovementGameTask : FiniteTimeGameTask
    {
        protected Vector2 Velocity { get; set; }

        public FiniteMovementGameTask(GameTaskRunner target, float duration) : base(target, duration) { }

        protected virtual void OnFiniteMovementTaskFrameRun(float deltaTime) { }
        protected sealed override void OnFiniteTaskFrameRun(float deltaTime)
        {
            ApplyVelocity(deltaTime);
            OnFiniteMovementTaskFrameRun(deltaTime);
        }

        protected void ApplyVelocity(float deltaTime)
        {
            Target.transform.position += new Vector3(Velocity.x * deltaTime, Velocity.y * deltaTime, 0);
        }
    }
}

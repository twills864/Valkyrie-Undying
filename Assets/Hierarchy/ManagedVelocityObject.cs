using UnityEngine;

namespace Assets
{
    /// <summary>
    /// Represents an object with a velocity that will be manually managed.
    /// </summary>
    /// <inheritdoc/>
    public abstract class ManagedVelocityObject : FrameRunner
    {
        public virtual Vector2 Velocity { get; set; }
        public virtual float VelocityX
        {
            get => Velocity.x;
            set => Velocity = new Vector2(value, Velocity.y);
        }
        public virtual float VelocityY
        {
            get => Velocity.y;
            set => Velocity = new Vector2(Velocity.x, value);
        }

        public void ApplyVelocity(Vector2 velocity, float deltaTime)
        {
            Vector3 translation = new Vector3(velocity.x * deltaTime, velocity.y * deltaTime, 0);
            transform.Translate(translation, Space.World);

            //transform.position += new Vector3(velocity.x * deltaTime, velocity.y * deltaTime, 0);
        }

        protected virtual void OnManagedVelocityObjectFrameRun(float deltaTime) { }
        public sealed override void RunFrame(float deltaTime)
        {
            ApplyVelocity(Velocity, deltaTime);
            //transform.position += new Vector3(Velocity.x * deltaTime, Velocity.y * deltaTime, 0);
            //transform.Translate(deltaTime * Velocity, Space.World);
            OnManagedVelocityObjectFrameRun(deltaTime);
        }
    }
}

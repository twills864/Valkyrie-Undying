using UnityEngine;

namespace Assets.Enemies
{
    /// <inheritdoc/>
    public abstract class PermanentVelocityEnemy : ConstantVelocityEnemy
    {
        [SerializeField]
        protected float PermanentVelocityX;
        [SerializeField]
        protected float PermanentVelocityY;

        public sealed override Vector2 Velocity => new Vector2(PermanentVelocityX, PermanentVelocityY);
    }
}
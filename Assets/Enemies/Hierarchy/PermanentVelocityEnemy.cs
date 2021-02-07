using UnityEngine;

namespace Assets.Enemies
{
    /// <inheritdoc/>
    public abstract class PermanentVelocityEnemy : Enemy
    {
        [SerializeField]
        protected float PermanentVelocityX;
        [SerializeField]
        protected float PermanentVelocityY;

        private Vector2 _velocity;
        public sealed override Vector2 Velocity => _velocity;

        protected virtual void OnPermanentVelocityEnemyInit() { }
        protected override void OnEnemyInit()
        {
            _velocity = new Vector2(PermanentVelocityX, PermanentVelocityY);
            OnPermanentVelocityEnemyInit();
        }
    }
}
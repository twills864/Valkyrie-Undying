using UnityEngine;

namespace Assets.Enemies
{
    /// <inheritdoc/>
    public abstract class PermanentVelocityEnemy : FireStrategyEnemy
    {
        [SerializeField]
        protected float PermanentVelocityX;
        [SerializeField]
        protected float PermanentVelocityY;

        private Vector2 _velocity;
        public sealed override Vector2 Velocity => !IsVoidPaused ? _velocity : Vector2.zero;

        protected virtual void OnPermanentVelocityEnemyInit() { }
        protected sealed override void OnEnemyInit()
        {
            _velocity = new Vector2(PermanentVelocityX, PermanentVelocityY);
            OnPermanentVelocityEnemyInit();
        }
    }
}
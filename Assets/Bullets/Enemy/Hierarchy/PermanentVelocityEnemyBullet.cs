using UnityEngine;

namespace Assets.Bullets.EnemyBullets
{
    /// <inheritdoc/>
    public abstract class PermanentVelocityEnemyBullet : EnemyBullet
    {
        [SerializeField]
        protected float PermanentVelocityX;
        [SerializeField]
        protected float PermanentVelocityY;

        private Vector2 _velocity;
        public sealed override Vector2 Velocity => _velocity;

        protected virtual void OnPermanentVelocityEnemyBulletInit() { }
        protected sealed override void OnEnemyBulletInit()
        {
            _velocity = new Vector2(PermanentVelocityX, PermanentVelocityY);
            OnPermanentVelocityEnemyBulletInit();
        }
    }
}
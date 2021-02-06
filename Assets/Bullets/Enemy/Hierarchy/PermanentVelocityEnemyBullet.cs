using UnityEngine;

namespace Assets.Bullets.EnemyBullets
{
    /// <inheritdoc/>
    public abstract class PermanentVelocityEnemyBullet : ConstantVelocityEnemyBullet
    {
        [SerializeField]
        protected float PermanentVelocityX;
        [SerializeField]
        protected float PermanentVelocityY;

        public sealed override Vector2 Velocity => new Vector2(PermanentVelocityX, PermanentVelocityY);

        protected void OnPermanentVelocityBulletInit() { }
        protected override sealed void OnConstantVelocityEnemyBulletInit()
        {
            OnPermanentVelocityBulletInit();
        }

        protected virtual void RunPermanentVelocityEnemyBulletFrame(float deltaTime) { }
        protected sealed override void RunConstantVelocityEnemyBulletFrame(float deltaTime)
        {
            RunPermanentVelocityEnemyBulletFrame(deltaTime);
        }
    }
}
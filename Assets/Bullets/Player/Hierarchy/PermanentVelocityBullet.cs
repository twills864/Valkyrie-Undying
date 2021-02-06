using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public abstract class PermanentVelocityBullet : ConstantVelocityBullet
    {
        [SerializeField]
        protected float PermanentVelocityX;
        [SerializeField]
        protected float PermanentVelocityY;

        public sealed override Vector2 Velocity => new Vector2(PermanentVelocityX, PermanentVelocityY);

        protected virtual void OnPermanentVelocityBulletInit() { }
        protected override sealed void OnConstantVelocityBulletInit()
        {
            OnPermanentVelocityBulletInit();
        }

        protected virtual void RunPermanentVelocityBulletFrame(float deltaTime) { }
        protected sealed override void RunConstantVelocityBulletFrame(float deltaTime)
        {
            RunPermanentVelocityBulletFrame(deltaTime);
        }
    }
}
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public abstract class PermanentVelocityPlayerBullet : PlayerBullet
    {
        [SerializeField]
        protected float PermanentVelocityX;
        [SerializeField]
        protected float PermanentVelocityY;

        private Vector2 _velocity;
        public sealed override Vector2 Velocity => _velocity;

        protected virtual void RunPermanentVelocityBulletFrame(float deltaTime) { }
        protected override void OnPlayerBulletFrameRun(float deltaTime)
        {
            RunPermanentVelocityBulletFrame(deltaTime);
        }

        protected virtual void OnPermanentVelocityBulletInit() { }
        protected sealed override void OnPlayerBulletInit()
        {
            _velocity = new Vector2(PermanentVelocityX, PermanentVelocityY);
            OnPermanentVelocityBulletInit();
        }
    }
}
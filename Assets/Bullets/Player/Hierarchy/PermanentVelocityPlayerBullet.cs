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

        protected virtual void RunPermanentVelocityBulletFrame(float deltaTime, float realDeltaTime) { }
        protected override void OnPlayerBulletFrameRun(float deltaTime, float realDeltaTime)
        {
            RunPermanentVelocityBulletFrame(deltaTime, realDeltaTime);
        }

        protected virtual void OnPermanentVelocityBulletInit() { }
        protected sealed override void OnPlayerBulletInit()
        {
            _velocity = new Vector2(PermanentVelocityX, PermanentVelocityY);
            OnPermanentVelocityBulletInit();
        }
    }
}
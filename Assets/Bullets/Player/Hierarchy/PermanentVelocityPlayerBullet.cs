using Assets.Constants;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public abstract class PermanentVelocityPlayerBullet : PlayerBullet
    {
        #region Prefabs

        [SerializeField]
        private float _PermanentVelocityX = GameConstants.PrefabNumber;
        [SerializeField]
        private float _PermanentVelocityY = GameConstants.PrefabNumber;

        #endregion Prefabs

        #region Prefab Properties

        protected float PermanentVelocityX => _PermanentVelocityX;
        protected float PermanentVelocityY => _PermanentVelocityY;

        #endregion Prefab Properties

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
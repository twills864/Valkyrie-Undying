using UnityEngine;

namespace Assets.Bullets.EnemyBullets
{
    /// <summary>
    /// Represents an enemy bullet whose velocity will be assigned
    /// as soon as the instance is created, and will never change.
    /// </summary>
    /// <inheritdoc/>
    public abstract class PermanentVelocityEnemyBullet : EnemyBullet
    {
        #region Prefabs

        [SerializeField]
        protected float _PermanentVelocityX;
        [SerializeField]
        protected float _PermanentVelocityY;

        #endregion Prefabs


        #region Prefab Properties

        public float PermanentVelocityX => _PermanentVelocityX;
        public float PermanentVelocityY => _PermanentVelocityY;

        #endregion Prefab Properties


        private Vector2 _velocity;
        public override Vector2 Velocity { get => _velocity; set { } }

        protected virtual void OnPermanentVelocityEnemyBulletInit() { }
        protected sealed override void OnEnemyBulletInit()
        {
            _velocity = new Vector2(PermanentVelocityX, PermanentVelocityY);
            OnPermanentVelocityEnemyBulletInit();
        }
    }
}
using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Enemies;
using Assets.ObjectPooling;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public class ShrapnelBullet : PlayerBullet
    {
        protected override bool ShouldMarkSelfCollision => false;
        public override AudioClip FireSound => SoundBank.Silence;

        #region Prefabs

        [SerializeField]
        private float _Speed = GameConstants.PrefabNumber;

        [SerializeField]
        private float _RotationSpeed = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        public float Speed => _Speed;
        public float RotationSpeed => _RotationSpeed;

        #endregion Prefab Properties

        public bool IsBurning => FireDamage != 0;
        public int FireDamage { get; set; }
        public int FireDamageMax { get; set; }

        public override Vector2 Velocity
        {
            get => base.Velocity;
            set
            {
                base.Velocity = value;
                RotationDirection = Velocity.x > 0 ? -1f : 1f;
            }
        }
        private float RotationDirection { get; set; }

        public PooledObjectTracker<Enemy> Parent { get; private set; }

        protected override void OnPlayerBulletInit()
        {
            Parent = new PooledObjectTracker<Enemy>();
        }

        protected override void OnActivate()
        {
            FireDamage = 0;
        }

        protected override void OnPlayerBulletFrameRun(float deltaTime, float realDeltaTime)
        {
            RotationDegrees += RotationSpeed * deltaTime * RotationDirection;
        }

        public override bool CollidesWithEnemy(Enemy enemy)
        {
            bool ret = !Parent.IsTarget(enemy);
            return ret;
        }

        public override void OnCollideWithEnemy(Enemy enemy, Vector3 hitPosition)
        {
            // Burn enemy if applicable before deactivating.
            if(IsBurning && !enemy.IsBurning)
                enemy.Ignite(FireDamage, FireDamage, FireDamageMax);

            base.OnCollideWithEnemy(enemy, hitPosition);
        }
    }
}
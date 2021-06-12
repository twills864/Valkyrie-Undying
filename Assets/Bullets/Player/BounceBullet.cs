using Assets.Constants;
using Assets.Enemies;
using Assets.ScreenEdgeColliders;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public class BounceBullet : BouncingBullet
    {
        public override AudioClip FireSound => SoundBank.LaserPuff;
        public override float BulletTrailWidth => base.BulletTrailWidth * 0.5f;

        #region Prefabs

        [SerializeField]
        private float _RotationSpeed = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        private float RotationSpeed => _RotationSpeed;

        #endregion Prefab Properties


        protected override void OnPlayerBulletFrameRun(float deltaTime, float realDeltaTime)
        {
            transform.Rotate(0, 0, deltaTime * RotationSpeed);
        }

        #region Enemy Bounce

        protected override void OnBounce(Enemy enemy)
        {
            if (Director.TryGetRandomEnemyExcluding(enemy, out Enemy newTarget))
                SetTarget(newTarget);
            else
                BounceToRandomDirection();
        }

        private void SetTarget(Enemy newTarget)
        {
            var velocity = MathUtil.VelocityVector(transform.position, newTarget.transform.position, Speed)
                        + newTarget.Velocity;
            Velocity = velocity;
        }

        private void BounceToRandomDirection()
        {
            float x = RandomUtil.RandomlyNegative(1);
            float y = RandomUtil.RandomlyNegative(1);

            var velocity = MathUtil.VelocityVector(x, y, Speed);
            Velocity = velocity;
        }

        #endregion Enemy Bounce

        protected override void OnPlayerBulletTriggerEnter2D(Collider2D collision)
        {
            // Max level bullets can bounce off the screen edge if they've already hit an enemy.
            const int MaxPossibleBounces = 2 + GameConstants.MaxWeaponLevel;
            if (CollisionUtil.IsScreenEdge(collision))
            {
                // Edge case preventing us from calling DeactivateSelf() here if BouncesLeft > 0 fails:
                // Imagine the bullet touches the screen edge, doesn't have any bounces left, and gets deactivated.
                // The bullet could theoretically miss an enemy behind the screen edge that it should have hit
                // under normal deactivation rules.
                if (IsMaxLevel && BouncesLeft > 0 && BouncesLeft != MaxPossibleBounces)
                    BounceOffScreenEdge();
            }
        }

        #region Max Level Screen Collision Bounce

        private void BounceOffScreenEdge()
        {
            ApplyBounceDebuff();

            if (Director.TryGetRandomEnemy(out Enemy newTarget))
                SetTarget(newTarget);
            else
                SetTargetToCenter();
        }

        private void SetTargetToCenter()
        {
            Velocity = MathUtil.VelocityVector(transform.position, SpaceUtil.WorldMap.Center, Speed);
        }

        #endregion Max Level Screen Collision Bounce
    }
}
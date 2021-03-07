using Assets.Constants;
using Assets.Enemies;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public class BounceBullet : BouncingBullet
    {
        [SerializeField]
        private float RotationSpeed = GameConstants.PrefabNumber;

        protected override void OnPlayerBulletFrameRun(float deltaTime)
        {
            transform.Rotate(0, 0, deltaTime * RotationSpeed);
        }


        #region Enemy Bounce

        protected override void OnBounce(Enemy enemy)
        {
            if (GameManager.Instance.TryGetRandomEnemyExcluding(enemy, out Enemy newTarget))
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
            if (CollisionUtil.IsScreenEdge(collision))
            {
                // Edge case preventing us from calling DeactivateSelf() here if BouncesLeft > 0 fails:
                // Imagine the bullet touches the screen edge, doesn't have any bounces left, and gets deactivated.
                // The bullet could theoretically miss an enemy behind the screen edge that it should have hit
                // under normal deactivation rules.
                if (IsMaxLevel && BouncesLeft > 0)
                    BounceOffScreenEdge();
            }
        }

        #region Max Level Screen Collision Bounce

        private void BounceOffScreenEdge()
        {
            ApplyBounceDebuff();

            if (GameManager.Instance.TryGetRandomEnemy(out Enemy newTarget))
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
using System;
using System.Linq;
using Assets.Bullets.EnemyBullets;
using Assets.Constants;
using Assets.Enemies;
using Assets.ObjectPooling;
using Assets.Powerups.Balance;
using Assets.Util;
using UnityEngine;

namespace Assets.FireStrategies.EnemyFireStrategies
{
    /// <summary>
    /// Fires Wasp enemy bullets directly toward the player's position.
    ///
    /// Additionally, fires a spread of Wasp bullets behind the Wasp
    /// when it jumps to a new location.
    /// </summary>
    /// <inheritdoc/>
    public class WaspEnemyStrategy : EnemyFireStrategy<WaspEnemyBullet>
    {
        public WaspEnemyStrategy() : this(PoolManager.Instance.EnemyBulletPool.GetPrefab<WaspEnemyBullet>())
        {
        }
        public WaspEnemyStrategy(WaspEnemyBullet bulletPrefab) : base(bulletPrefab)
        {
        }

        public EnemyBullet[] GetBullets(Vector3 enemyFirePos, float angleDegrees)
        {
            var bullets = base.GetBullets(enemyFirePos);
            var bullet = (WaspEnemyBullet)bullets[0];

            bullet.Velocity = MathUtil.Vector2AtDegreeAngle(angleDegrees, bullet.Speed);
            bullet.OnSpawn();

            return bullets;
        }

        public EnemyBullet[] FireBehind(Vector3 behindFirePos, float angleDegrees, float spreadAngleOffset)
        {
            const int NumBullets = 3;

            var bullets = PoolManager.Instance.EnemyBulletPool.GetMany<WaspEnemyBullet>(NumBullets);

            const float AngleCircleQuarter = 90f;
            float angleNormal = angleDegrees - AngleCircleQuarter;
            Vector3 bulletOffset = MathUtil.Vector3AtDegreeAngle(angleNormal, bullets[0].SpriteMap.Width * 2f);

            const float AngleCircleHalf = 180f;
            angleDegrees = angleDegrees + AngleCircleHalf;

            float currentAngle = angleDegrees - ((NumBullets / 2) * spreadAngleOffset);
            Vector3 currentBulletOffset = bulletOffset * -(NumBullets / 2);
            foreach (var bullet in bullets)
            {
                bullet.transform.position = behindFirePos + currentBulletOffset;
                bullet.Velocity = MathUtil.Vector2AtDegreeAngle(currentAngle, bullet.Speed);
                bullet.OnSpawn();

                currentAngle += spreadAngleOffset;
                currentBulletOffset += bulletOffset;
            }


            return bullets;
        }
    }
}

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
    ///
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
    }
}

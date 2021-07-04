using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Bullets.EnemyBullets;
using Assets.Constants;
using Assets.Enemies;
using Assets.ObjectPooling;
using Assets.Util;
using UnityEngine;

namespace Assets.FireStrategies.EnemyFireStrategies
{
    /// <summary>
    /// Fires a Laser enemy's laser by calculating the position and rotation needed
    /// to give the bullet the appearance of being fired from the front of the Laser enemy.
    /// </summary>
    /// <inheritdoc/>
    public class LaserEnemyFireStrategy : EnemyFireStrategy<LaserEnemyBullet>
    {
        public LaserEnemyFireStrategy() : this(PoolManager.Instance.EnemyBulletPool.GetPrefab<LaserEnemyBullet>())
        {
        }
        public LaserEnemyFireStrategy(LaserEnemyBullet bulletPrefab) : base(bulletPrefab)
        {
        }

        public EnemyBullet[] GetBullets(LaserEnemy host)
        {
            float hostRotation = host.RotationDegrees;
            float hostWidthHalf = host.WidthHalf;

            var ret = base.GetBullets();
            var laserBullet = (LaserEnemyBullet)ret[0];

            laserBullet.SpawnPoint = host.transform.position;
            laserBullet.RotationDegrees = hostRotation;

            float laserWidthHalf = laserBullet.WidthHalf;
            float length = hostWidthHalf + laserWidthHalf;
            Vector3 positionOffset = MathUtil.Vector3AtDegreeAngle(host.RotationDegrees, length);
            laserBullet.transform.position = host.transform.position + positionOffset;

            return ret;
        }

        //public EnemyBullet[] GetBulletsAtAngle(Vector3 enemyFirePos, float degreesAngle)
        //{
        //    Vector2 velocityScale = MathUtil.Vector2AtDegreeAngle(degreesAngle);
        //    return GetBullets(enemyFirePos, velocityScale);
        //}
    }
}

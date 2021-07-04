using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Bullets.EnemyBullets;
using Assets.ObjectPooling;
using Assets.Util;
using UnityEngine;

namespace Assets.FireStrategies.EnemyFireStrategies
{
    /// <summary>
    /// Fires Cradle enemy bullets with a given velocity scale
    /// that matches the current rotation of the Cradle enemy.
    /// </summary>
    /// <inheritdoc/>
    public class CradleEnemyFireStrategy : EnemyFireStrategy<CradleEnemyBullet>
    {
        public CradleEnemyFireStrategy() : this(PoolManager.Instance.EnemyBulletPool.GetPrefab<CradleEnemyBullet>())
        {
        }
        public CradleEnemyFireStrategy(CradleEnemyBullet bulletPrefab) : base(bulletPrefab)
        {
        }

        public EnemyBullet[] GetBullets(Vector3 enemyFirePos, Vector2 velocityScale)
        {
            var ret = base.GetBullets(enemyFirePos);
            CradleEnemyBullet bullet = (CradleEnemyBullet) ret[0];
            bullet.Velocity = bullet.Speed * velocityScale;
            bullet.OnSpawn();
            return ret;
        }

        //public EnemyBullet[] GetBulletsAtAngle(Vector3 enemyFirePos, float degreesAngle)
        //{
        //    Vector2 velocityScale = MathUtil.Vector2AtDegreeAngle(degreesAngle);
        //    return GetBullets(enemyFirePos, velocityScale);
        //}
    }
}

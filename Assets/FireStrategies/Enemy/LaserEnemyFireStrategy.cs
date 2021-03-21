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
    public class LaserEnemyFireStrategy : EnemyFireStrategy<LaserEnemyBullet>
    {
        public const int LaserIndex = 0;
        public const int LaserSpawnerIndex = 1;

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

            var laserBullet = (LaserEnemyBullet) PoolManager.Instance.EnemyBulletPool.Get<LaserEnemyBullet>();
            laserBullet.RotationDegrees = hostRotation;

            float laserWidthHalf = laserBullet.WidthHalf;
            float length = hostWidthHalf + laserWidthHalf;
            Vector3 positionOffset = MathUtil.Vector3AtDegreeAngle(host.RotationDegrees, length);
            //laserBullet.transform.position = host.transform.position + positionOffset;

            var laserBulletSpawner = (LaserEnemyBulletSpawner)PoolManager.Instance.EnemyBulletPool.Get<LaserEnemyBulletSpawner>();
            laserBulletSpawner.transform.localScale = laserBullet.transform.localScale;
            laserBulletSpawner.RotationDegrees = hostRotation;
            laserBulletSpawner.transform.position = host.transform.position + positionOffset;
            laserBullet.transform.position = laserBulletSpawner.transform.position;

            StowUtil.StowX(laserBullet, StowUtil.LaserEnemyBulletStowX);

            var ret = new EnemyBullet[2];
            ret[LaserIndex] = laserBullet;
            ret[LaserSpawnerIndex] = laserBulletSpawner;

            return ret;
        }

        //public EnemyBullet[] GetBulletsAtAngle(Vector3 enemyFirePos, float degreesAngle)
        //{
        //    Vector2 velocityScale = MathUtil.Vector2AtDegreeAngle(degreesAngle);
        //    return GetBullets(enemyFirePos, velocityScale);
        //}

        protected override EnemyFireStrategy CloneSelf()
        {
            throw new NotImplementedException();
        }
    }
}

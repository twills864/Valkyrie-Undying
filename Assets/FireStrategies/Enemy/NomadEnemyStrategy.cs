using System;
using System.Linq;
using Assets.Bullets.EnemyBullets;
using Assets.Constants;
using Assets.Enemies;
using Assets.ObjectPooling;
using Assets.Powerups.Balance;
using Assets.UnityPrefabStructs;
using Assets.Util;
using UnityEngine;

namespace Assets.FireStrategies.EnemyFireStrategies
{
    /// <summary>
    /// Fires two Nomad enemy bullets straight down with a specified x-offset from one another.
    /// </summary>
    /// <inheritdoc/>
    public class NomadEnemyStrategy : VariantLoopingEnemyFireStrategy<NomadEnemyBullet>
    {
        public NomadEnemyStrategy(VariantFireSpeed variantFireSpeed)
        : base(variantFireSpeed)
        {
        }

        public override EnemyBullet[] GetBullets(Vector3 enemyFirePos)
        {
            var bullets = PoolManager.Instance.EnemyBulletPool.GetMany<NomadEnemyBullet>(2);

            NomadEnemyBullet left = bullets[0];
            InitBullet(left, enemyFirePos, -left.SpawnOffsetX);

            NomadEnemyBullet right = bullets[1];
            InitBullet(right, enemyFirePos, right.SpawnOffsetX);

            return bullets;
        }

        private void InitBullet(NomadEnemyBullet bullet, Vector3 enemyFirePos, float spawnOffsetX)
        {
            bullet.transform.position = enemyFirePos.AddX(spawnOffsetX);
            bullet.OnSpawn();
        }
    }
}

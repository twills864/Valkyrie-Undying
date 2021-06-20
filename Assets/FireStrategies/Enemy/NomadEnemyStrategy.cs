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
    ///
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
            var bullets = base.GetBullets(enemyFirePos);

            foreach (var bullet in bullets)
                bullet.OnSpawn();

            return bullets;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Bullets.EnemyBullets;
using Assets.ObjectPooling;
using Assets.UnityPrefabStructs;
using Assets.Util;
using UnityEngine;

namespace Assets.FireStrategies.EnemyFireStrategies
{
    /// <summary>
    /// Fires Basic enemy bullets in a straight line down.
    /// </summary>
    /// <inheritdoc/>
    public class BasicEnemyFireStrategy : VariantLoopingEnemyFireStrategy<BasicEnemyBullet>
    {
        public BasicEnemyFireStrategy(VariantFireSpeed variantFireSpeed)
        : base(variantFireSpeed)
        {
        }

        public override EnemyBullet[] GetBullets(Vector3 enemyFirePos)
        {
            var ret = base.GetBullets(enemyFirePos);
            ret[0].OnSpawn();
            return ret;
        }
    }
}

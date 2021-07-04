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
    /// Fires two Ring enemy bullets diagonally-down with a specified x-velocity offset.
    /// </summary>
    /// <inheritdoc/>
    public class RingEnemyFireStrategy : VariantLoopingEnemyFireStrategy<RingEnemyBullet>
    {
        public RingEnemyFireStrategy(VariantFireSpeed variantFireSpeed)
        : base(variantFireSpeed)
        {
        }

        public override EnemyBullet[] GetBullets(Vector3 enemyFirePos)
        {
            var ret = PoolManager.Instance.EnemyBulletPool.GetMany<RingEnemyBullet>(2);

            var left = ret[0];
            left.transform.position = enemyFirePos.AddX(-left.OffsetX);
            left.Velocity = left.LeftVelocity;
            left.OnSpawn();

            var right = ret[1];
            right.transform.position = enemyFirePos.AddX(right.OffsetX);
            right.Velocity = right.RightVelocity;
            right.OnSpawn();

            return ret;
        }
    }
}

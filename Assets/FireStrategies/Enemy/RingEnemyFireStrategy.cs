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
            left.transform.position = VectorUtil.AddX3(enemyFirePos, -left.OffsetX);
            left.Velocity = left.LeftVelocity;

            var right = ret[1];
            right.transform.position = VectorUtil.AddX3(enemyFirePos, right.OffsetX);
            right.Velocity = right.RightVelocity;

            return ret;
        }
    }
}

using Assets.EnemyBullets;
using Assets.Util;
using Assets.Util.ObjectPooling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.EnemyFireStrategies
{
    public abstract class EnemyFireStrategy : FireStrategies.FireStrategy
    {
        public LoopingFrameTimer FireTimer { get; set; }

        public EnemyFireStrategy()
        {
            FireTimer = DefaultFireTimer;
        }

        public void Reset()
        {
            FireTimer.Reset();
        }
    }

    public abstract class EnemyFireStrategy<TBullet> : EnemyFireStrategy where TBullet : EnemyBullet
    {
        public override Bullets.Bullet[] GetBullets(Vector2 enemyFirePos)
        {
            TBullet[] ret = new TBullet[] { PoolManager.Instance.EnemyBulletPool.Get<TBullet>() };
            ret[0].transform.position = enemyFirePos;
            return ret;
        }
    }
}

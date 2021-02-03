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
    public abstract class EnemyFireStrategy
    {
        public abstract LoopingFrameTimer FireTimer { get; }
        public abstract EnemyBullet[] GetBullets();
    }
    public abstract class EnemyFireStrategy<TBullet> : EnemyFireStrategy where TBullet : EnemyBullet
    {
        public override EnemyBullet[] GetBullets()
        {
            TBullet[] ret = new TBullet[] { PoolManager.Instance.EnemyBulletPool.Get<TBullet>() };
            return ret;
        }
    }
}

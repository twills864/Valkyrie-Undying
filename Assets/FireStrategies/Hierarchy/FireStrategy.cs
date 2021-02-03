using Assets.Bullets;
using Assets.Util;
using Assets.Util.ObjectPooling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.FireStrategies
{
    public abstract class FireStrategy
    {
        public abstract LoopingFrameTimer FireTimer { get; }
        public abstract Bullet[] GetBullets();
    }
    public abstract class FireStrategy<TBullet> : FireStrategy where TBullet : Bullet
    {
        public override Bullet[] GetBullets()
        {
            TBullet[] ret = new TBullet[] { PoolManager.Instance.BulletPool.Get<TBullet>() };
            return ret;
        }
    }
}

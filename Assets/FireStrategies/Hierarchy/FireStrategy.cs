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
        public abstract Bullet GetBullet();
    }
    public abstract class FireStrategy<TBullet> : FireStrategy where TBullet : Bullet
    {
        public override Bullet GetBullet()
        {
            TBullet ret = PoolManager.Instance.BulletPool.Get<TBullet>();
            return ret;
        }
    }
}

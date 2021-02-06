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
        public abstract LoopingFrameTimer FireTimer { get; protected set; }
        public void Reset() => FireTimer.ResetAndActivateSelf();

        //public abstract Bullet ObjectPrefab { get; }
    }
    public abstract class FireStrategy<TBullet> : FireStrategy where TBullet : Bullet
    {
        protected TBullet ObjectPrefab { get; }

        public FireStrategy() : this(null)
        {
        }
        public FireStrategy(TBullet bulletPrefab)
        {
            ObjectPrefab = bulletPrefab;
        }
    }
}

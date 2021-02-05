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
        public abstract LoopingFrameTimer DefaultFireTimer { get; }
        public abstract Bullet[] GetBullets(int weaponLevel, Vector2 playerFirePos);
    }
    public abstract class FireStrategy<TBullet> : FireStrategy where TBullet : Bullet
    {
        public override Bullet[] GetBullets(int weaponLevel, Vector2 playerFirePos)
        {
            TBullet[] ret = new TBullet[] { PoolManager.Instance.BulletPool.Get<TBullet>() };
            ret[0].transform.position = playerFirePos;
            return ret;
        }
    }
}

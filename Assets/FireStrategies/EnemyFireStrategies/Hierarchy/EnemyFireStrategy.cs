using Assets.Bullets;
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
    // TODO: Remove subclassing from FireStrategies.FireStrategy
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

        public abstract Bullets.Bullet[] GetBullets(Vector2 enemyFirePos);

        public override Bullet[] GetBullets(int weaponLevel, Vector2 playerFirePos)
        {
            return GetBullets(playerFirePos);
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

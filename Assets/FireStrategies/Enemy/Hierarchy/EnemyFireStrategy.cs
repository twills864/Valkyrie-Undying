using Assets.Bullets.EnemyBullets;
using Assets.FireStrategyManagers;
using Assets.ObjectPooling;
using Assets.Util;
using UnityEngine;

namespace Assets.FireStrategies.EnemyFireStrategies
{
    /// <summary>
    /// Contains the logic used to spawn enemy bullets in accordance with their functionality.
    /// </summary>
    /// <inheritdoc/>
    public abstract class EnemyFireStrategy : FireStrategy<EnemyBullet>
    {
        public EnemyFireStrategy(EnemyBullet bulletPrefab) : base(bulletPrefab)
        {

        }
        //protected EnemyFireStrategy(EnemyFireStrategy strategy) : base(strategy)
        //{
        //    FireSpeed = strategy.FireSpeed;
        //    Variance = strategy.Variance;
        //}

        public abstract EnemyBullet[] GetBullets();
        public abstract EnemyBullet[] GetBullets(Vector3 enemyFirePos);

        //protected abstract EnemyFireStrategy CloneSelf();
    }

    /// <inheritdoc/>
    public abstract class EnemyFireStrategy<TBullet> : EnemyFireStrategy where TBullet : EnemyBullet
    {
        //public EnemyFireStrategy() : this(null) { }
        public EnemyFireStrategy(TBullet bulletPrefab) : base(bulletPrefab)
        {

        }

        //protected EnemyFireStrategy(EnemyFireStrategy<TBullet> strategy) : base(strategy)
        //{
        //    FireTimer = ClonedFireTimer(strategy);
        //}

        public override EnemyBullet[] GetBullets()
        {
            TBullet[] ret = new TBullet[]
            {
                PoolManager.Instance.EnemyBulletPool.Get<TBullet>()
            };
            return ret;
        }

        public override EnemyBullet[] GetBullets(Vector3 enemyFirePos)
        {
            TBullet[] ret = new TBullet[]
            {
                PoolManager.Instance.EnemyBulletPool.Get<TBullet>(enemyFirePos)
            };
            return ret;
        }
    }
}

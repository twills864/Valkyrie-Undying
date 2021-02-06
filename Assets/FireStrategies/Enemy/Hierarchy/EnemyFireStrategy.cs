using Assets.Bullets.EnemyBullets;
using Assets.Util.ObjectPooling;
using UnityEngine;

namespace Assets.FireStrategies.EnemyFireStrategies
{
    /// <inheritdoc/>
    public abstract class EnemyFireStrategy : FireStrategy<EnemyBullet>
    {
        public EnemyFireStrategy(EnemyBullet bulletPrefab) : base(bulletPrefab)
        {
        }

        public abstract EnemyBullet[] GetBullets(Vector2 enemyFirePos);
    }

    /// <inheritdoc/>
    public abstract class EnemyFireStrategy<TBullet> : EnemyFireStrategy where TBullet : EnemyBullet
    {
        public EnemyFireStrategy() : this(null) { }
        public EnemyFireStrategy(TBullet bulletPrefab) : base(bulletPrefab)
        {

        }

        public override EnemyBullet[] GetBullets(Vector2 enemyFirePos)
        {
            TBullet[] ret = new TBullet[]
            {
                PoolManager.Instance.EnemyBulletPool.Get<TBullet>(enemyFirePos)
            };
            return ret;
        }
    }
}

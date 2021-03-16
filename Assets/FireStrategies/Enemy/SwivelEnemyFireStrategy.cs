using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Bullets.EnemyBullets;
using Assets.ObjectPooling;
using Assets.Util;

namespace Assets.FireStrategies.EnemyFireStrategies
{
    public class SwivelEnemyFireStrategy : EnemyFireStrategy<SwivelEnemyBullet>
    {
        public SwivelEnemyFireStrategy() : this(PoolManager.Instance.EnemyBulletPool.GetPrefab<SwivelEnemyBullet>())
        {
        }
        public SwivelEnemyFireStrategy(SwivelEnemyBullet bulletPrefab) : base(bulletPrefab)
        {
        }

        protected override EnemyFireStrategy CloneSelf()
        {
            throw new NotImplementedException();
        }
    }
}

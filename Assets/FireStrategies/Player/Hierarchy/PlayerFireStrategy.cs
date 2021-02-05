using Assets.Bullets;
using Assets.Bullets.PlayerBullets;
using Assets.Util.ObjectPooling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.FireStrategies.PlayerFireStrategies
{
    public abstract class PlayerFireStrategy : FireStrategy<PlayerBullet>
    {
        public PlayerFireStrategy(PlayerBullet bulletPrefab) : base(bulletPrefab)
        {
        }

        public abstract PlayerBullet[] GetBullets(int weaponLevel, Vector2 playerFirePos);
    }
    public abstract class PlayerFireStrategy<TBullet> : PlayerFireStrategy where TBullet : PlayerBullet
    {
        public PlayerFireStrategy() : this(null) { }
        public PlayerFireStrategy(TBullet bulletPrefab) : base(bulletPrefab)
        {
        }

        public override PlayerBullet[] GetBullets(int weaponLevel, Vector2 playerFirePos)
        {
            TBullet[] ret = new TBullet[]
            {
                PoolManager.Instance.BulletPool.Get<TBullet>(playerFirePos)
            };

            return ret;
        }
    }
}

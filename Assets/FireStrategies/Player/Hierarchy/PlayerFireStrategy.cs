using System;
using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Util.ObjectPooling;
using UnityEngine;

namespace Assets.FireStrategies.PlayerFireStrategies
{
    /// <inheritdoc/>
    public abstract class PlayerFireStrategy : FireStrategy<PlayerBullet>
    {
        public PlayerFireStrategy(PlayerBullet bulletPrefab) : base(bulletPrefab)
        {
        }

        public abstract PlayerBullet[] GetBullets(int weaponLevel, Vector2 playerFirePos);

        /// <summary>
        /// Gives an extra level to a given weapon level if it's equal to the
        /// maximum obtainable weapon level in the game.
        /// Generally, weapons gain an additional boost for achieving the maximum level.
        /// </summary>
        /// <param name="weaponLevel">The current weapon level.</param>
        /// <returns>The current weapon level plus one
        /// if it's the current level is the maximum obtainable weapon level.
        /// Otherwise, returns the current weapon level.</returns>
        protected int PlusOneIfMaxLevel(int weaponLevel)
        {
            int ret = weaponLevel != GameConstants.MaxWeaponLevel ? weaponLevel : weaponLevel + 1;
            return ret;
        }
    }

    /// <inheritdoc/>
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

        protected virtual PlayerBullet[] GetBullets(int weaponLevel, Vector2 playerFirePos, Vector2 velocity)
        {
            TBullet bullet = PoolManager.Instance.BulletPool.Get<TBullet>(playerFirePos);
            bullet.Velocity = velocity;
            TBullet[] ret = new TBullet[]
            {
                bullet
            };

            return ret;
        }

        protected virtual PlayerBullet[] GetBullets(int weaponLevel, Vector2 playerFirePos, Vector2 velocity, Action<TBullet> action)
        {
            TBullet bullet = PoolManager.Instance.BulletPool.Get<TBullet>(playerFirePos);
            bullet.Velocity = velocity;
            action(bullet);
            TBullet[] ret = new TBullet[]
            {
                bullet
            };

            return ret;
        }
    }
}

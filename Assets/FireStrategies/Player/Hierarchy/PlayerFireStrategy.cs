using System;
using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.FireStrategyManagers;
using Assets.ObjectPooling;
using Assets.Util;
using UnityEngine;

namespace Assets.FireStrategies.PlayerFireStrategies
{
    /// <inheritdoc/>
    public abstract class PlayerFireStrategy : FireStrategy<PlayerBullet>
    {
        protected abstract float GetFireSpeedRatio(in PlayerFireStrategyManager.PlayerRatio ratios);

        protected float InitialFireSpeed(in PlayerFireStrategyManager manager)
            => manager.BaseFireSpeed * GetFireSpeedRatio(in manager.PlayerRatios);

        public PlayerFireStrategy(PlayerBullet bulletPrefab, in PlayerFireStrategyManager manager) : base(bulletPrefab)
        {
            FireTimer = InitialFireTimer(in manager);
        }

        protected LoopingFrameTimer InitialFireTimer(in PlayerFireStrategyManager manager)
        {
            float speed = InitialFireSpeed(in manager);
            return new LoopingFrameTimer(speed);
        }

        public abstract PlayerBullet[] GetBullets(int weaponLevel, Vector2 playerFirePos);
        public abstract PlayerBullet[] GetBullets(int weaponLevel, Vector2 playerFirePos, Vector2 velocity);

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
        //public PlayerFireStrategy() : this(null) { }
        public PlayerFireStrategy(TBullet bulletPrefab, in PlayerFireStrategyManager manager) : base(bulletPrefab, manager)
        {
        }

        /// <summary>
        /// Gets a single bullet from the associated Bullet Pool and wraps it into an array.
        /// The default behavior for GetBullets().
        /// </summary>
        /// <param name="weaponLevel">The current weapon level.</param>
        /// <param name="playerFirePos">The current position from which to fire the bullet.</param>
        /// <returns>A single bullet wrapped in an array.</returns>
        public override PlayerBullet[] GetBullets(int weaponLevel, Vector2 playerFirePos)
        {
            TBullet[] ret = new TBullet[]
            {
                PoolManager.Instance.BulletPool.Get<TBullet>(playerFirePos, weaponLevel)
            };

            return ret;
        }

        /// <summary>
        /// Gets a single bullet from the associated Bullet Pool,
        /// gives it a specified <paramref name="velocity"/>, and wraps it into an array.
        /// </summary>
        /// <param name="weaponLevel">The current weapon level.</param>
        /// <param name="playerFirePos">The current position from which to fire the bullet.</param>
        /// <param name="velocity">The velocity to give to the bullet.</param>
        /// <returns>A single bullet wrapped in an array.</returns>
        public override PlayerBullet[] GetBullets(int weaponLevel, Vector2 playerFirePos, Vector2 velocity)
        {
            TBullet bullet = PoolManager.Instance.BulletPool.Get<TBullet>(playerFirePos, velocity, weaponLevel);
            TBullet[] ret = new TBullet[]
            {
                bullet
            };

            return ret;
        }
    }
}

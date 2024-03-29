﻿using System;
using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.FireStrategyManagers;
using Assets.ObjectPooling;
using Assets.UI.SpriteBank;
using Assets.Util;
using UnityEngine;

namespace Assets.FireStrategies.PlayerFireStrategies
{
    /// <summary>
    /// Contains the logic used to spawn player bullets in accordance with the functionality of the bullet type.
    /// </summary>
    /// <inheritdoc/>
    public abstract class PlayerFireStrategy : FireStrategy<PlayerBullet>
    {
        /// <summary>
        /// The name used to represent this fire strategy.
        /// </summary>
        public string StrategyName { get; }

        /// <summary>
        /// Whether or not to immediately update this bullet when it's first fired.
        /// Useful, for example, for HighVelocityPlayerBullets so that the bullets
        /// immediately spawn with a trail, instead of clumped at the spawn point.
        /// </summary>
        public virtual bool UpdateOnFire => true;

        public virtual string NotificationName(int weaponLevel)
        {
            string ret;
            if (weaponLevel > 0)
                ret = $"{StrategyName}!\r\nMk. {weaponLevel}";
            else
                ret = $"{StrategyName}!";

            return ret;
        }

        protected abstract float GetFireSpeedRatio(in PlayerFireStrategyManager.PlayerRatio ratios);

        protected float InitialFireSpeed(in PlayerFireStrategyManager manager)
            => manager.BaseFireSpeed * GetFireSpeedRatio(in manager.PlayerRatios);

        public PlayerFireStrategy(PlayerBullet bulletPrefab, in PlayerFireStrategyManager manager) : base(bulletPrefab)
        {
            FireTimer = InitialFireTimer(in manager);
            StrategyName = CalculateFireStrategyName();
        }

        protected LoopingFrameTimer InitialFireTimer(in PlayerFireStrategyManager manager)
        {
            float speed = InitialFireSpeed(in manager);
            return new LoopingFrameTimer(speed);
        }

        protected abstract Sprite GetPickupSprite(HeavyWeaponSpriteBank bank);
        public Sprite PickupSprite => GetPickupSprite(SpriteBank.HeavyWeapons);

        public abstract PlayerBullet[] GetBullets(int weaponLevel, Vector3 playerFirePos);
        public abstract PlayerBullet[] GetBullets(int weaponLevel, Vector3 playerFirePos, Vector2 velocity);

        public virtual void OnActivate() { }

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

        protected virtual string CalculateFireStrategyName()
        {
            string name = this.GetType().Name;
            name = name.Replace("Strategy", "");

            string ret = StringUtil.AddSpacesBeforeCapitals(name);
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
        public override PlayerBullet[] GetBullets(int weaponLevel, Vector3 playerFirePos)
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
        public override PlayerBullet[] GetBullets(int weaponLevel, Vector3 playerFirePos, Vector2 velocity)
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

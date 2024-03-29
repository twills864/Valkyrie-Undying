using System;
using System.Linq;
using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Enemies;
using Assets.FireStrategyManagers;
using Assets.ObjectPooling;
using Assets.Powerups.Balance;
using Assets.UI.SpriteBank;
using Assets.Util;
using UnityEngine;

namespace Assets.FireStrategies.PlayerFireStrategies
{
    /// <summary>
    /// Fires a single Reflect bullet directly up.
    /// </summary>
    /// <inheritdoc/>
    public class ReflectStrategy : PlayerFireStrategy<ReflectBullet>
    {
        public ReflectStrategy(ReflectBullet bullet, in PlayerFireStrategyManager manager) : base(bullet, manager)
        {
        }

        protected override Sprite GetPickupSprite(HeavyWeaponSpriteBank bank) => bank.Reflect;

        protected override float GetFireSpeedRatio(in PlayerFireStrategyManager.PlayerRatio ratios)
            => ratios.Reflect;
    }
}

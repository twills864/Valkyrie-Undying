using System;
using System.Linq;
using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Enemies;
using Assets.FireStrategyManagers;
using Assets.ObjectPooling;
using Assets.Powerups.Balance;
using Assets.Util;
using UnityEngine;

namespace Assets.FireStrategies.PlayerFireStrategies
{
    /// <summary>
    ///
    /// </summary>
    /// <inheritdoc/>
    public class ReflectStrategy : PlayerFireStrategy<ReflectBullet>
    {
        public ReflectStrategy(ReflectBullet bullet, in PlayerFireStrategyManager manager) : base(bullet, manager)
        {
        }

        protected override float GetFireSpeedRatio(in PlayerFireStrategyManager.PlayerRatio ratios)
            => ratios.Reflect;

        public override PlayerBullet[] GetBullets(int weaponLevel, Vector3 playerFirePos)
        {
            var ret = base.GetBullets(weaponLevel, playerFirePos);
            return ret;
        }
    }
}

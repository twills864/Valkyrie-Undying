using Assets.Bullets.PlayerBullets;
using Assets.FireStrategyManagers;
using Assets.Util;
using UnityEngine;

namespace Assets.FireStrategies.PlayerFireStrategies
{
    /// <inheritdoc/>
    public class DefaultStrategy : PlayerFireStrategy<DefaultBullet>
    {
        public DefaultStrategy(DefaultBullet bullet, in PlayerFireStrategyManager manager) : base(bullet, manager)
        {
        }

        protected override float GetFireSpeedRatio(in PlayerFireStrategyManager.PlayerRatio ratios) => 1.0f;

    }
}
